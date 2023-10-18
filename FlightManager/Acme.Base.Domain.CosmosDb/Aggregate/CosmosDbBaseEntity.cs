using Acme.Base.Domain.CosmosDb.Factory;
using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Factory;
using System;

namespace Acme.Base.Domain.CosmosDb.Aggregate;

public abstract class CosmosDbBaseEntity : BaseEntity
{
    public string Discriminator { get; private set; }
    public string ETag { get; private set; }
    public string PartitionKey { get; private set; }

    protected CosmosDbBaseEntity() { }

    public CosmosDbBaseEntity(IIdentityFactory<Guid> identityFactory, IPartitionKeyFactory partitionKeyFactory)
        : base(identityFactory)
    {
        Discriminator = GetType().Name;
        PartitionKey = partitionKeyFactory is null ? id.ToString() : partitionKeyFactory.CreatePartitionKey();
    }
}