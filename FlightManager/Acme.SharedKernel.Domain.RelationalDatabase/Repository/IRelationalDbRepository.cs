using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.RelationalDatabase.Aggregate;
using Acme.SharedKernel.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.SharedKernel.Domain.RelationalDatabase.Repository;

public interface IRelationalDbRepository
{
    Task<IReadOnlyCollection<TAggregateRoot>> GetAllAsync<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> query)
        where TAggregateRoot : RelationalBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id)
        where TAggregateRoot : RelationalBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}