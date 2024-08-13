using Acme.SharedKernel.Domain.CosmosDb.Aggregate;
using Acme.SharedKernel.Domain.Repository;

namespace Acme.SharedKernel.Domain.CosmosDb.Repository;

public interface ICosmosDbUpsertUnitOfWork : IUnitOfWork
{
    ICosmosDbUpsertUnitOfWork Upsert(CosmosDbBaseEntity aggregateRoot);
}