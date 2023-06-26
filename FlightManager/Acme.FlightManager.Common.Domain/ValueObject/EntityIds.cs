using Acme.Base.Domain.ValueObject;
using System;

namespace Acme.FlightManager.Common.Domain.ValueObject;

public class DestinationId : IdValueObject
{
    protected DestinationId(Guid id) : base(id) { }
}

public class FlightId : IdValueObject
{
    protected FlightId(Guid id) : base(id) { }
}

public class OnlineUserId : IdValueObject
{
    protected OnlineUserId(Guid id) : base(id) { }
}

public class PlaneConfigurationId : IdValueObject
{
    protected PlaneConfigurationId(Guid id) : base(id) { }
}

public class RouteId : IdValueObject
{
    protected RouteId(Guid id) : base(id) { }
}

public class TicketId : IdValueObject
{
    protected TicketId(Guid id) : base(id) { }
}