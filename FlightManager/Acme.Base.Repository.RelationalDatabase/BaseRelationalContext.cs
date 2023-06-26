using Acme.Base.Domain.RelationalDatabase.Repository;
using Acme.Base.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.Base.Repository.RelationalDatabase;

public abstract class BaseContext : DbContext, IRelationalDbRepository, IRelationalDbDeleteUnitOfWork
{
    protected BaseContext(DbContextOptions<BaseContext> options) : base(options) { }

    async Task<IReadOnlyCollection<TAggregateRoot>> IRelationalDbRepository.GetAllAsync<TAggregateRoot>(
        Expression<Func<TAggregateRoot, bool>> query) =>
        await Set<TAggregateRoot>().Where(query).ToListAsync();

    async Task<TAggregateRoot> IRelationalDbRepository.GetSingleAsync<TAggregateRoot>(Guid id) =>
        await Set<TAggregateRoot>().SingleOrDefaultAsync(art => art.Id.Equals(id));

    Task IUnitOfWork.CommitAsync() =>
        SaveChangesAsync();

    IRelationalDbDeleteUnitOfWork IRelationalDbDeleteUnitOfWork.Delete<TAggregateRoot>(TAggregateRoot aggregateRoot)
    {
        Set<TAggregateRoot>().Remove(aggregateRoot);

        return this;
    }

    IRelationalDbInsertUnitOfWork IRelationalDbInsertUnitOfWork.Insert<TAggregateRoot>(TAggregateRoot aggregateRoot)
    {
        Set<TAggregateRoot>().Add(aggregateRoot);

        return this;
    }
}