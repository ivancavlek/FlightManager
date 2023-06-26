using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common.Domain.Entity;
using Acme.FlightManager.Common.Domain.ValueObject;
using System;

namespace Acme.FlightManager.Plane.Domain.Entity;

public class PlaneConfiguration : BasePlaneEntity<PlaneConfigurationId>, IPlaneConfiguration
{
    public int FuelBurnPerHours { get; private set; }
    public int FuelCapacity { get; private set; }
    public int HoursOfFuel { get; private set; }
    public int MaxTakeOffWeight { get; private set; }
    public string PlaneType { get; private set; }
    public int Seats { get; private set; }
    public int Weight { get; private set; }

    protected PlaneConfiguration(IdValueObject id) : base(id) { }

    public static PlaneConfiguration Create(PlaneConfigurationId id)
    {
        throw new NotImplementedException();
    }
}