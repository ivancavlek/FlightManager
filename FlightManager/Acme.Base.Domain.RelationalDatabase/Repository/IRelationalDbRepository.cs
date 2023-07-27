using Acme.Base.Domain.Entity;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.Base.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.Base.Domain.RelationalDatabase.Repository;

public interface IRelationalDbRepository
{
    Task<IReadOnlyCollection<TAggregateRoot>> GetAllAsync<TAggregateRoot>(Expression<Func<TAggregateRoot, bool>> query)
        where TAggregateRoot : RelationalBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id)
        where TAggregateRoot : RelationalBaseEntity, IAggregateRoot, IMainIdentity<IdValueObject>;
}