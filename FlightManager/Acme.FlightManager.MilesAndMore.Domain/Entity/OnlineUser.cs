using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common.Domain;
using Acme.FlightManager.Common.Domain.Entity;
using Acme.FlightManager.Common.Domain.ValueObject;
using System;

namespace Acme.FlightManager.MilesAndMore.Domain.Entity;

public class OnlineUser : BaseOnlineUserEntity<OnlineUserId>, IPassengerInformation
{
    public Gender Gender { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int MilesPoints { get; private set; }
    public string Username { get; private set; }

    protected OnlineUser(IdValueObject id) : base(id) { }
}