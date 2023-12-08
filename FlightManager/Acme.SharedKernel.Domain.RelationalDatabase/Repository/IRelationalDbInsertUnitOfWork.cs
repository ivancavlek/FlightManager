using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.RelationalDatabase.Aggregate;
using Acme.SharedKernel.Domain.Repository;
using Acme.SharedKernel.Domain.ValueObject;

namespace Acme.SharedKernel.Domain.RelationalDatabase.Repository;

public interface IRelationalDbInsertUnitOfWork : IUnitOfWork
{
    IRelationalDbInsertUnitOfWork Insert<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : RelationalBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}