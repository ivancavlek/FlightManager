using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.SharedKernel.Domain.ValueObject;

public abstract class BaseValueObject : IEquatable<BaseValueObject>
{
    protected abstract IEnumerable<object> PropertiesForEqualizing();

    public override bool Equals(object obj)
    {
        if (obj is null || obj.GetType() != typeof(BaseValueObject))
            return false;

        var valueObject = (BaseValueObject)obj;

        return PropertiesForEqualizing().SequenceEqual(valueObject.PropertiesForEqualizing());
    }

    public override int GetHashCode() =>
        PropertiesForEqualizing().Aggregate(default(int), HashCode.Combine);

    bool IEquatable<BaseValueObject>.Equals(BaseValueObject other) =>
        Equals(other);

    public static bool operator ==(BaseValueObject left, BaseValueObject right) =>
        Equals(left, right);

    public static bool operator !=(BaseValueObject left, BaseValueObject right) =>
        !Equals(left, right);
}