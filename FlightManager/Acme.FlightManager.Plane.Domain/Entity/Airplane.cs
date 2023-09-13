using Acme.Base.Domain.CosmosDb.Aggregate;
using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Factory;
using Acme.Base.Domain.Service;
using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common;
using Acme.FlightManager.Plane.Domain.Factory;
using Acme.FlightManager.Plane.Domain.ValueObject;
using Acme.FlightManager.Text;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Acme.FlightManager.Plane.Domain.Entity;

public class Airplane : CosmosDbBaseEntity/*RelationalBaseEntity*/, IMainIdentity<AirplaneId>, IAggregateRoot
{
    private int _maximumRangeInKilometers;
    private int _maximumSeats;

    public AcmePeriod Active { get; private set; }
    public AirplaneConfiguration Configuration { get; private set; }
    public AirplaneRegistration Registration { get; private set; }
    public AirplaneType Type { get; private set; }
    public AirplaneId Id => new(id);

    private class A320Airplane : Airplane
    {
        public A320Airplane(string airplaneRegistration, ITimeService timeService)
            : base(airplaneRegistration, new GuidIdentityFactory())
        {
            _maximumRangeInKilometers = 6_100;
            _maximumSeats = 186;
            Active = AcmePeriod.CreateFromNow(timeService);
            Type = AirplaneType.Create(AirplaneManufacturer.Airbus, AirplaneTypeAbbreviation.A320);
        }
    }

    private class A320neoAirplane : Airplane
    {
        public A320neoAirplane(string airplaneRegistration, ITimeService timeService)
            : base(airplaneRegistration, new GuidIdentityFactory())
        {
            _maximumRangeInKilometers = 6_500;
            _maximumSeats = 195;
            Active = AcmePeriod.CreateFromNow(timeService);
            Type = AirplaneType.Create(AirplaneManufacturer.Airbus, AirplaneTypeAbbreviation.A320neo);
        }
    }

    private class A380Airplane : Airplane
    {
        public A380Airplane(string airplaneRegistration, ITimeService timeService)
            : base(airplaneRegistration, new GuidIdentityFactory())
        {
            _maximumRangeInKilometers = 14_800;
            _maximumSeats = 853;
            Active = AcmePeriod.CreateFromNow(timeService);
            Type = AirplaneType.Create(AirplaneManufacturer.Airbus, AirplaneTypeAbbreviation.A380);
        }
    }

    private class Boeing7478Airplane : Airplane
    {
        public Boeing7478Airplane(string airplaneRegistration, ITimeService timeService)
            : base(airplaneRegistration, new GuidIdentityFactory())
        {
            _maximumRangeInKilometers = 14_320;
            _maximumSeats = 467;
            Active = AcmePeriod.CreateFromNow(timeService);
            Type = AirplaneType.Create(AirplaneManufacturer.Boeing, AirplaneTypeAbbreviation.Boeing7478);
        }
    }

    private Airplane() { }

    private Airplane(string airplaneRegistration, IIdentityFactory<Guid> identityFactory)
        : base(identityFactory, new AirplanePartitionKeyFactory(airplaneRegistration)) { }

    public static Airplane AddAirplaneToTheFleet(
        AirplaneTypeAbbreviation type,
        PlaneConfigurationType configuration,
        Country country,
        string airplaneRegistration,
        ITimeService timeService)
    {
        var newAirplaneInTheFleet = GetAirplaneType();

        newAirplaneInTheFleet.SetConfiguration(configuration);
        newAirplaneInTheFleet.SetRegistration(country, airplaneRegistration);

        return newAirplaneInTheFleet;

        Airplane GetAirplaneType() =>
            type switch
            {
                AirplaneTypeAbbreviation.A320 => new A320Airplane(airplaneRegistration, timeService),
                AirplaneTypeAbbreviation.A320neo => new A320neoAirplane(airplaneRegistration, timeService),
                AirplaneTypeAbbreviation.A380 => new A380Airplane(airplaneRegistration, timeService),
                AirplaneTypeAbbreviation.Boeing7478 => new Boeing7478Airplane(airplaneRegistration, timeService),
                _ => throw new NotImplementedException(),
            };
    }

    public void SetConfiguration(PlaneConfigurationType configuration) =>
        Configuration = AirplaneConfiguration.Create(configuration, _maximumRangeInKilometers, _maximumSeats);

    public void SetRegistration(Country country, string airplaneRegistration) =>
        Registration = AirplaneRegistration.Create(country, airplaneRegistration);

    public static ReadOnlyCollection<string> GetAirplaneTypesInTheFleet() =>
        EnumerationService.GetEnumValues<AirplaneTypeAbbreviation>()
            .Select(ata => ata.AirplaneTypeAbbreviationText())
            .ToList()
            .AsReadOnly();
}

public class AirplaneId : IdValueObject
{
    internal protected AirplaneId(Guid id) : base(id) { }
}