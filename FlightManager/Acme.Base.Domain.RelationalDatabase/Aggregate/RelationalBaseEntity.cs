using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Factory;
using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.Base.Domain.RelationalDatabase.Aggregate;

public abstract class RelationalBaseEntity : BaseEntity
{
    [ConcurrencyCheck] // ToDo: remove and generalize in EF configurations (remove nuget Annotations)
    protected Guid Version { get; private set; }

    protected RelationalBaseEntity(IIdentityFactory<Guid> identityFactory) : base(identityFactory) { }
}