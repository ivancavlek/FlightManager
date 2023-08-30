using Acme.Base.Domain.Service;
using System;

namespace Acme.FlightManager.Common.Domain.Service;

public class CurrentTimeFreezeService : ITimeService
{
    private readonly DateTimeOffset _time;

    public CurrentTimeFreezeService(ITimeService timeService) =>
        _time = timeService.GetTime();

    DateTimeOffset ITimeService.GetTime() =>
        _time;
}