using Acme.Base.Domain.Factory;
using System;
using System.Collections.Generic;

namespace Acme.Base.Domain.ValueObject;

public class IdValueObject : BaseValueObject
{
    public Guid Value { get; private set; }

    public IdValueObject(Guid id) =>
        Value = id;

    public static IdValueObject Create(IIdentityFactory<Guid> identityFactory) =>
        new(identityFactory.CreateIdentity());

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return Value;
    }
}