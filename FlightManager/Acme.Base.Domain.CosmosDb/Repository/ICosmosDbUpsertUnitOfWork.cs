using Acme.Base.Domain.CosmosDb.Aggregate;
using Acme.Base.Domain.Repository;

namespace Acme.Base.Domain.CosmosDb.Repository;

public interface ICosmosDbUpsertUnitOfWork : IUnitOfWork
{
    ICosmosDbUpsertUnitOfWork Upsert<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : CosmosDbAggregateRoot;
}