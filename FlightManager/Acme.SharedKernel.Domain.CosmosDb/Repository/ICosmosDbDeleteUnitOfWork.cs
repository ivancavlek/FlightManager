using Acme.SharedKernel.Domain.CosmosDb.Aggregate;

namespace Acme.SharedKernel.Domain.CosmosDb.Repository;

public interface ICosmosDbDeleteUnitOfWork : ICosmosDbUpsertUnitOfWork
{
    ICosmosDbDeleteUnitOfWork Delete(CosmosDbBaseEntity aggregateRoot);
}