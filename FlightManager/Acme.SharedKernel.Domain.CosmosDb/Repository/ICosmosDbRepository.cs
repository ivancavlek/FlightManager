using Acme.SharedKernel.Domain.CosmosDb.Aggregate;
using Acme.SharedKernel.Domain.CosmosDb.Factory;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.SharedKernel.Domain.CosmosDb.Repository;

public interface ICosmosDbRepository
{
    Task<IReadOnlyCollection<TAggregateRoot>> GetAllAsync<TAggregateRoot>(
        Expression<Func<TAggregateRoot, bool>> query,
        IPartitionKeyFactory partitionKeyFactory,
        CancellationToken cancellationToken)
        where TAggregateRoot : CosmosDbBaseEntity;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id, CancellationToken cancellationToken)
        where TAggregateRoot : CosmosDbBaseEntity;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(
        Guid id, IPartitionKeyFactory partitionKeyFactory, CancellationToken cancellationToken)
        where TAggregateRoot : CosmosDbBaseEntity;
}