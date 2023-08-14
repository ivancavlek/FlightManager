using Acme.FlightManager.Common;
using Acme.FlightManager.FlightDirector.DataTransferObject;
using Acme.FlightManager.FlightDirector.UseCase.Command;
using Acme.FlightManager.WebApi.AspNet.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Riok.Mapperly.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.WebApi.AspNet.Apis;

public static class FlightApis
{
    static readonly RequestMapper mapper = new();

    public static RouteGroupBuilder MapFlightApis(this RouteGroupBuilder group)
    {
        //group.MapGet("{start:int}/{end:int}", GetWeatherForecast)
        //    .AddEndpointFilter<ValidatorFilter<Test>>()
        //    .WithName(nameof(GetWeatherForecast));

        group.MapPost("buyticket", BuyTicket)
            .AddEndpointFilter<ValidatorFilter<TicketRequest>>()
            .WithName(nameof(BuyTicket));

        return group;
    }

    //private static Ok<WeatherForecast[]> GetWeatherForecast(
    //    [AsParameters] Test test, CancellationToken cancellationToken)
    //{
    //    var summaries = new[]
    //    {
    //        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //    };

    //    var forecast = Enumerable.Range(test.Start, test.End).Select(index =>
    //        new WeatherForecast
    //        (
    //            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //            Random.Shared.Next(-20, 55),
    //            summaries[Random.Shared.Next(summaries.Length)]
    //        ))
    //        .ToArray();

    //    return TypedResults.Ok(forecast);
    //}

    private static async Task<Created<TicketDto>> BuyTicket(
        [FromBody] TicketRequest request,
        ICommandDispatcher messageDispatcher,
        CancellationToken cancellationToken)
    {
        var buyTicketCommand = mapper.TicketRequestToBuyTicketCommand(request);
        var ticket = await messageDispatcher.DispatchCommand<BuyTicketCommand, TicketDto>(buyTicketCommand, cancellationToken);
        return TypedResults.Created($"ticket/{ticket.Id.Value}", ticket);
    }
}

public record TicketRequest(FlightIdDto FlightId, DateOnly DateOfBirth, Gender Gender, string FirstName, string LastName, int Seat);

public class TicketRequestValidator : AbstractValidator<TicketRequest>
{
    public TicketRequestValidator()
    {
        RuleFor(tst => tst.FlightId)
            .NotNull();
        RuleFor(tst => tst.FlightId.Value)
            .NotEmpty();
        RuleFor(tst => tst.DateOfBirth)
            .GreaterThanOrEqualTo(new DateOnly(1900, 1, 1));
        RuleFor(tst => tst.FirstName)
            .NotEmpty();
        RuleFor(tst => tst.LastName)
            .NotEmpty();
        RuleFor(tst => tst.Seat)
            .GreaterThan(default(int));
    }
}

[Mapper]
public partial class RequestMapper
{
    public partial BuyTicketCommand TicketRequestToBuyTicketCommand(TicketRequest ticketRequest);
}