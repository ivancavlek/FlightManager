using Acme.Base.Domain.Entity;
using Acme.Base.Domain.ValueObject;
using Newtonsoft.Json;

namespace Acme.FlightManager.Common.Domain;

public static class Extensions
{
    public static TDto ConvertTo<TDto>(this BaseEntity baseEntity) =>
        ConvertObjectTo<TDto>(baseEntity);

    public static TDto ConvertTo<TDto>(this BaseValueObject baseValueObject) =>
        ConvertObjectTo<TDto>(baseValueObject);

    private static TDto ConvertObjectTo<TDto>(object value) =>
        JsonConvert.DeserializeObject<TDto>(JsonConvert.SerializeObject(value));
}