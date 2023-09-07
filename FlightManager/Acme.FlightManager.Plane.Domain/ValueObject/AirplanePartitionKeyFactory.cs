using Acme.Base.Domain.CosmosDb.Factory;

namespace Acme.FlightManager.Plane.Domain.ValueObject;

public sealed class AirplanePartitionKeyFactory : IPartitionKeyFactory
{
    string IPartitionKeyFactory.CreatePartitionKey()
    {
        throw new System.NotImplementedException();
    }
}