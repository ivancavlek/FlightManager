using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Acme.FlightManager.Plane.Domain.ValueObject;

public class AirplaneRegistration : BaseValueObject
{
    public Country Country { get; private set; }
    public string Registration { get; private set; }

    private AirplaneRegistration() { }

    private AirplaneRegistration(Country country, string airplaneRegistration)
    {
        Country = country;
        Registration = airplaneRegistration;
    }

    public static AirplaneRegistration Create(Country country, string airplaneRegistration) =>
        country switch
        {
            Country.DE => new GermanAirplaneRegistration(country, airplaneRegistration),
            _ => throw new NotImplementedException(country.ToString())
        };

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return Registration;
    }

    private class GermanAirplaneRegistration : AirplaneRegistration
    {
        private List<ValidationFailure> _validationErrors;

        internal GermanAirplaneRegistration(Country country, string airplaneRegistration)
            : base(country, airplaneRegistration)
        {
            if (Registration is null)
                AddValidationError("value");
            if (Registration.Length != 6)
                AddValidationError("length");
            if (!Registration.StartsWith("D-A"))
                AddValidationError("registration start");

            CheckForErrorsInValidations();
        }

        private void AddValidationError(string invalidProperty) =>
            (_validationErrors ??= new()).Add(new("airplaneRegistration", $"Invalid {invalidProperty}", Registration));

        private void CheckForErrorsInValidations()
        {
            if (_validationErrors?.Count > 0)
                throw new ValidationException(_validationErrors);
        }
    }
}