using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading;

namespace Acme.FlightManager.WebApi.AspNet.Api;

public static class FlightDirectorApis
{
    public static RouteGroupBuilder MapFlightDirectorApis(this RouteGroupBuilder group)
    {
        group.MapGet("/{start:int}/{end:int}", GetWeatherForecast)
            // AddFilter<Validation> // https://www.youtube.com/watch?v=Kt9TiXrwIp4
            .WithName("GetWeatherForecast")
            .WithOpenApi()
            .AllowAnonymous();

        return group;
    }

    private static Ok<WeatherForecast[]> GetWeatherForecast(
        [AsParameters] Test test,
        CancellationToken cancellationToken)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(test.Start, test.End).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();

        return TypedResults.Ok(forecast);
    }
}

internal record Test(int Start, int End);

internal record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}