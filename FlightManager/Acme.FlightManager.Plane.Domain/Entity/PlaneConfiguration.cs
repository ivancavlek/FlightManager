using Acme.Base.Domain.Factory;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.FlightManager.Common.Domain.Entity;
using System;

namespace Acme.FlightManager.Plane.Domain.Entity;

public class PlaneConfiguration : RelationalBaseEntity, IPlaneConfiguration
{
    public int FuelBurnPerHours { get; private set; }
    public int FuelCapacity { get; private set; }
    public int HoursOfFuel { get; private set; }
    public int MaxTakeOffWeight { get; private set; }
    public string PlaneType { get; private set; }
    public int Seats { get; private set; }
    public int Weight { get; private set; }

    protected PlaneConfiguration(IIdentityFactory<Guid> identityFactory) : base(identityFactory) { }

    public static PlaneConfiguration Create()
    {
        throw new NotImplementedException();
    }
}