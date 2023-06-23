using Acme.Base.Domain.RelationalDatabase.Aggregate;

namespace Acme.Base.Domain.RelationalDatabase.Repository;

public interface IRelationalDbDeleteUnitOfWork : IRelationalDbInsertUnitOfWork
{
    IRelationalDbDeleteUnitOfWork Delete<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : RelationalAggregateRoot;
}