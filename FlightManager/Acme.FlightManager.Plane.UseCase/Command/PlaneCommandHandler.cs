﻿using Acme.Base.Domain.CosmosDb.Factory;
using Acme.Base.Domain.CosmosDb.Repository;
using Acme.FlightManager.Plane.Domain.Factory;
using System;

namespace Acme.FlightManager.Plane.UseCase.Command;

public abstract class PlaneCommandHandler
{
    protected readonly IPartitionKeyFactory _partitionKeyFactory;
    protected readonly ICosmosDbRepository _repository;
    protected readonly ICosmosDbUpsertUnitOfWork _unitOfWork;

    protected PlaneCommandHandler(ICosmosDbRepository repository, ICosmosDbUpsertUnitOfWork unitOfWork)
    {
        _partitionKeyFactory = new AirplanePartitionKeyFactory();
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
}