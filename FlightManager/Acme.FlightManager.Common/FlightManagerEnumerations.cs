using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Acme.FlightManager.Common;

public static class EnumerationService
{
    public static string GetDescription(this Enum genericEnum) =>
        genericEnum
            .GetType()
            .GetMember(genericEnum.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DisplayAttribute>()
            ?.GetDescription() ?? genericEnum.ToString();

    public static IEnumerable<TEnum> GetEnumValues<TEnum>() where TEnum : Enum =>
        Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

    public static TEnum GetParsedEnum<TEnum>(string valueToParse) where TEnum : Enum =>
        (TEnum)Enum.Parse(typeof(TEnum), valueToParse);
}

public enum Gender
{
    Female,
    Male
}