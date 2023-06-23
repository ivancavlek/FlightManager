using Acme.Base.Domain.Entity;
using Acme.Base.Domain.ValueObject;
using System;

namespace Acme.FlightManager.FlightDirector.Domain.Entity;

public class PlaneType : BaseRelationalFlightDirectorEntity<PlaneTypeId>, IAggregateRoot
{
    protected PlaneType(IdValueObject id) : base(id) { }

    public static PlaneType Create(PlaneTypeId id)
    {
        throw new NotImplementedException();
    }
}

public class PlaneTypeId : IdValueObject
{
    protected PlaneTypeId(Guid id) : base(id) { }
}