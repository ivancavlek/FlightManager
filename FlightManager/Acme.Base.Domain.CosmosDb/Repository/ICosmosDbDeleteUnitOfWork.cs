using Acme.Base.Domain.CosmosDb.Aggregate;

namespace Acme.Base.Domain.CosmosDb.Repository;

public interface ICosmosDbDeleteUnitOfWork : ICosmosDbUpsertUnitOfWork
{
    ICosmosDbDeleteUnitOfWork Delete<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : CosmosDbAggregateRoot;
}