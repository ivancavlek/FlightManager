using Acme.Base.Domain.ValueObject;
using System;

namespace Acme.Base.Domain.Entity;

public abstract class BaseEntity
{
    //[JsonProperty("id")] // ToDo: For Cosmos DB, remove
    public Guid Id { get; private set; }

    protected BaseEntity() { }

    protected BaseEntity(IdValueObject idValueObject) =>
        Id = idValueObject.Value/*.Throw()*/;

    public override bool Equals(object obj) =>
        Equals(obj as BaseEntity);

    public bool Equals(BaseEntity otherEntity) =>
        ReferenceEquals(otherEntity, this) && otherEntity.Id.Equals(Id);

    public override int GetHashCode() =>
        Id.GetHashCode();

    public static bool operator ==(BaseEntity x, BaseEntity y) =>
        Equals(x, y);

    public static bool operator !=(BaseEntity x, BaseEntity y) =>
        !(x == y);
}