using Acme.Base.Domain.Factory;
using ErrorOr;
using System;
using System.Collections.Generic;

namespace Acme.Base.Domain.ValueObject;

public class IdValueObject : BaseValueObject
{
    internal Guid Value { get; private set; }

    protected IdValueObject(Guid value) =>
        Value = value;

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return Value;
    }

    public static ErrorOr<TIdValueObject> Create<TIdValueObject>(IIdentityFactory<Guid> identityFactory)
        where TIdValueObject : IdValueObject =>
        new IdValueObject(identityFactory.CreateIdentity()) as TIdValueObject;
}