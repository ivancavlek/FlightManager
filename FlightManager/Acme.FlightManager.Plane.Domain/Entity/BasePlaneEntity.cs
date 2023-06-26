using Acme.Base.Domain.Entity;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.Base.Domain.ValueObject;

namespace Acme.FlightManager.Plane.Domain.Entity;

public abstract class BasePlaneEntity<TKey> : RelationalAggregateRoot, IIdentityEntity<TKey>
    where TKey : IdValueObject
{
    protected BasePlaneEntity(IdValueObject id) : base(id) { }

    public TKey MainId { get; }
}