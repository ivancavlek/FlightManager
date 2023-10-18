using Acme.Base.Domain.CosmosDb.Factory;
using Acme.FlightManager.Common;

namespace Acme.FlightManager.Plane.Domain.Factory;

public sealed class AirplanePartitionKeyFactory : IPartitionKeyFactory
{
    string IPartitionKeyFactory.CreatePartitionKey() =>
        $"{nameof(AcmeApplications.Plane)}";
}