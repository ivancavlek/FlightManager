using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Factory;
using Acme.FlightManager.Common.Domain.Entity;
using System;
using System.Collections.ObjectModel;

namespace Acme.FlightManager.DestinationDirector.Domain.ValueObject;

public class Route : BaseEntity, IAggregateRoot, IRoute
{
    // ToDo: Could we play with Graph Database?
    // ToDo: maybe with actual coordinates to calculate hours
    public string Destination { get; private set; }
    public int DistanceInKilometers { get; private set; }
    public int FlightTimeInMinutes { get; private set; }
    public string PointOfDeparture { get; private set; }
    public ReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    public Route(IIdentityFactory<Guid> identityFactory, IataCode pointOfDeparture, IataCode destination)
        : base(identityFactory)
    {
        PointOfDeparture = pointOfDeparture.InternationalAirportCode;
        Destination = destination.InternationalAirportCode;
    }
}