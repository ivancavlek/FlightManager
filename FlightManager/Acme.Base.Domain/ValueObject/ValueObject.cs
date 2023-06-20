using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.Base.Domain.ValueObject;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> PropertiesForEqualizing();

    public override bool Equals(object obj)
    {
        if (obj is null || obj.GetType() != typeof(ValueObject))
            return false;

        var valueObject = (ValueObject)obj;

        return PropertiesForEqualizing().SequenceEqual(valueObject.PropertiesForEqualizing());
    }

    public override int GetHashCode() =>
        PropertiesForEqualizing().Aggregate(default(int), HashCode.Combine);

    bool IEquatable<ValueObject>.Equals(ValueObject other) =>
        Equals(other);

    public static bool operator ==(ValueObject left, ValueObject right) =>
        Equals(left, right);

    public static bool operator !=(ValueObject left, ValueObject right) =>
        !Equals(left, right);
}