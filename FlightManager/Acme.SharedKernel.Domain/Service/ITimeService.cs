using System;

namespace Acme.SharedKernel.Domain.Service;

public interface ITimeService
{
    DateTimeOffset GetTime();
}