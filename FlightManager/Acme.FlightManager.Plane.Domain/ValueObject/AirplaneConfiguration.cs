using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common;
using System;
using System.Collections.Generic;

namespace Acme.FlightManager.Plane.Domain.ValueObject;

public class AirplaneConfiguration : BaseValueObject
{
    public int NumberOfSeats { get; private set; }
    public PlaneConfigurationType PlaneConfigurationType { get; private set; }
    public int RangeInKilometers { get; private set; }

    private AirplaneConfiguration() { }

    private AirplaneConfiguration(
        int maximumRangeInKilometers, int maximumSeats, double percentageInKilometers, double percentageSeats)
    {
        RangeInKilometers = (int)(percentageInKilometers * maximumRangeInKilometers);
        NumberOfSeats = (int)(percentageSeats * maximumSeats);
    }

    internal static AirplaneConfiguration Create(
        PlaneConfigurationType planeConfigurationType, int maximumRangeInKilometers, int maximumSeats) =>
        planeConfigurationType switch
        {
            PlaneConfigurationType.Business => new BusinessAirplaneConfiguration(maximumRangeInKilometers, maximumSeats),
            PlaneConfigurationType.Compact => new CompactAirplaneConfiguration(maximumRangeInKilometers, maximumSeats),
            PlaneConfigurationType.Premium => new PremiumAirplaneConfiguration(maximumRangeInKilometers, maximumSeats),
            _ => throw new NotImplementedException()
        };

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return NumberOfSeats;
        yield return PlaneConfigurationType;
        yield return RangeInKilometers;
    }

    private class BusinessAirplaneConfiguration : AirplaneConfiguration
    {
        internal BusinessAirplaneConfiguration(int maximumRangeInKilometers, int maximumSeats)
            : base(maximumRangeInKilometers, maximumSeats, percentageInKilometers: 0.75D, percentageSeats: 0.8D) =>
            PlaneConfigurationType = PlaneConfigurationType.Business;
    }

    private class CompactAirplaneConfiguration : AirplaneConfiguration
    {
        internal CompactAirplaneConfiguration(int maximumRangeInKilometers, int maximumSeats)
            : base(maximumRangeInKilometers, maximumSeats, percentageInKilometers: 0.6D, percentageSeats: 1D) =>
            PlaneConfigurationType = PlaneConfigurationType.Compact;
    }

    private class PremiumAirplaneConfiguration : AirplaneConfiguration
    {
        internal PremiumAirplaneConfiguration(int maximumRangeInKilometers, int maximumSeats)
            : base(maximumRangeInKilometers, maximumSeats, percentageInKilometers: 0.9D, percentageSeats: 0.6D) =>
            PlaneConfigurationType = PlaneConfigurationType.Premium;
    }
}