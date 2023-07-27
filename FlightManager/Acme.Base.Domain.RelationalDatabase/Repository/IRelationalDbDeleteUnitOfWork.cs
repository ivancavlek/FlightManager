using Acme.Base.Domain.Entity;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.Base.Domain.ValueObject;

namespace Acme.Base.Domain.RelationalDatabase.Repository;

public interface IRelationalDbDeleteUnitOfWork : IRelationalDbInsertUnitOfWork
{
    IRelationalDbDeleteUnitOfWork Delete<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : RelationalBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}