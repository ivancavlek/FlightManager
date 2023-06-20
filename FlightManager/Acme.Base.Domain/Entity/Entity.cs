using Acme.Base.Domain.ValueObject;

namespace Acme.Base.Domain.Entity;

public abstract class Entity<TKey>
    where TKey : IdValueObject
{
    public TKey Id { get; private set; }

    protected Entity() { }

    protected Entity(TKey id) =>
        Id = id/*.Throw()*/;

    public override bool Equals(object obj) =>
        Equals(obj as Entity<TKey>);

    public bool Equals(Entity<TKey> otherEntity) =>
        ReferenceEquals(otherEntity, this) && otherEntity.Id.Equals(Id);

    public override int GetHashCode() =>
        Id.GetHashCode();

    public static bool operator ==(Entity<TKey> x, Entity<TKey> y) =>
        Equals(x, y);

    public static bool operator !=(Entity<TKey> x, Entity<TKey> y) =>
        !(x == y);
}