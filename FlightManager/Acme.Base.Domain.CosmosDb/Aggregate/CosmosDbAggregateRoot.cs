using Acme.Base.Domain.Aggregate;
using Acme.Base.Domain.CosmosDb.Factory;
using Acme.Base.Domain.ValueObject;
using Newtonsoft.Json;

namespace Acme.Base.Domain.CosmosDb.Aggregate;

public class CosmosDbAggregateRoot<TKey> : AggregateRoot<TKey>
    where TKey : IdValueObject
{
    public string Discriminator { get; private set; }
    [JsonProperty("_etag")]
    public string ETag { get; private set; }
    public string PartitionKey { get; private set; }

    protected CosmosDbAggregateRoot(TKey id) : base(id) { }

    public CosmosDbAggregateRoot(IPartitionKeyFactory partitionKeyFactory, TKey id)
        : base(id)
    {
        Discriminator = GetType().Name;
        PartitionKey = partitionKeyFactory/*.Throw(nameof(partitionKeyFactory))*/.CreatePartitionKey();
    }
}