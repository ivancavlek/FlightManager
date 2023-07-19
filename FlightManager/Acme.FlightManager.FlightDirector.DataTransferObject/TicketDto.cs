using Acme.FlightManager.Common;
using System;

namespace Acme.FlightManager.FlightDirector.DataTransferObject;

public record TicketDto
{
    public TicketId Id { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Seat { get; set; }
}

public record TicketId
{
    public Guid Value { get; set; }
}