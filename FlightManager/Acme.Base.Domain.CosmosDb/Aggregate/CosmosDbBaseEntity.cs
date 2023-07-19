using Acme.Base.Domain.CosmosDb.ValueObject;
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

    protected CosmosDbBaseEntity(IIdentityFactory<Guid> identityFactory, DomainPartitionKey partitionKey) : base(identityFactory)
    {
        Discriminator = GetType().Name;
        PartitionKey = partitionKey is null ? Id.ToString() : partitionKey.Value;
    }
}