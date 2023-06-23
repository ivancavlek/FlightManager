using Acme.Base.Domain.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.Base.Domain.RelationalDatabase.Aggregate;

public abstract class RelationalAggregateRoot : BaseEntity, IAggregateRoot
{
    [ConcurrencyCheck]
    protected Guid Version { get; private set; }
}