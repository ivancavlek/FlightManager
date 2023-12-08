namespace Acme.SharedKernel.Domain.Factory;

public interface IIdentityFactory<out TKey>
{
    TKey CreateIdentity();
}