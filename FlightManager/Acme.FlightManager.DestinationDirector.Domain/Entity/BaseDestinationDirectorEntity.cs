using Acme.Base.Domain.Entity;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.Base.Domain.ValueObject;

namespace Acme.FlightManager.Plane.Domain.Entity;

// ToDo: Could we play with Graph Database?
public abstract class BaseDestinationDirectorEntity<TKey> : RelationalAggregateRoot, IIdentityEntity<TKey>
    where TKey : IdValueObject
{
    protected BaseDestinationDirectorEntity(IdValueObject id) : base(id) { }

    public TKey MainId { get; }
}