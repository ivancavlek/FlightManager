using Acme.Base.Domain.Aggregate;
using Acme.Base.Domain.ValueObject;
using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.Base.Domain.RelationalDatabase.Aggregate;

public class SqlServerAggregateRoot<TKey> : AggregateRoot<TKey>
    where TKey : IdValueObject
{
    [ConcurrencyCheck]
    protected Guid Version { get; private set; }
}