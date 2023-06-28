using Acme.Base.Domain.Entity;
using Acme.Base.Domain.ValueObject;
using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.Base.Domain.RelationalDatabase.Aggregate;

public abstract class RelationalAggregateRoot : BaseEntity, IAggregateRoot
{
    [ConcurrencyCheck] // ToDo: remove and generalize in EF configurations (remove nuget Annotations)
    protected Guid Version { get; private set; }

    protected RelationalAggregateRoot(IdValueObject id) : base(id) { }
}