using Acme.Base.Domain.CosmosDb.Factory;
using Acme.Base.Domain.ValueObject;
using System.Collections.Generic;

namespace Acme.Base.Domain.CosmosDb.ValueObject;

public sealed class DomainPartitionKey : BaseValueObject
{
    public string Value { get; private set; }

    private DomainPartitionKey(string value) =>
        Value = value;

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return Value;
    }

    public static DomainPartitionKey Create(IPartitionKeyFactory partitionKeyFactory) =>
        new(partitionKeyFactory.CreatePartitionKey());
}