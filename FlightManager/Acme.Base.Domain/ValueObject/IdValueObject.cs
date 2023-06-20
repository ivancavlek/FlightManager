using Acme.Base.Domain.Factory;
using ErrorOr;
using System;
using System.Collections.Generic;

namespace Acme.Base.Domain.ValueObject;

public abstract class IdValueObject : ValueObject
{
    public Guid Value { get; private set; }

    protected IdValueObject(Guid value) =>
        Value = value;

    public override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return Value;
    }

    public abstract ErrorOr<IdValueObject> Create(IIdentityFactory<Guid> identityFactory);
}