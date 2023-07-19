using Acme.Base.Domain.Factory;
using System;

namespace Acme.Base.Domain.Entity;

public abstract class BaseEntity
{
    public Guid Id { get; private set; }

    protected BaseEntity() { }

    protected BaseEntity(IIdentityFactory<Guid> identityFactory) =>
        Id = identityFactory.CreateIdentity();

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