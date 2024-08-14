using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.Factory;
using Acme.SharedKernel.Domain.Service;
using System;

namespace Acme.SharedKernel.Domain.RelationalDatabase.Aggregate;

public abstract class RelationalBaseEntity : BaseEntity
{
    public DateTimeOffset UpdatedAt { get; private set; }

    protected RelationalBaseEntity() { }

    protected RelationalBaseEntity(IIdentityFactory<Guid> identityFactory) : base(identityFactory) { }

    public void SetUpdatedAt(ITimeService timeService) =>
        UpdatedAt = timeService.GetTime();
}