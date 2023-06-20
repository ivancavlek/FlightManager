using MassTransit;
using System;

namespace Acme.Base.Domain.Factory;

public sealed class GuidIdentityFactory : IIdentityFactory<Guid>
{
    Guid IIdentityFactory<Guid>.CreateIdentity() =>
        NewId.NextSequentialGuid();
}