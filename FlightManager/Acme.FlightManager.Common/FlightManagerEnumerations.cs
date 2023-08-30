using Acme.FlightManager.Text;
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

public enum AcmeApplications
{
    DestinationDirector,
    FlightDirector,
    MilesAndMore,
    Plane
}

public enum AirplaneManufacturer
{
    Airbus,
    Boeing
}

public enum AirplaneTypeAbbreviation
{
    [Display(Description = "A320", ResourceType = typeof(FlightManagerMessage))]
    A320,
    [Display(Description = "A320neo", ResourceType = typeof(FlightManagerMessage))]
    A320neo,
    [Display(Description = "A380", ResourceType = typeof(FlightManagerMessage))]
    A380,
    [Display(Description = "Boeing7478", ResourceType = typeof(FlightManagerMessage))]
    Boeing7478
}

public enum Country
{
    Germany
}

public enum Gender
{
    Female,
    Male
}

public enum PlaneConfigurationType
{
    Business,
    Compact,
    Premium
}