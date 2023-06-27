using Acme.Base.Domain.CosmosDb.Aggregate;
using Acme.Base.Domain.Entity;
using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common.Domain.Entity;
using Acme.FlightManager.FlightDirector.Domain.Factory;

namespace Acme.FlightManager.FlightDirector.Domain.Entity;

public abstract class BaseFlightDirectorEntity<TKey> : CosmosDbAggregateRoot, IIdentityEntity<TKey>
    where TKey : IdValueObject
{
    protected BaseFlightDirectorEntity(IdValueObject id, IRoute route)
        : base(id, new FlightDirectorPartitionKeyFactory(route)) { }

    public TKey MainId { get; private set; }
}