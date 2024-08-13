using Acme.SharedKernel.Domain.RelationalDatabase.Aggregate;
using Acme.SharedKernel.Domain.Repository;

namespace Acme.SharedKernel.Domain.RelationalDatabase.Repository;

public interface IRelationalDbInsertUnitOfWork : IUnitOfWork
{
    IRelationalDbInsertUnitOfWork Insert<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : RelationalBaseEntity;
}