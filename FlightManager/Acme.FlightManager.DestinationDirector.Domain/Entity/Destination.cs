using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Factory;
using Acme.FlightManager.DestinationDirector.Domain.ValueObject;
using System;
using System.Collections.ObjectModel;

namespace Acme.FlightManager.DestinationDirector.Domain.Entity;

public class Destination : BaseEntity, IAggregateRoot
{
    public IataCode AirportCode { get; private set; }
    public string City { get; private set; }
    public ReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    protected Destination(IIdentityFactory<Guid> identityFactory) : base(identityFactory) { }
}