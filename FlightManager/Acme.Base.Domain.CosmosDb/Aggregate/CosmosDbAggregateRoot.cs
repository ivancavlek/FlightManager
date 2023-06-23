using Acme.Base.Domain.CosmosDb.Factory;
using Acme.Base.Domain.Entity;
using Acme.Base.Domain.ValueObject;

namespace Acme.Base.Domain.CosmosDb.Aggregate;

public abstract class CosmosDbAggregateRoot : BaseEntity, IAggregateRoot
{
    public string Discriminator { get; private set; }
    public string ETag { get; private set; }
    public string PartitionKey { get; private set; }

    protected CosmosDbAggregateRoot() { }

    protected CosmosDbAggregateRoot(IdValueObject id, IPartitionKeyFactory partitionKeyFactory) : base(id)
    {
        Discriminator = GetType().Name;
        PartitionKey = partitionKeyFactory/*.Throw(nameof(partitionKeyFactory))*/.CreatePartitionKey();
    }
}