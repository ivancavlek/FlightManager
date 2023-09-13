using Acme.Base.Domain.CosmosDb.Aggregate;
using Acme.Base.Domain.Entity;
using Acme.Base.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.Base.Domain.CosmosDb.Repository;

public interface ICosmosDbRepository
{
    Task<IReadOnlyCollection<TAggregateRoot>> GetAllAsync<TAggregateRoot>(
        Expression<Func<TAggregateRoot, bool>> query, string partitionKey)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id, string partitionKey)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}