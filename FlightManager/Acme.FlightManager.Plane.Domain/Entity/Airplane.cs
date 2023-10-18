using Acme.Base.Domain.CosmosDb.Aggregate;
using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Factory;
using Acme.Base.Domain.Service;
using Acme.Base.Domain.ValueObject;
using Acme.FlightManager.Common;
using Acme.FlightManager.Plane.Domain.Factory;
using Acme.FlightManager.Plane.Domain.ValueObject;
using Acme.FlightManager.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Acme.FlightManager.Plane.Domain.Entity;

public class Airplane : CosmosDbBaseEntity/*RelationalBaseEntity*/, IMainIdentity<AirplaneId>, IAggregateRoot
{
    public AcmePeriod Active { get; private set; }
    public AirplaneConfiguration Configuration { get; private set; }
    public int MaximumRangeInKilometers { get; private set; }
    public int MaximumSeats { get; private set; }
    public AirplaneRegistration Registration { get; private set; }
    [JsonConverter(typeof(StringEnumConverter))] // ToDo: Delete NuGet Package once resolved
    public AirplaneStatus Status { get; set; }
    public AirplaneType Type { get; private set; }
    public AirplaneId Id => new(id);

    private class A320Airplane : Airplane
    {
        public A320Airplane(ITimeService timeService) : base(new GuidIdentityFactory())
        {
            Active = AcmePeriod.CreateFromNow(timeService);
            MaximumRangeInKilometers = 6_100;
            MaximumSeats = 186;
            Type = AirplaneType.Create(AirplaneManufacturer.Airbus, AirplaneTypeAbbreviation.A320);
        }
    }

    private class A320neoAirplane : Airplane
    {
        public A320neoAirplane(ITimeService timeService) : base(new GuidIdentityFactory())
        {
            Active = AcmePeriod.CreateFromNow(timeService);
            MaximumRangeInKilometers = 6_500;
            MaximumSeats = 195;
            Type = AirplaneType.Create(AirplaneManufacturer.Airbus, AirplaneTypeAbbreviation.A320neo);
        }
    }

    private class A380Airplane : Airplane
    {
        public A380Airplane(ITimeService timeService) : base(new GuidIdentityFactory())
        {
            Active = AcmePeriod.CreateFromNow(timeService);
            MaximumRangeInKilometers = 14_800;
            MaximumSeats = 853;
            Type = AirplaneType.Create(AirplaneManufacturer.Airbus, AirplaneTypeAbbreviation.A380);
        }
    }

    private class Boeing7478Airplane : Airplane
    {
        public Boeing7478Airplane(ITimeService timeService) : base(new GuidIdentityFactory())
        {
            Active = AcmePeriod.CreateFromNow(timeService);
            MaximumRangeInKilometers = 14_320;
            MaximumSeats = 467;
            Type = AirplaneType.Create(AirplaneManufacturer.Boeing, AirplaneTypeAbbreviation.Boeing7478);
        }
    }

    private Airplane() { }

    private Airplane(IIdentityFactory<Guid> identityFactory)
        : base(identityFactory, new AirplanePartitionKeyFactory()) =>
        Status = AirplaneStatus.Active;

    public static Airplane AddAirplaneToTheFleet(
        AirplaneTypeAbbreviation type,
        PlaneConfigurationType configuration,
        Country country,
        string airplaneRegistration,
        ITimeService timeService)
    {
        var newAirplaneInTheFleet = GetAirplaneType();
        newAirplaneInTheFleet.SetRegistration(country, airplaneRegistration);
        newAirplaneInTheFleet.SetConfiguration(configuration);

        return newAirplaneInTheFleet;

        Airplane GetAirplaneType() =>
            type switch
            {
                AirplaneTypeAbbreviation.A320 => new A320Airplane(timeService),
                AirplaneTypeAbbreviation.A320neo => new A320neoAirplane(timeService),
                AirplaneTypeAbbreviation.A380 => new A380Airplane(timeService),
                AirplaneTypeAbbreviation.Boeing7478 => new Boeing7478Airplane(timeService),
                _ => throw new NotImplementedException(),
            };
    }
    private void SetRegistration(Country country, string airplaneRegistration) =>
        Registration = AirplaneRegistration.Create(country, airplaneRegistration);

    public void SetConfiguration(PlaneConfigurationType configuration) =>
        Configuration = AirplaneConfiguration.Create(configuration, MaximumRangeInKilometers, MaximumSeats);

    public void Sold() =>
        Status = AirplaneStatus.Inactive;

    public static ReadOnlyCollection<string> GetAirplaneTypesInTheFleet() =>
        EnumerationService.GetEnumValues<AirplaneTypeAbbreviation>()
            .Select(ata => ata.AirplaneTypeAbbreviationText())
            .ToList()
            .AsReadOnly();
}

public sealed class AirplaneId : IdValueObject
{
    private AirplaneId() { }

    internal AirplaneId(Guid id) : base(id) { }
}