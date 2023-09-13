using Acme.FlightManager.Common;
using Acme.FlightManager.Plane.Domain.Entity;
using Acme.FlightManager.Plane.UseCase.Command;
using Acme.FlightManager.WebApi.AspNet.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Riok.Mapperly.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.WebApi.AspNet.Apis;

public static class PlaneApis
{
    static readonly PlaneRequestMapper mapper = new();

    public static RouteGroupBuilder MapPlaneApis(this RouteGroupBuilder group)
    {
        group.MapPost("addtothefleet", AddAirplaneToTheFleet)
            .AddEndpointFilter<ValidatorFilter<AddAirplaneToTheFleetRequest>>()
            .WithName(nameof(AddAirplaneToTheFleet));

        return group;
    }

    private static async Task<Created<AirplaneId>> AddAirplaneToTheFleet(
        [FromBody] AddAirplaneToTheFleetRequest request,
        ICommandDispatcher messageDispatcher,
        CancellationToken cancellationToken)
    {
        var addAirplaneToTheFleetCommand = mapper.AddAirplaneToTheFleetRequestToAddAirplaneToTheFleetCommand(request);
        var plane = await messageDispatcher.DispatchCommand<AddAirplaneToTheFleetCommand, AirplaneId>(
            addAirplaneToTheFleetCommand, cancellationToken);
        return TypedResults.Created($"plane/{plane.Value}", plane);
    }
}

public record AddAirplaneToTheFleetRequest(
    AirplaneTypeAbbreviation Type, PlaneConfigurationType Configuration, Country Country, string AircraftRegistration);

public class AddAirplaneToTheFleetRequestValidator : AbstractValidator<AddAirplaneToTheFleetRequest>
{
    public AddAirplaneToTheFleetRequestValidator()
    {
        RuleFor(tst => tst.AircraftRegistration)
            .NotNull()
            .NotEmpty();
    }
}

[Mapper]
public partial class PlaneRequestMapper
{
    public partial AddAirplaneToTheFleetCommand AddAirplaneToTheFleetRequestToAddAirplaneToTheFleetCommand(
        AddAirplaneToTheFleetRequest ticketRequest);
}