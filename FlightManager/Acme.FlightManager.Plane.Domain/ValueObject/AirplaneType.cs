using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common;
using System;
using System.Collections.Generic;

namespace Acme.FlightManager.Plane.Domain.ValueObject;

public class AirplaneType : BaseValueObject
{
    public string Family { get; private set; }
    public AirplaneManufacturer Manufacturer { get; private set; }
    public AirplaneTypeAbbreviation Type { get; private set; }

    private AirplaneType(string family, AirplaneManufacturer manufacturer, AirplaneTypeAbbreviation type) =>
        (Family, Manufacturer, Type) = (family, manufacturer, type);

    public static AirplaneType Create(
        string family,
        AirplaneManufacturer manufacturer,
        AirplaneTypeAbbreviation type)
    {
        if (string.IsNullOrWhiteSpace(family))
            throw new ArgumentNullException(nameof(family));

        return new AirplaneType(family, manufacturer, type);
    }

    public override string ToString() =>
        $"{Manufacturer} {Family}";

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return Manufacturer;
        yield return Family;
    }
}