using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common.Domain;
using Acme.FlightManager.Common.Domain.Entity;
using Acme.FlightManager.Common.Domain.ValueObject;
using System;

namespace Acme.FlightManager.FlightDirector.Domain.Entity;

public class Ticket : BaseFlightDirectorEntity<TicketId>, IPassengerInformation
{
    public int Seat { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    protected Ticket(IdValueObject id, IRoute route) : base(id, route) { }

    public static Ticket Create(IPassengerInformation passengerInformation)
    {
        throw new NotImplementedException();
    }
}