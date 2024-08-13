using Acme.SharedKernel.Domain.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.SharedKernel.Domain.Entity;

public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents;

    public Guid Id { get; init; }

    protected BaseEntity() { }

    protected BaseEntity(IIdentityFactory<Guid> identityFactory)
    {
        _domainEvents = [];
        Id = identityFactory.CreateIdentity();
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
        ReferenceEquals(otherEntity, this) && otherEntity.Id.Equals(Id);

    public override int GetHashCode() =>
        Id.GetHashCode();

    public static bool operator ==(BaseEntity x, BaseEntity y) =>
        Equals(x, y);

    public static bool operator !=(BaseEntity x, BaseEntity y) =>
        !(x == y);
}