using System;
using System.Security.Cryptography;

namespace Acme.Base.Domain.Factory;

//public sealed class GuidIdentityFactory : IIdentityFactory<Guid>
//{
//    Guid IIdentityFactory<Guid>.CreateIdentity() =>
//        MassTransit.NewId.NextSequentialGuid();
//}

/// <summary>
/// <see href="http://www.codeproject.com/Articles/388157/GUIDs-as-fast-primary-keys-under-multiple-database">GUID Factory credits. Optimized by me.</see>
/// </summary>
public sealed class GuidIdentityFactory : IIdentityFactory<Guid>
{
    Guid IIdentityFactory<Guid>.CreateIdentity()
    {
        Span<byte> guidBytes = stackalloc byte[16];

        using var rng = RandomNumberGenerator.Create();
        Span<byte> randomBytes = stackalloc byte[10];
        rng.GetBytes(randomBytes);

        for (var i = 0; i < 10; i++)
            guidBytes[i] = randomBytes[i];

        var timestamp = DateTime.UtcNow.Ticks / 10000L;
        Span<byte> timestampBytes = BitConverter.GetBytes(timestamp);

        if (BitConverter.IsLittleEndian)
            timestampBytes.Reverse();

        for (var (i, j) = (10, 2); i < 16; i++, j++)
            guidBytes[i] = timestampBytes[j];

        return new Guid(guidBytes);
    }
}