using Acme.SharedKernel.Domain.CosmosDb.Aggregate;
using Acme.SharedKernel.Domain.CosmosDb.Factory;
using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.SharedKernel.Domain.CosmosDb.Repository;

public interface ICosmosDbRepository
{
    Task<IReadOnlyCollection<TAggregateRoot>> GetAllAsync<TAggregateRoot>(
        Expression<Func<TAggregateRoot, bool>> query, IPartitionKeyFactory partitionKeyFactory)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id, IPartitionKeyFactory partitionKeyFactory)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}