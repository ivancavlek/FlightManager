using FluentValidation;

namespace Acme.FlightManager.Common.Domain.Entity;

public interface IRoute
{
    public string Destination { get; }
    public string PointOfDeparture { get; }
}

public class RouteValidator : AbstractValidator<IRoute>
{
    public RouteValidator()
    {
        RuleFor(pin => pin.Destination).NotEmpty().Length(3).Matches("[A-Z]");
        RuleFor(pin => pin.PointOfDeparture).NotEmpty().Length(3).Matches("[A-Z]");
    }
}

public static class RouteExtensions
{
    public static TRoute Validate<TRoute>(this IRoute route)
        where TRoute : class, IRoute
    {
        new RouteValidator().ValidateAndThrow(route);

        return route as TRoute;
    }
}