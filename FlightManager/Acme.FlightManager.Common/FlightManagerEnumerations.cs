using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.FlightManager.Common;

public static class EnumerationService
{
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
    A320,
    A320neo,
    A380,
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