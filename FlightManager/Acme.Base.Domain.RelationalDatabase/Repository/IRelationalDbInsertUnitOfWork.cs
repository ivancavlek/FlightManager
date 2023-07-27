using Acme.Base.Domain.Entity;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.Base.Domain.Repository;
using Acme.Base.Domain.ValueObject;

namespace Acme.Base.Domain.RelationalDatabase.Repository;

public interface IRelationalDbInsertUnitOfWork : IUnitOfWork
{
    IRelationalDbInsertUnitOfWork Insert<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : RelationalBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}