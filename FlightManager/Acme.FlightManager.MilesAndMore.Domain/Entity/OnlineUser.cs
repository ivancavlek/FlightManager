using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common.Domain;
using Acme.FlightManager.Common.Domain.Entity;
using Acme.FlightManager.Common.Domain.ValueObject;

namespace Acme.FlightManager.MilesAndMore.Domain.Entity;

public class OnlineUser : BaseOnlineUserEntity<OnlineUserId>, IPassengerInformation
{
    public Gender Gender { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public int MilesPoints { get; private set; }
    public string Username { get; private set; }

    protected OnlineUser(IdValueObject id) : base(id) { }
}