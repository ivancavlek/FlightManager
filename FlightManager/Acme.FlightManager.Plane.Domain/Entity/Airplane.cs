﻿using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Factory;
using Acme.Base.Domain.RelationalDatabase.Aggregate;
using Acme.Base.Domain.Service;
using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common;
using Acme.FlightManager.Plane.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.FlightManager.Plane.Domain.Entity;

public class Airplane : RelationalBaseEntity, IMainIdentity<AirplaneId>, IAggregateRoot
{
    private int _maximumRangeInKilometers;
    private int _maximumSeats;

    public AcmePeriod Active { get; private set; }
    public int MaximumRangeInKilometers { get; private set; }
    public int MaximumSeats { get; private set; }
    public AirplaneConfiguration Configuration { get; private set; }
    public AirplaneRegistration Registration { get; private set; }
    public AirplaneType Type { get; private set; }
    public AirplaneId Id => new(id);

    private class A320Airplane : Airplane
    {
        public A320Airplane(ITimeService timeService) : base(new GuidIdentityFactory())
        {
            Active = AcmePeriod.CreateFromNow(timeService);
            MaximumRangeInKilometers = _maximumRangeInKilometers = 6_100;
            MaximumSeats = _maximumSeats = 186;
            Type = AirplaneType.Create("A320", AirplaneManufacturer.Airbus, AirplaneTypeAbbreviation.A320);
        }
    }

    private class A320neoAirplane : Airplane
    {
        public A320neoAirplane(ITimeService timeService) : base(new GuidIdentityFactory())
        {
            Active = AcmePeriod.CreateFromNow(timeService);
            MaximumRangeInKilometers = _maximumRangeInKilometers = 6_500;
            MaximumSeats = _maximumSeats = 195;
            Type = AirplaneType.Create("A320neo", AirplaneManufacturer.Airbus, AirplaneTypeAbbreviation.A320neo);
        }
    }

    private class A380Airplane : Airplane
    {
        public A380Airplane(ITimeService timeService) : base(new GuidIdentityFactory())
        {
            Active = AcmePeriod.CreateFromNow(timeService);
            MaximumRangeInKilometers = _maximumRangeInKilometers = 14_800;
            MaximumSeats = _maximumSeats = 853;
            Type = AirplaneType.Create("A380", AirplaneManufacturer.Airbus, AirplaneTypeAbbreviation.A380);
        }
    }

    private class Boeing7478Airplane : Airplane
    {
        public Boeing7478Airplane(ITimeService timeService) : base(new GuidIdentityFactory())
        {
            Active = AcmePeriod.CreateFromNow(timeService);
            MaximumRangeInKilometers = _maximumRangeInKilometers = 14_320;
            MaximumSeats = _maximumSeats = 467;
            Type = AirplaneType.Create("747-8", AirplaneManufacturer.Boeing, AirplaneTypeAbbreviation.Boeing7478);
        }
    }

    private Airplane() { }

    private Airplane(IIdentityFactory<Guid> identityFactory) : base(identityFactory) { }

    public static Airplane AddAirplaneToTheFleet(
        AirplaneTypeAbbreviation type,
        PlaneConfigurationType configuration,
        Country country,
        string aircraftRegistration,
        ITimeService timeService)
    {
        Airplane airplane = type switch
        {
            AirplaneTypeAbbreviation.A320 => new A320Airplane(timeService),
            AirplaneTypeAbbreviation.A320neo => new A320neoAirplane(timeService),
            AirplaneTypeAbbreviation.A380 => new A380Airplane(timeService),
            AirplaneTypeAbbreviation.Boeing7478 => new Boeing7478Airplane(timeService),
            _ => throw new NotImplementedException(),
        };
        airplane.SetAirplaneConfiguration(configuration);
        airplane.SetAirplaneRegistration(country, aircraftRegistration);

        return airplane;
    }

    public void SetAirplaneConfiguration(PlaneConfigurationType configuration)
    {
        Configuration = AirplaneConfiguration.Create(configuration);
        MaximumRangeInKilometers = (int)(Configuration.PercentageInKilometers * _maximumRangeInKilometers);
        MaximumSeats = (int)(Configuration.PercentageSeats * _maximumSeats);
    }

    public void SetAirplaneRegistration(Country country, string airplaneRegistration) =>
        Registration = AirplaneRegistration.Create(country, airplaneRegistration);

    public static List<string> GetAirplaneTypesInTheFleet() =>
        new List<AirplaneTypeAbbreviation>()
        {
            AirplaneTypeAbbreviation.A320,
            AirplaneTypeAbbreviation.A320neo,
            AirplaneTypeAbbreviation.A380,
            AirplaneTypeAbbreviation.Boeing7478
        }
        .Select(ape => ape.GetDescription())
        .ToList();
}

public class AirplaneId : IdValueObject
{
    internal protected AirplaneId(Guid id) : base(id) { }
}