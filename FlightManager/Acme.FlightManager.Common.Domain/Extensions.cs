using Acme.Base.Domain.Entity;
using Newtonsoft.Json;

namespace Acme.FlightManager.Common.Domain;

public static class Extensions
{
    public static TDto ConvertTo<TDto>(this BaseEntity baseEntity) =>
        JsonConvert.DeserializeObject<TDto>(JsonConvert.SerializeObject(baseEntity));
}