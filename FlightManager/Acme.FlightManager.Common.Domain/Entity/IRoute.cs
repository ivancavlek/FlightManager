namespace Acme.FlightManager.Common.Domain.Entity;

public interface IRoute
{
    public string Destination { get; }
    public string PointOfDeparture { get; }
}