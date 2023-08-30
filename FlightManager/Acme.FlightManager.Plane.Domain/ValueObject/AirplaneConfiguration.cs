using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common;
using System;
using System.Collections.Generic;

namespace Acme.FlightManager.Plane.Domain.ValueObject;

public abstract class AirplaneConfiguration : BaseValueObject
{
    public double PercentageSeats { get; private set; }
    public double PercentageInKilometers { get; private set; }
    public PlaneConfigurationType PlaneConfigurationType { get; private set; }

    private AirplaneConfiguration() { }

    internal static AirplaneConfiguration Create(PlaneConfigurationType planeConfigurationType) =>
        planeConfigurationType switch
        {
            PlaneConfigurationType.Business => new BusinessAirplaneConfiguration(),
            PlaneConfigurationType.Compact => new CompactAirplaneConfiguration(),
            PlaneConfigurationType.Premium => new PremiumAirplaneConfiguration(),
            _ => throw new NotImplementedException()
        };

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return PercentageInKilometers;
        yield return PlaneConfigurationType;
        yield return PercentageSeats;
    }

    private class BusinessAirplaneConfiguration : AirplaneConfiguration
    {
        internal BusinessAirplaneConfiguration()
        {
            PercentageSeats = 0.8D;
            PercentageInKilometers = 0.75D;
            PlaneConfigurationType = PlaneConfigurationType.Business;
        }
    }

    private class CompactAirplaneConfiguration : AirplaneConfiguration
    {
        internal CompactAirplaneConfiguration()
        {
            PercentageSeats = 1D;
            PercentageInKilometers = 0.6D;
            PlaneConfigurationType = PlaneConfigurationType.Compact;
        }
    }

    private class PremiumAirplaneConfiguration : AirplaneConfiguration
    {
        internal PremiumAirplaneConfiguration()
        {
            PercentageSeats = 0.6D;
            PercentageInKilometers = 0.9D;
            PlaneConfigurationType = PlaneConfigurationType.Premium;
        }
    }
}