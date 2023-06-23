using Acme.Base.Domain.Entity;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.Base.Domain.ValueObject;

namespace Acme.FlightManager.FlightDirector.Domain.Entity;

public abstract class BaseRelationalFlightDirectorEntity<TKey> : RelationalAggregateRoot, IIdentityEntity<TKey>
    where TKey : IdValueObject
{
    protected BaseRelationalFlightDirectorEntity(IdValueObject id) : base(id) { }

    public TKey MainId { get; }
}