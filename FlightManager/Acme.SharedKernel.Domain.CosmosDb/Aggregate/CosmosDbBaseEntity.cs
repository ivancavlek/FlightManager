using Acme.SharedKernel.Domain.CosmosDb.Factory;
using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.Factory;
using System;

namespace Acme.SharedKernel.Domain.CosmosDb.Aggregate;

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