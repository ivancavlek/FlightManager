using System;

namespace Acme.Base.Domain.Service;

public interface ITimeService
{
    DateTimeOffset GetTime();
}