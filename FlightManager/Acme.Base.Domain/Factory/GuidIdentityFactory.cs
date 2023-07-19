using System;
using System.Security.Cryptography;

namespace Acme.Base.Domain.Factory;

//public sealed class GuidIdentityFactory : IIdentityFactory<Guid>
//{
//    Guid IIdentityFactory<Guid>.CreateIdentity() =>
//        MassTransit.NewId.NextSequentialGuid();
//}

/// <summary>
/// <see href="http://www.codeproject.com/Articles/388157/GUIDs-as-fast-primary-keys-under-multiple-database">GUID Factory credits</see>
/// </summary>
public sealed class GuidIdentityFactory : IIdentityFactory<Guid>
{
    Guid IIdentityFactory<Guid>.CreateIdentity()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[10];
        rng.GetBytes(randomBytes);

        var timestamp = DateTime.UtcNow.Ticks / 10000L;
        var timestampBytes = BitConverter.GetBytes(timestamp);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(timestampBytes);

        var guidBytes = new byte[16];

        Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);

        return new Guid(guidBytes);
    }
}