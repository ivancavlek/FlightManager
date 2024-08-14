using Acme.SharedKernel.Domain.RelationalDatabase.Aggregate;
using Acme.SharedKernel.Domain.RelationalDatabase.Repository;
using Acme.SharedKernel.Domain.Repository;
using Acme.SharedKernel.Domain.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.Base.Repository.RelationalDatabase;

public abstract class BaseContext : DbContext, IRelationalDbRepository, IRelationalDbDeleteUnitOfWork
{
    private readonly ITimeService _timeService;

    protected BaseContext(ITimeService timeService, DbContextOptions<BaseContext> options) : base(options) =>
        _timeService = timeService;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<RelationalBaseEntity>()
            .Property(ppy => ppy.UpdatedAt)
            .IsConcurrencyToken();
    }

    async Task<IReadOnlyCollection<TAggregateRoot>> IRelationalDbRepository.GetAllAsync<TAggregateRoot>(
        Expression<Func<TAggregateRoot, bool>> query, CancellationToken cancellationToken) =>
        await Set<TAggregateRoot>().Where(query).ToListAsync(cancellationToken);

    async Task<TAggregateRoot> IRelationalDbRepository.GetSingleAsync<TAggregateRoot>(
        Guid id, CancellationToken cancellationToken) =>
        await Set<TAggregateRoot>().SingleOrDefaultAsync(art => art.Id.Equals(id), cancellationToken);

    Task IUnitOfWork.CommitAsync(CancellationToken cancellationToken)
    {
        var upsertedEntries = ChangeTracker.Entries()
            .Where(e => e.Entity is RelationalBaseEntity &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in upsertedEntries)
            ((RelationalBaseEntity)entry.Entity).SetUpdatedAt(_timeService);

        return SaveChangesAsync(cancellationToken);
    }

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