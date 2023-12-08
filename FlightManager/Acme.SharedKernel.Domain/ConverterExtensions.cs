using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.ValueObject;
using System.Text.Json;

namespace Acme.SharedKernel.Domain;

public static class ConverterExtensions
{
    public static TDto ConvertTo<TDto>(this BaseEntity baseEntity) =>
        ConvertObjectTo<TDto>(baseEntity);

    public static TDto ConvertTo<TDto>(this BaseValueObject baseValueObject) =>
        ConvertObjectTo<TDto>(baseValueObject);

    private static TDto ConvertObjectTo<TDto>(object value) =>
        JsonSerializer.Deserialize<TDto>(JsonSerializer.Serialize(value));
}