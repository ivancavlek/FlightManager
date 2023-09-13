using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common;
using Acme.FlightManager.Text;
using System.Collections.Generic;

namespace Acme.FlightManager.Plane.Domain.ValueObject;

public class AirplaneType : BaseValueObject
{
    public AirplaneManufacturer Manufacturer { get; private set; }
    public AirplaneTypeAbbreviation Type { get; private set; }

    private AirplaneType() { }

    private AirplaneType(AirplaneManufacturer manufacturer, AirplaneTypeAbbreviation type) =>
        (Manufacturer, Type) = (manufacturer, type);

    public static AirplaneType Create(AirplaneManufacturer manufacturer, AirplaneTypeAbbreviation type) =>
        new(manufacturer, type);

    public override string ToString() =>
        Type.AirplaneTypeAbbreviationText();

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return Manufacturer;
        yield return Type;
    }
}