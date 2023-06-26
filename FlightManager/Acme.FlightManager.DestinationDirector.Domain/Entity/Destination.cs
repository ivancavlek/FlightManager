using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common.Domain.ValueObject;
using Acme.FlightManager.DestinationDirector.Domain.ValueObject;
using Acme.FlightManager.Plane.Domain.Entity;

namespace Acme.FlightManager.DestinationDirector.Domain.Entity;

public class Destination : BaseDestinationDirectorEntity<DestinationId>
{
    public IataCode AirportCode { get; private set; }
    public string City { get; private set; }

    protected Destination(IdValueObject id) : base(id) { }
}