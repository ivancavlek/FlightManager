using Acme.Base.Domain.Entity;
using Acme.Base.Domain.ValueObject;

namespace Acme.Base.Domain.Aggregate;

public abstract class AggregateRoot<TKey> : Entity<TKey>
    where TKey : IdValueObject
{
    protected AggregateRoot() { }

    protected AggregateRoot(TKey id) : base(id) { }
}