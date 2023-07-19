using Acme.Base.Domain.CosmosDb.Repository;
using Acme.Base.Domain.CosmosDb.ValueObject;
using Acme.Base.Domain.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Acme.Base.Repository.CosmosDb;

public sealed class AcmeCosmosContext : ICosmosDbUpsertUnitOfWork, ICosmosDbDeleteUnitOfWork, ICosmosDbRepository
{
    private readonly IReadOnlyCollection<Container> _containers;
    private TransactionalBatch _transaction;
    private int _transactionCount;

    public AcmeCosmosContext(CosmosClient cosmosClient, string databaseName, IEnumerable<string> containerNames)
    {
        var database = cosmosClient/*.MustNotBeNull(nameof(cosmosClient))*/.GetDatabase(databaseName);
        _containers = containerNames.Select(database.GetContainer).ToList();
    }

    async Task IUnitOfWork.CommitAsync()
    {
        if (_transactionCount > 0)
        {
            using var response = await _transaction.ExecuteAsync().ConfigureAwait(false);

            _transactionCount = 0;

            if (!response.IsSuccessStatusCode)
                throw new CosmosException(
                    "Transaction is not commited", response.StatusCode, default, response.ActivityId, response.RequestCharge);
        }
    }

    async Task<IReadOnlyCollection<TAggregateRoot>> ICosmosDbRepository.GetAllAsync<TAggregateRoot>(
        Expression<Func<TAggregateRoot, bool>> query, string partitionKey)
    {
        var iterator = GetContainer(partitionKey)
            .GetItemLinqQueryable<TAggregateRoot>(
                requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(partitionKey), PopulateIndexMetrics = true }) // ToDo: remove PopulateIndexMetrics in prod, watch RU costs
            .Where(query)
            .ToFeedIterator();

        var results = new List<TAggregateRoot>();

        while (iterator.HasMoreResults)
        {
            var result = await iterator.ReadNextAsync().ConfigureAwait(false);
            //var index = result.IndexMetrics;
            //var cost = result.RequestCharge;
            //var diagnostics = result.Diagnostics;
            results.AddRange(result);
        }

        return results;
    }

    async Task<TAggregateRoot> ICosmosDbRepository.GetSingleAsync<TAggregateRoot>(Guid id)
    {
        try
        {
            return await GetContainer(id.ToString())
                .ReadItemAsync<TAggregateRoot>(id.ToString(), new PartitionKey(id.ToString()))
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

    async Task<TAggregateRoot> ICosmosDbRepository.GetSingleAsync<TAggregateRoot>(Guid id, DomainPartitionKey partitionKey)
    {
        try
        {
            return await GetContainer(partitionKey.Value)
                .ReadItemAsync<TAggregateRoot>(id.ToString(), new PartitionKey(partitionKey.Value))
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

    ICosmosDbUpsertUnitOfWork ICosmosDbUpsertUnitOfWork.Upsert<TAggregateRoot>(TAggregateRoot aggregateRoot)
    {
        if (aggregateRoot is null)
            return this;

        _transaction ??= GetContainer(aggregateRoot.PartitionKey)
            .CreateTransactionalBatch(new PartitionKey(aggregateRoot.PartitionKey));

        var options = new TransactionalBatchItemRequestOptions { IfMatchEtag = aggregateRoot.ETag };

        _transaction.UpsertItem(aggregateRoot, options);
        ++_transactionCount;

        return this;
    }

    ICosmosDbDeleteUnitOfWork ICosmosDbDeleteUnitOfWork.Delete<TAggregateRoot>(TAggregateRoot aggregateRoot)
    {
        if (aggregateRoot is null)
            return this;

        _transaction ??= GetContainer(aggregateRoot.PartitionKey)
            .CreateTransactionalBatch(new PartitionKey(aggregateRoot.PartitionKey));

        _transaction.DeleteItem(aggregateRoot.Id.ToString());
        ++_transactionCount;

        return this;
    }

    private Container GetContainer(string partitionKey) =>
        _containers.Single(ctr => partitionKey.Contains(ctr.Id));
}