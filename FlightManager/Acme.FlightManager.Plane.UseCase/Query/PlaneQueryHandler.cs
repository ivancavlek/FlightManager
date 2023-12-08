using Acme.SharedKernel.Domain.CosmosDb.Factory;
using Acme.SharedKernel.Domain.CosmosDb.Repository;
using Acme.FlightManager.Plane.Domain.Factory;
using System;

namespace Acme.FlightManager.Plane.UseCase.Query;

public abstract class PlaneQueryHandler
{
    protected readonly IPartitionKeyFactory _partitionKeyFactory;
    protected readonly ICosmosDbRepository _repository;

    public PlaneQueryHandler(ICosmosDbRepository repository)
    {
        _partitionKeyFactory = new AirplanePartitionKeyFactory();
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}