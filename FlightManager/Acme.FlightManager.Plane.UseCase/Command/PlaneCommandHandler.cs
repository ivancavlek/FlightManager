using Acme.Base.Domain.CosmosDb.Factory;
using Acme.Base.Domain.CosmosDb.Repository;
using Acme.Base.Domain.Messaging;
using Acme.FlightManager.Plane.Domain.Factory;
using System;

namespace Acme.FlightManager.Plane.UseCase.Command;

public abstract class PlaneCommandHandler
{
    protected readonly IMessagePublisher _messagePublisher;
    protected readonly IPartitionKeyFactory _partitionKeyFactory;
    protected readonly ICosmosDbRepository _repository;
    protected readonly ICosmosDbUpsertUnitOfWork _unitOfWork;

    protected PlaneCommandHandler(
        IMessagePublisher messagePublisher, ICosmosDbRepository repository, ICosmosDbUpsertUnitOfWork unitOfWork)
    {
        _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
        _partitionKeyFactory = new AirplanePartitionKeyFactory();
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
}