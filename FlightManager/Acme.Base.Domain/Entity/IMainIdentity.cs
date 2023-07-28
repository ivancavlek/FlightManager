using Acme.Base.Domain.ValueObject;

namespace Acme.Base.Domain.Entity;

public interface IMainIdentity<out TIdentity> where TIdentity : IdValueObject
{
    TIdentity Id { get; }
}