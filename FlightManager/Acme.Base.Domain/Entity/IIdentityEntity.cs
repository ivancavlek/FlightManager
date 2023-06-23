using Acme.Base.Domain.ValueObject;

namespace Acme.Base.Domain.Entity;

// used to prevent "configuration hell" for EF and Cosmos, but enables programmers to avoid primitive obsession
public interface IIdentityEntity<TKey>
    where TKey : IdValueObject
{
    public TKey MainId { get; }
}