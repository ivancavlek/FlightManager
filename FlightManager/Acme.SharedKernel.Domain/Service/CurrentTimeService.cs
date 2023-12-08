using System;

namespace Acme.SharedKernel.Domain.Service;

public class CurrentTimeService : ITimeService
{
    DateTimeOffset ITimeService.GetTime() =>
        DateTimeOffset.Now;
}
