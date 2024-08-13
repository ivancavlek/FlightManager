using Acme.SharedKernel.Domain.CosmosDb.Aggregate;
using Acme.SharedKernel.Domain.CosmosDb.Factory;
using Acme.SharedKernel.Domain.CosmosDb.Repository;
using Acme.SharedKernel.Domain.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Base.Repository.CosmosDb;

public sealed class AcmeCosmosContext : ICosmosDbUpsertUnitOfWork, ICosmosDbDeleteUnitOfWork, ICosmosDbRepository
{
    private readonly Container _container;
    private TransactionalBatch _transaction;
    private int _transactionCount;

    public AcmeCosmosContext(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        var database = cosmosClient.GetDatabase(databaseName);
        _container = database.GetContainer(containerName);
    }

    async Task IUnitOfWork.CommitAsync(CancellationToken cancellationToken)
    {
        if (_transactionCount > 0)
        {
            using var response = await _transaction.ExecuteAsync(cancellationToken).ConfigureAwait(false);

            _transactionCount = 0;

            if (!response.IsSuccessStatusCode)
                throw new CosmosException(
                    "Transaction is not commited", response.StatusCode, default, response.ActivityId, response.RequestCharge);
        }
    }

    async Task<IReadOnlyCollection<TAggregateRoot>> ICosmosDbRepository.GetAllAsync<TAggregateRoot>(
        Expression<Func<TAggregateRoot, bool>> query,
        IPartitionKeyFactory partitionKeyFactory,
        CancellationToken cancellationToken)
    {
        var iterator = _container
            .GetItemLinqQueryable<TAggregateRoot>(
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(partitionKeyFactory.CreatePartitionKey()),
                    PopulateIndexMetrics = true
                }) // ToDo: remove PopulateIndexMetrics in prod, watch RU costs
            .Where(query)
            .ToFeedIterator();

        var results = new List<TAggregateRoot>();

        while (iterator.HasMoreResults)
        {
            var result = await iterator.ReadNextAsync(cancellationToken).ConfigureAwait(false);
            //var index = result.IndexMetrics;
            //var cost = result.RequestCharge;
            //var diagnostics = result.Diagnostics;
            results.AddRange(result);
        }

        return results;
    }

    async Task<TAggregateRoot> ICosmosDbRepository.GetSingleAsync<TAggregateRoot>(
        Guid id, CancellationToken cancellationToken)
    {
        try
        {
            return await _container
                .ReadItemAsync<TAggregateRoot>(
                    id.ToString(), new PartitionKey(id.ToString()), cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (CosmosException ex) when (ItemIsNotFound(ex))
        {
            return null;
        }
        catch (AggregateException ex) when (ItemIsNotFound(ex.InnerException))
        {
            return null;
        }

        static bool ItemIsNotFound(Exception ex) =>
            ex is CosmosException &&
            (ex as CosmosException).StatusCode == HttpStatusCode.NotFound;
    }

    async Task<TAggregateRoot> ICosmosDbRepository.GetSingleAsync<TAggregateRoot>(
        Guid id, IPartitionKeyFactory partitionKeyFactory, CancellationToken cancellationToken)
    {
        try
        {
            return await _container
                .ReadItemAsync<TAggregateRoot>(
                    id.ToString(),
                    new PartitionKey(partitionKeyFactory.CreatePartitionKey()),
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (CosmosException ex) when (ItemIsNotFound(ex))
        {
            return null;
        }
        catch (AggregateException ex) when (ItemIsNotFound(ex.InnerException))
        {
            return null;
        }

        static bool ItemIsNotFound(Exception ex) =>
            ex is CosmosException &&
            (ex as CosmosException).StatusCode == HttpStatusCode.NotFound;
    }

    ICosmosDbUpsertUnitOfWork ICosmosDbUpsertUnitOfWork.Upsert(CosmosDbBaseEntity aggregateRoot)
    {
        if (aggregateRoot is null)
            return this;

        _transaction ??= _container.CreateTransactionalBatch(new PartitionKey(aggregateRoot.PartitionKey));

        var options = new TransactionalBatchItemRequestOptions { IfMatchEtag = aggregateRoot.ETag };

        _transaction.UpsertItem(aggregateRoot, options);
        ++_transactionCount;

        return this;
    }

    ICosmosDbDeleteUnitOfWork ICosmosDbDeleteUnitOfWork.Delete(CosmosDbBaseEntity aggregateRoot)
    {
        if (aggregateRoot is null)
            return this;

        _transaction ??= _container.CreateTransactionalBatch(new PartitionKey(aggregateRoot.PartitionKey));

        _transaction.DeleteItem(aggregateRoot.Id.ToString());
        ++_transactionCount;

        return this;
    }
}