using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.Factory;
using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.SharedKernel.Domain.RelationalDatabase.Aggregate;

public abstract class RelationalBaseEntity : BaseEntity
{
    [ConcurrencyCheck] // ToDo: remove and generalize in EF configurations (remove nuget Annotations)
    protected Guid Version { get; private set; }

    protected RelationalBaseEntity() { }

    protected RelationalBaseEntity(IIdentityFactory<Guid> identityFactory) : base(identityFactory) { }
}