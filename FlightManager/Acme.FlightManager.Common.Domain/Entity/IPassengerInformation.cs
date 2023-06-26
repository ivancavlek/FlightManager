namespace Acme.FlightManager.Common.Domain.Entity;

public interface IPassengerInformation
{
    public Gender Gender { get; }
    public string FirstName { get; }
    public string LastName { get; }
}