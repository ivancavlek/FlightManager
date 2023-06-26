namespace Acme.FlightManager.Common.Domain.Entity;

public interface IPlaneConfiguration
{
    public string PlaneType { get; }
    public int Seats { get; }
}
