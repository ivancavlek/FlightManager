namespace Acme.SharedKernel.Domain.CosmosDb.Factory;

public interface IPartitionKeyFactory
{
    string CreatePartitionKey();
}