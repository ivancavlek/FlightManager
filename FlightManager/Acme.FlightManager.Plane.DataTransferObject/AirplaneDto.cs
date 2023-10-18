using Acme.FlightManager.Common;
using System;

namespace Acme.FlightManager.Plane.DataTransferObject;

public sealed record CreatedAirplaneDto(AirplaneIdDto Id, AirplaneRegistrationDto Registration);

public sealed record AirplaneDto(
    AcmePeriodDto Active,
    AirplaneConfigurationDto Configuration,
    AirplaneRegistrationDto Registration,
    AirplaneTypeDto Type,
    AirplaneIdDto Id);

public sealed record AcmePeriodDto(DateTimeOffset From, DateTimeOffset? Until);

public sealed record AirplaneConfigurationDto(
    int NumberOfSeats, PlaneConfigurationType PlaneConfigurationType, int RangeInKilometers);

public sealed record AirplaneRegistrationDto(Country Country, string Registration);

public sealed record AirplaneTypeDto(AirplaneManufacturer Manufacturer, AirplaneTypeAbbreviation Type);

public sealed record AirplaneIdDto(Guid Value);