using System;
using System.Collections.Generic;

namespace Acme.Base.Domain.ValueObject;

public abstract class IdValueObject : BaseValueObject
{
    public Guid Value { get; private set; }

    protected IdValueObject(Guid id) =>
        Value = id;

    //public static IdValueObject Create(IIdentityFactory<Guid> identityFactory) =>
    //    new(identityFactory.CreateIdentity());

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return Value;
    }
}