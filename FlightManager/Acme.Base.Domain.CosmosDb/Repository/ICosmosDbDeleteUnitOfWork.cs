using Acme.Base.Domain.CosmosDb.Aggregate;
using Acme.Base.Domain.Entity;
using Acme.Base.Domain.ValueObject;

namespace Acme.Base.Domain.CosmosDb.Repository;

public interface ICosmosDbDeleteUnitOfWork : ICosmosDbUpsertUnitOfWork
{
    ICosmosDbDeleteUnitOfWork Delete<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}