using Acme.Base.Domain.ValueObject;

namespace Acme.Base.Domain.Entity;

public interface IIdentityEntity<TKey, TEntity>
    where TKey : IdValueObject
    where TEntity : BaseEntity
{
    public TKey MainId { get; }

    public TEntity Create(TKey key);
}