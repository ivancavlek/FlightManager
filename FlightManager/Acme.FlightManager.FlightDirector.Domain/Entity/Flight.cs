using Acme.SharedKernel.Domain.CosmosDb.Aggregate;
using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.Factory;
using Acme.SharedKernel.Domain.ValueObject;
using Acme.FlightManager.Common;
using Acme.FlightManager.Common.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Acme.FlightManager.FlightDirector.Domain.Entity;

public sealed class Flight :
    CosmosDbBaseEntity,
    IMainIdentity<Flight.FlightId>,
    IAggregateRoot,
    IPlaneConfiguration,
    IRoute
{
    private readonly List<Ticket> _tickets;

    public int Hours { get; private set; }
    public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();
    public string PlaneType { get; private set; }
    public int Seats { get; private set; }
    public string Destination { get; private set; }
    public string PointOfDeparture { get; private set; }
    public Guid RouteId { get; private set; }
    public ReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    public FlightId Id => new(id);

    private Flight() { }

    private Flight(IIdentityFactory<Guid> identityFactory) : base(identityFactory, null) =>
        _tickets = new();

    public Ticket BuyTicketForOnlineUser(IPassengerInformation passengerInformation, int seat) =>
        BuyTicketForGuest(
            passengerInformation.DateOfBirth,
            passengerInformation.Gender,
            passengerInformation.FirstName,
            passengerInformation.LastName,
            seat);

    public Ticket BuyTicketForGuest(
        DateOnly dateOfBirth, Gender gender, string firstName, string lastName, int seat)
    {

        // manipulirati sa seats
        var ticket = Ticket.Create(this, dateOfBirth, gender, firstName, lastName, seat);

        _tickets.Add(ticket);

        return ticket;

        int GetAvailableSeat() =>
            seat.Equals(default) ? seat : 1;
    }

    public static Flight Create(
        IPlaneConfiguration planeConfiguration,
        Guid route)
    {
        return new(new GuidIdentityFactory());
    }

    public sealed class Ticket : BaseEntity, IMainIdentity<TicketId>, IPassengerInformation
    {
        public DateOnly DateOfBirth { get; private set; }
        public FlightId FlightId { get; private set; }
        public Gender Gender { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int Seat { get; private set; }
        public TicketId Id => new(id);

        private Ticket() { }

        private Ticket(
            IIdentityFactory<Guid> identityFactory,
            Flight flight,
            DateOnly dateOfBirth,
            Gender gender,
            string firstName,
            string lastName,
            int seat)
            : base(identityFactory)
        {
            DateOfBirth = dateOfBirth;
            FlightId = flight.Id;
            Gender = gender;
            FirstName = firstName;
            LastName = lastName;
            Seat = seat;
        }

        internal static Ticket Create(
            Flight flight, DateOnly dateOfBirth, Gender gender, string firstName, string lastName, int seat) =>
            new(new GuidIdentityFactory(), flight, dateOfBirth, gender, firstName, lastName, seat);
    }

    public class FlightId : IdValueObject
    {
        internal protected FlightId(Guid id) : base(id) { }
    }
}

public class TicketId : IdValueObject
{
    public TicketId(Guid id) : base(id) { }
}