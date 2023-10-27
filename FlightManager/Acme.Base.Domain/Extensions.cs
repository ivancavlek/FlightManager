using Acme.Base.Domain.Entity;
using Acme.Base.Domain.ValueObject;
using System.Text.Json;

namespace Acme.Base.Domain;

public static class Extensions
{
    public static TDto ConvertTo<TDto>(this BaseEntity baseEntity) =>
        ConvertObjectTo<TDto>(baseEntity);

    public static TDto ConvertTo<TDto>(this BaseValueObject baseValueObject) =>
        ConvertObjectTo<TDto>(baseValueObject);

    public static string ConvertToString(this IIntegrationEvent integrationEvent) =>
        ConvertObjectTo<string>(integrationEvent);

    private static TDto ConvertObjectTo<TDto>(object value) =>
        JsonSerializer.Deserialize<TDto>(JsonSerializer.Serialize(value));
}