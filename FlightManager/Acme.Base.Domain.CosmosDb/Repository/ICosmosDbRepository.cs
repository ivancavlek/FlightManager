using Acme.Base.Domain.CosmosDb.Aggregate;
using Acme.Base.Domain.CosmosDb.ValueObject;
using Acme.Base.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.Base.Domain.CosmosDb.Repository;

public interface ICosmosDbRepository
{
    Task<IReadOnlyCollection<TAggregateRoot>> GetAllAsync<TAggregateRoot>(
        Expression<Func<TAggregateRoot, bool>> query, string partitionKey)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id, DomainPartitionKey partitionKey)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot;
}