using Acme.FlightManager.Common.Domain.Entity;
using Acme.FlightManager.Common.Domain.ValueObject;
using Acme.FlightManager.Plane.Domain.Entity;

namespace Acme.FlightManager.DestinationDirector.Domain.ValueObject;

public class Route : BaseDestinationDirectorEntity<RouteId>, IRoute
{
    // ToDo: maybe with actual coordinates to calculate hours
    public string Destination { get; private set; }
    public int DistanceInKilometers { get; private set; }
    public int FlightTimeInMinutes { get; private set; }
    public string PointOfDeparture { get; private set; }

    public Route(RouteId routeId, IataCode pointOfDeparture, IataCode destination) : base(routeId)
    {
        PointOfDeparture = pointOfDeparture.InternationalAirportCode;
        Destination = destination.InternationalAirportCode;
    }
}