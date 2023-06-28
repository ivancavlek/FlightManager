using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Text;
using FluentValidation;
using System.Collections.Generic;

namespace Acme.FlightManager.DestinationDirector.Domain.ValueObject;

public sealed class IataCode : BaseValueObject
{
    public string InternationalAirportCode { get; private set; }

    private IataCode(string internationalAirportCode) =>
        InternationalAirportCode = internationalAirportCode;

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return InternationalAirportCode;
    }

    public static IataCode Create(string internationalAirportCode)
    {
        return IsValid() ?
            new IataCode(internationalAirportCode.ToUpperInvariant()) :
            throw new ValidationException(GeneralFlightManagerMessage.Invalid(internationalAirportCode));

        bool IsValid() =>
            internationalAirportCode is not null && internationalAirportCode.Length.Equals(3);
    }
}