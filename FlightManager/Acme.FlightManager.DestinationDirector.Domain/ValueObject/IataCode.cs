using Acme.Base.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace Acme.FlightManager.DestinationDirector.Domain.ValueObject;

public class IataCode : BaseValueObject
{
    public string InternationalAirportCode { get; private set; }

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        throw new NotImplementedException();
    }
}