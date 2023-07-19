using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Factory;
using Acme.FlightManager.DestinationDirector.Domain.ValueObject;
using System;

namespace Acme.FlightManager.DestinationDirector.Domain.Entity;

public class Destination : BaseEntity, IAggregateRoot
{
    public IataCode AirportCode { get; private set; }
    public string City { get; private set; }

    protected Destination(IIdentityFactory<Guid> identityFactory) : base(identityFactory) { }
}