using Acme.SharedKernel.Domain.CosmosDb.Aggregate;
using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.ValueObject;

namespace Acme.SharedKernel.Domain.CosmosDb.Repository;

public interface ICosmosDbDeleteUnitOfWork : ICosmosDbUpsertUnitOfWork
{
    ICosmosDbDeleteUnitOfWork Delete<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}