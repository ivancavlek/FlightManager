using System.Collections.ObjectModel;

namespace Acme.Base.Domain.Entity;

public interface IAggregateRoot
{
    public ReadOnlyCollection<IDomainEvent> DomainEvents { get; }
}