using Acme.Base.Domain.Entity;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.Base.Domain.Repository;

namespace Acme.Base.Domain.RelationalDatabase.Repository;

public interface IRelationalDbInsertUnitOfWork : IUnitOfWork
{
    IRelationalDbInsertUnitOfWork Insert<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : RelationalBaseEntity, IAggregateRoot;
}