using Acme.Base.Domain.CosmosDb.Aggregate;
using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Repository;
using Acme.Base.Domain.ValueObject;

namespace Acme.Base.Domain.CosmosDb.Repository;

public interface ICosmosDbUpsertUnitOfWork : IUnitOfWork
{
    ICosmosDbUpsertUnitOfWork Upsert<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : CosmosDbBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}