using Acme.Base.Domain.Factory;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.FlightManager.Common;
using Acme.FlightManager.Common.Domain.Entity;
using System;

namespace Acme.FlightManager.MilesAndMore.Domain.Entity;

public class OnlineUser : RelationalBaseEntity, IPassengerInformation
{
    public Gender Gender { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int MilesAndMorePoints { get; private set; }
    public string Username { get; private set; }

    protected OnlineUser(IIdentityFactory<Guid> identityFactory) : base(identityFactory) { }

    public static OnlineUser Create()
    {
        throw new NotImplementedException();
    }
}