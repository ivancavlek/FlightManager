using Acme.SharedKernel.Domain.ValueObject;

namespace Acme.SharedKernel.Domain.Entity;

public interface IMainIdentity<out TIdentity> where TIdentity : IdValueObject
{
    TIdentity Id { get; }
}