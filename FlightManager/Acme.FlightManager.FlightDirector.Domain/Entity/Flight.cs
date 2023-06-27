using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common.Domain.Entity;
using Acme.FlightManager.Common.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.FlightManager.FlightDirector.Domain.Entity;

public class Flight : BaseFlightDirectorEntity<FlightId>, IPlaneConfiguration, IRoute
{
    private List<Ticket> _tickets;

    public int Hours { get; private set; }
    public IReadOnlyCollection<Ticket> Tickets
    {
        get => _tickets.AsReadOnly();
        private set => _tickets = value.ToList();
    }
    public string PlaneType { get; private set; }
    public int Seats { get; private set; }
    public string Destination { get; private set; }
    public string PointOfDeparture { get; private set; }

    protected Flight(IdValueObject id, IRoute route) : base(id, route) { }

    public static Flight Create(
        FlightId id, PlaneConfigurationId planeConfigurationId, IPlaneConfiguration planeConfiguration, RouteId route)
    {
        throw new NotImplementedException();
    }
}