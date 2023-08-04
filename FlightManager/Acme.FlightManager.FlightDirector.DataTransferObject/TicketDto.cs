using Acme.FlightManager.Common;
using System;

namespace Acme.FlightManager.FlightDirector.DataTransferObject;

public record TicketDto(TicketIdDto Id, DateOnly DateOfBirth, Gender Gender, string FirstName, string LastName, int Seat);

public record TicketIdDto(Guid Value);

public record FlightDto(TicketIdDto Id, DateOnly DateOfBirth, Gender Gender, string FirstName, string LastName, int Seat);

public record FlightIdDto(Guid Value);