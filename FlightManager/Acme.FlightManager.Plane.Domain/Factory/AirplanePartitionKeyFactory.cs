using Acme.Base.Domain.CosmosDb.Factory;
using Acme.FlightManager.Common;

namespace Acme.FlightManager.Plane.Domain.Factory;

public sealed class AirplanePartitionKeyFactory : IPartitionKeyFactory
{
    private readonly string _airplaneRegistration;

    public AirplanePartitionKeyFactory(string airplaneRegistration) =>
        _airplaneRegistration = airplaneRegistration;

    string IPartitionKeyFactory.CreatePartitionKey() =>
        $"{AcmeApplications.Plane}-{_airplaneRegistration}";
}