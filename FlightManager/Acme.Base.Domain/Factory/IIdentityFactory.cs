namespace Acme.Base.Domain.Factory;

public interface IIdentityFactory<out TKey>
{
    TKey CreateIdentity();
}