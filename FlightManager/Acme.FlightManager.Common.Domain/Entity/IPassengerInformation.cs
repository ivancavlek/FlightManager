using FluentValidation;
using System;

namespace Acme.FlightManager.Common.Domain.Entity;

public interface IPassengerInformation
{
    public DateOnly DateOfBirth { get; }
    public Gender Gender { get; }
    public string FirstName { get; }
    public string LastName { get; }
}

public class PassengerInformationValidator : AbstractValidator<IPassengerInformation>
{
    public PassengerInformationValidator()
    {
        RuleFor(pin => pin.FirstName).NotEmpty();
        RuleFor(pin => pin.LastName).NotEmpty();
    }
}

public static class PassengerInformationExtensions
{
    public static TPassengerInformation Validate<TPassengerInformation>(this IPassengerInformation passengerInformation)
        where TPassengerInformation : class, IPassengerInformation
    {
        new PassengerInformationValidator().ValidateAndThrow(passengerInformation);

        return passengerInformation as TPassengerInformation;
    }
}