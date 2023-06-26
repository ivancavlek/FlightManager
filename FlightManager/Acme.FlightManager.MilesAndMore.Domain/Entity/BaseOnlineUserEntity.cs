using Acme.Base.Domain.Entity;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.Base.Domain.ValueObject;

namespace Acme.FlightManager.MilesAndMore.Domain.Entity;

public abstract class BaseOnlineUserEntity<TKey> : RelationalAggregateRoot, IIdentityEntity<TKey>
    where TKey : IdValueObject
{
    protected BaseOnlineUserEntity(IdValueObject id) : base(id) { }

    public TKey MainId { get; }
}