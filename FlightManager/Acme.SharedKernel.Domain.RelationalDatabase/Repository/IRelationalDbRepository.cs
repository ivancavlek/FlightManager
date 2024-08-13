using Acme.SharedKernel.Domain.RelationalDatabase.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.SharedKernel.Domain.RelationalDatabase.Repository;

public interface IRelationalDbRepository
{
    Task<IReadOnlyCollection<TAggregateRoot>> GetAllAsync<TAggregateRoot>(
        Expression<Func<TAggregateRoot, bool>> query, CancellationToken cancellationToken)
        where TAggregateRoot : RelationalBaseEntity;
    Task<TAggregateRoot> GetSingleAsync<TAggregateRoot>(Guid id, CancellationToken cancellationToken)
        where TAggregateRoot : RelationalBaseEntity;
}