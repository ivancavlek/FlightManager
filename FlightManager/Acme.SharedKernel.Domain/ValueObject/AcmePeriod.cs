using Acme.SharedKernel.Domain.Service;
using System;
using System.Collections.Generic;

namespace Acme.SharedKernel.Domain.ValueObject;

public sealed class AcmePeriod : BaseValueObject
{
    public DateTimeOffset From { get; private set; }
    public DateTimeOffset? Until { get; private set; }

    private AcmePeriod() { }

    private AcmePeriod(DateTimeOffset from, DateTimeOffset? until)
    {
        From = from;
        Until = until;
    }

    public static AcmePeriod CreateFromNow(ITimeService timeService) =>
        new(timeService.GetTime(), null);

    public bool IsActive(DateTime clientTime) =>
        (From <= clientTime && clientTime < Until) ||
        (From < Until && clientTime <= From && clientTime < Until);

    public bool IsOverlappingWith(AcmePeriod period) =>
        From <= period.Until && period.From <= Until;

    protected override IEnumerable<object> PropertiesForEqualizing()
    {
        yield return From;
        yield return Until;
    }
}