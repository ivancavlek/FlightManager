namespace Acme.Base.Domain.CosmosDb.Factory;

public interface IPartitionKeyFactory
{
    string CreatePartitionKey();
}