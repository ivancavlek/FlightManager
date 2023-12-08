using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.ValueObject;

namespace Acme.SharedKernel.Domain.Repository;

public interface IDeleteUnitOfWork : IInsertUnitOfWork
{
    IDeleteUnitOfWork Delete<TAggregateRoot>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : BaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}