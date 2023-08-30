using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Acme.FlightManager.Plane.Domain.ValueObject;

public abstract class AirplaneRegistration : BaseValueObject
{
    public string Registration { get; private set; }

    private AirplaneRegistration(string aircraftRegistration) =>
        Registration = aircraftRegistration;

    public static AirplaneRegistration Create(Country country, string aircraftRegistration) =>
        country switch
        {
            Country.Germany => new GermanAircraftRegistration(aircraftRegistration),
            _ => throw new NotImplementedException(country.ToString())
        };

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return Registration;
    }

    private static ValidationFailure InvalidAirplaneRegistration(string aircraftRegistration) =>
        new("airplaneRegistration", "Invalid", aircraftRegistration.Length);

    private class GermanAircraftRegistration : AirplaneRegistration
    {
        public GermanAircraftRegistration(string airplaneRegistration) : base(airplaneRegistration)
        {
            if (airplaneRegistration.Length != 6 || !airplaneRegistration.StartsWith("D-A"))
                throw new ValidationException(
                    new List<ValidationFailure> { InvalidAirplaneRegistration(airplaneRegistration) });
        }
    }
}