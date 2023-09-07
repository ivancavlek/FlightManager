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

    private class GermanAircraftRegistration : AirplaneRegistration
    {
        private List<ValidationFailure> _validationErrors;

        internal GermanAircraftRegistration(string airplaneRegistration) : base(airplaneRegistration)
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