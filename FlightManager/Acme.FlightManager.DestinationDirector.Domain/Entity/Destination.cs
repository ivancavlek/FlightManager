using Acme.FlightManager.DestinationDirector.Domain.ValueObject;
using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.Factory;
using System;
using System.Collections.ObjectModel;

namespace Acme.FlightManager.DestinationDirector.Domain.Entity;

public class Destination : BaseEntity
{
    public IataCode AirportCode { get; private set; }
    public string City { get; private set; }
    public ReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    protected Destination(IIdentityFactory<Guid> identityFactory) : base(identityFactory) { }
}