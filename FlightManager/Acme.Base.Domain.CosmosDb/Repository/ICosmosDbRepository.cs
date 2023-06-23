using Acme.Base.Domain.CosmosDb.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.Base.Domain.CosmosDb.Repository;

public interface ICosmosDbRepository
{
    Task<IReadOnlyCollection<TAggregateRoot>> GetAllAsync<TAggregateRoot>(
        Expression<Func<TAggregateRoot, bool>> query, string partitionKey)
        where TAggregateRoot : CosmosDbAggregateRoot;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id, string partitionKey)
        where TAggregateRoot : CosmosDbAggregateRoot;
}