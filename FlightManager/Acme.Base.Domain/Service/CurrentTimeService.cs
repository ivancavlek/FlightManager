using System;

namespace Acme.Base.Domain.Service;

public class CurrentTimeService : ITimeService
{
    DateTimeOffset ITimeService.GetTime() =>
        DateTimeOffset.Now;
}
