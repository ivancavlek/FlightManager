using Acme.Base.Domain.CosmosDb.Factory;
using Acme.Base.Domain.Entity;

namespace Acme.Base.Domain.CosmosDb.Aggregate;

public abstract class CosmosDbAggregateRoot : BaseEntity, IAggregateRoot
{
    public string Discriminator { get; private set; }
    public string ETag { get; private set; }
    public string PartitionKey { get; private set; }

    protected CosmosDbAggregateRoot() { }

    public CosmosDbAggregateRoot(IPartitionKeyFactory partitionKeyFactory)
    {
        Discriminator = GetType().Name;
        PartitionKey = partitionKeyFactory/*.Throw(nameof(partitionKeyFactory))*/.CreatePartitionKey();
    }
}