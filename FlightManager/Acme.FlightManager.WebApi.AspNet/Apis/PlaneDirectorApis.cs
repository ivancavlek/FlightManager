using Acme.FlightManager.Common;
using Acme.FlightManager.Plane.DataTransferObject;
using Acme.FlightManager.Plane.UseCase.Command;
using Acme.FlightManager.Plane.UseCase.Query;
using Acme.FlightManager.WebApi.AspNet.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.WebApi.AspNet.Apis;

public static class AirplaneApis
{
    const string airplaneRoute = "airplane";
    static readonly AirplaneRequestMapper mapper = new();

    public static RouteGroupBuilder MapFleetApis(this RouteGroupBuilder group)
    {
        group.MapDelete(airplaneRoute, SoldAirplane)
            .AddEndpointFilter<ValidatorFilter<SoldAirplaneRequest>>()
            .WithName(nameof(SoldAirplane));

        group.MapGet(airplaneRoute, GetAirplane)
            .WithName(nameof(GetAirplane));

        group.MapGet(string.Empty, GetFleet)
            .WithName(nameof(GetFleet));

        group.MapPost(airplaneRoute, AddAirplaneToTheFleet)
            .AddEndpointFilter<ValidatorFilter<AddAirplaneToTheFleetRequest>>()
            .WithName(nameof(AddAirplaneToTheFleet));

        group.MapPut(airplaneRoute, CorrectAirplaneData)
            .AddEndpointFilter<ValidatorFilter<CorrectAirplaneDataRequest>>()
            .WithName(nameof(CorrectAirplaneData));

        return group;
    }

    private static async Task<Created<AirplaneDto>> AddAirplaneToTheFleet(
        [FromBody] AddAirplaneToTheFleetRequest request,
        ICommandDispatcher messageDispatcher,
        CancellationToken cancellationToken)
    {
        // ToDo: it would be useful to send a request to a 3rd party for a certificate of registration and register if successful
        var addAirplaneToTheFleetCommand = mapper.AddAirplaneToTheFleetRequestToAddAirplaneToTheFleetCommand(request);
        var airplane = await messageDispatcher.DispatchCommand<AddAirplaneToTheFleetCommand, AirplaneDto>(
            addAirplaneToTheFleetCommand, cancellationToken);
        return TypedResults.Created(
            $"{airplaneRoute}?airplaneid={airplane.Id.Value}&airplaneregistration={airplane.Registration.Registration}",
            airplane);
    }

    private static async Task<Created<AirplaneDto>> CorrectAirplaneData(
        [FromBody] CorrectAirplaneDataRequest request,
        ICommandDispatcher messageDispatcher,
        CancellationToken cancellationToken)
    {
        var newAirplaneConfigurationCommand = mapper
            .CorrectAirplaneDataRequestToCorrectAirplaneDataCommand(request);
        var airplane = await messageDispatcher.DispatchCommand<CorrectAirplaneDataCommand, AirplaneDto>(
            newAirplaneConfigurationCommand, cancellationToken);
        return TypedResults.Created(
            $"{airplaneRoute}?airplaneid={airplane.Id.Value}&airplaneregistration={airplane.Registration.Registration}",
            airplane);
    }

    private static async Task<Ok<AirplaneDto>> GetAirplane(
        [AsParameters] GetAirplaneRequest request,
        IQueryDispatcher queryDispatcher,
        CancellationToken cancellationToken)
    {
        var airplaneQuery = mapper.GetAirplaneRequestToGetAirplaneQuery(request);
        var airplane = await queryDispatcher.DispatchQuery<GetAirplaneQuery, AirplaneDto>(
            airplaneQuery, cancellationToken);
        return TypedResults.Ok(airplane);
    }

    private static async Task<Ok<ReadOnlyCollection<AirplaneDto>>> GetFleet(
        IQueryDispatcher queryDispatcher,
        CancellationToken cancellationToken)
    {
        var fleetQuery = new GetFleetQuery();
        var fleet = await queryDispatcher.DispatchQuery<GetFleetQuery, ReadOnlyCollection<AirplaneDto>>(
            fleetQuery, cancellationToken);
        return TypedResults.Ok(fleet);
    }

    private static async Task<NoContent> SoldAirplane(
        [AsParameters] SoldAirplaneRequest request,
        ICommandDispatcher messageDispatcher,
        CancellationToken cancellationToken)
    {
        var soldAirplaneCommand = mapper.SoldAirplaneRequestToSoldAirplaneCommand(request);
        await messageDispatcher.DispatchCommand<SoldAirplaneCommand, AirplaneDto>(
            soldAirplaneCommand, cancellationToken);
        return TypedResults.NoContent();
    }
}

public record AddAirplaneToTheFleetRequest(
    AirplaneTypeAbbreviation Type, PlaneConfigurationType Configuration, Country Country, string AirplaneRegistration);

public class AddAirplaneToTheFleetRequestValidator : AbstractValidator<AddAirplaneToTheFleetRequest>
{
    public AddAirplaneToTheFleetRequestValidator()
    {
        RuleFor(tst => tst.AirplaneRegistration)
            .NotNull()
            .NotEmpty();
    }
}

public record CorrectAirplaneDataRequest(
    Guid AirplaneId, PlaneConfigurationType Configuration, string AirplaneRegistration);

public class CorrectAirplaneDataRequestValidator : AbstractValidator<CorrectAirplaneDataRequest>
{
    public CorrectAirplaneDataRequestValidator()
    {
        RuleFor(tst => tst.AirplaneId)
            .NotEmpty();
        RuleFor(tst => tst.AirplaneRegistration)
            .NotNull()
            .NotEmpty();
    }
}

public record SoldAirplaneRequest(Guid AirplaneId, string AirplaneRegistration);

public class SoldAirplaneRequestValidator : AbstractValidator<SoldAirplaneRequest>
{
    public SoldAirplaneRequestValidator()
    {
        RuleFor(tst => tst.AirplaneId)
            .NotEmpty();
        RuleFor(tst => tst.AirplaneRegistration)
            .NotNull()
            .NotEmpty();
    }
}

public record GetAirplaneRequest(Guid AirplaneId, string AirplaneRegistration);

[Mapper]
public partial class AirplaneRequestMapper
{
    public partial AddAirplaneToTheFleetCommand AddAirplaneToTheFleetRequestToAddAirplaneToTheFleetCommand(
        AddAirplaneToTheFleetRequest request);

    public partial GetAirplaneQuery GetAirplaneRequestToGetAirplaneQuery(GetAirplaneRequest request);

    public partial CorrectAirplaneDataCommand CorrectAirplaneDataRequestToCorrectAirplaneDataCommand(
        CorrectAirplaneDataRequest request);

    public partial SoldAirplaneCommand SoldAirplaneRequestToSoldAirplaneCommand(
        SoldAirplaneRequest request);
}