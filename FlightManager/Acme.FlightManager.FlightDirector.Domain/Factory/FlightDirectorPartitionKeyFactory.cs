using Acme.Base.Domain.CosmosDb.Factory;
using Acme.FlightManager.Common.Domain.Entity;
using FluentValidation;

namespace Acme.FlightManager.FlightDirector.Domain.Factory;

internal class FlightDirectorPartitionKeyFactory : IPartitionKeyFactory
{
    private readonly IRoute _route;

    public FlightDirectorPartitionKeyFactory(IRoute route) =>
        _route = route.Validate();

    string IPartitionKeyFactory.CreatePartitionKey() =>
        $"{_route.Destination}-{_route.PointOfDeparture}";
}