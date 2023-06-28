using Acme.Base.Domain.Factory;
using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common.Domain;
using Acme.FlightManager.Common.Domain.Entity;
using Acme.FlightManager.Common.Domain.ValueObject;
using FluentValidation;
using System;

namespace Acme.FlightManager.FlightDirector.Domain.Entity;

public class Ticket : BaseFlightDirectorEntity<TicketId>, IPassengerInformation
{
    public DateOnly DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int Seat { get; private set; }

    private Ticket(
        DateOnly dateOfBirth, Gender gender, string firstName, string lastName, int seat, IdValueObject id, IRoute route)
        : base(id, route)
    {
        DateOfBirth = dateOfBirth;
        Gender = gender;
        FirstName = firstName;
        LastName = lastName;
        Seat = seat;
    }

    public static Ticket CreateFromOnlineUser(
        IPassengerInformation passengerInformation, int seat, IRoute route)
    {
        var ticketId = IdValueObject.Create<TicketId>(new GuidIdentityFactory());

        var ticket = new Ticket(
            passengerInformation.DateOfBirth,
            passengerInformation.Gender,
            passengerInformation.FirstName,
            passengerInformation.LastName,
            seat,
            ticketId,
            route);

        return ticket;
    }

    public static Ticket CreateFromGuest(
        DateOnly dateOfBirth, Gender gender, string firstName, string lastName, int seat, IRoute route)
    {
        var ticketId = IdValueObject.Create<TicketId>(new GuidIdentityFactory());

        var ticket = new Ticket(dateOfBirth, gender, firstName, lastName, seat, ticketId, route);

        return ticket.Validate<Ticket>();
    }
}