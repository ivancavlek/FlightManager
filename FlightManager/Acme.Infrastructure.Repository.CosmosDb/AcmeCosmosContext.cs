﻿using Acme.SharedKernel.Domain.CosmosDb.Factory;
using Acme.SharedKernel.Domain.CosmosDb.Repository;
using Acme.SharedKernel.Domain.Repository;
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
    private readonly Container _container;
    private TransactionalBatch _transaction;
    private int _transactionCount;

    public AcmeCosmosContext(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        var database = cosmosClient.GetDatabase(databaseName);
        _container = database.GetContainer(containerName);
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
        Expression<Func<TAggregateRoot, bool>> query, IPartitionKeyFactory partitionKeyFactory)
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
            var result = await iterator.ReadNextAsync().ConfigureAwait(false);
            //var index = result.IndexMetrics;
            //var cost = result.RequestCharge;
            //var diagnostics = result.Diagnostics;
            results.AddRange(result);
        }

        return results;
    }

    async Task<TAggregateRoot> ICosmosDbRepository.GetSingleAsync<TAggregateRoot>(Guid id) // ToDo: I vote for simple type, because this library can be then reused, otherwise it's bound to specific projects
    {
        try
        {
            return await _container
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

    async Task<TAggregateRoot> ICosmosDbRepository.GetSingleAsync<TAggregateRoot>(
        Guid id, IPartitionKeyFactory partitionKeyFactory)
    {
        try
        {
            return await _container
                .ReadItemAsync<TAggregateRoot>(id.ToString(), new PartitionKey(partitionKeyFactory.CreatePartitionKey()))
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

        _transaction ??= _container.CreateTransactionalBatch(new PartitionKey(aggregateRoot.PartitionKey));

        var options = new TransactionalBatchItemRequestOptions { IfMatchEtag = aggregateRoot.ETag };

        _transaction.UpsertItem(aggregateRoot, options);
        ++_transactionCount;

        return this;
    }

    ICosmosDbDeleteUnitOfWork ICosmosDbDeleteUnitOfWork.Delete<TAggregateRoot>(TAggregateRoot aggregateRoot)
    {
        if (aggregateRoot is null)
            return this;

        _transaction ??= _container.CreateTransactionalBatch(new PartitionKey(aggregateRoot.PartitionKey));

        _transaction.DeleteItem(aggregateRoot.Id.Value.ToString());
        ++_transactionCount;

        return this;
    }
}