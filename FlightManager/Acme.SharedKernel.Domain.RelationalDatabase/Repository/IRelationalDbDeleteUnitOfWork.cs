using Acme.SharedKernel.Domain.RelationalDatabase.Aggregate;

namespace Acme.SharedKernel.Domain.RelationalDatabase.Repository;

public interface IRelationalDbDeleteUnitOfWork : IRelationalDbInsertUnitOfWork
{
    IRelationalDbDeleteUnitOfWork Delete<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : RelationalBaseEntity;
}