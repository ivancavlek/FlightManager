using Acme.Base.Domain.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.Base.Domain.Entity;

public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents;
    protected readonly Guid id;

    protected BaseEntity() { }

    protected BaseEntity(IIdentityFactory<Guid> identityFactory)
    {
        _domainEvents = new();
        id = identityFactory.CreateIdentity();
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() =>
        _domainEvents.ToList();

    public void ClearDomainEvents() =>
        _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    public override bool Equals(object obj) =>
        Equals(obj as BaseEntity);

    public bool Equals(BaseEntity otherEntity) =>
        ReferenceEquals(otherEntity, this) && otherEntity.id.Equals(id);

    public override int GetHashCode() =>
        id.GetHashCode();

    public static bool operator ==(BaseEntity x, BaseEntity y) =>
        Equals(x, y);

    public static bool operator !=(BaseEntity x, BaseEntity y) =>
        !(x == y);
}