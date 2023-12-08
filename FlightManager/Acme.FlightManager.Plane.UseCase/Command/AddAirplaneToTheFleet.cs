using Acme.SharedKernel.Domain;
using Acme.SharedKernel.Domain.Command;
using Acme.SharedKernel.Domain.CosmosDb.Repository;
using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.Messaging;
using Acme.SharedKernel.Domain.Service;
using Acme.FlightManager.Common;
using Acme.FlightManager.Plane.DataTransferObject;
using Acme.FlightManager.Plane.Domain.Entity;
using Acme.FlightManager.Plane.Domain.ValueObject;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.Plane.UseCase.Command;

public sealed record AddAirplaneToTheFleetCommand(
    AirplaneTypeAbbreviation Type, PlaneConfigurationType Configuration, Country Country, string AirplaneRegistration)
    : ICommand<AirplaneDto>
{
    internal sealed class AddAirplaneToTheFleetCommandHandler :
        PlaneCommandHandler, ICommandHandler<AddAirplaneToTheFleetCommand, AirplaneDto>
    {
        private readonly ITimeService _timeService;

        public AddAirplaneToTheFleetCommandHandler(
            ITimeService timeService,
            IMessagePublisher messagePublisher,
            ICosmosDbRepository repository,
            ICosmosDbUpsertUnitOfWork unitOfWork)
            : base(messagePublisher, repository, unitOfWork) =>
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));

        async Task<AirplaneDto> ICommandHandler<AddAirplaneToTheFleetCommand, AirplaneDto>.HandleAsync(
            AddAirplaneToTheFleetCommand command, CancellationToken cancellationToken)
        {
            var newAirplaneInTheFleet = Airplane.AddAirplaneToTheFleet(
                command.Type, command.Configuration, command.Country, command.AirplaneRegistration, _timeService);

            await _unitOfWork.Upsert(newAirplaneInTheFleet).CommitAsync().ConfigureAwait(false);

            _messagePublisher.PublishMessage(new AddedAirplaneToTheFleetEvent(
                newAirplaneInTheFleet.Id,
                newAirplaneInTheFleet.Configuration,
                newAirplaneInTheFleet.Registration,
                newAirplaneInTheFleet.Type));

            // send Email - Saga?

            return newAirplaneInTheFleet.ConvertTo<AirplaneDto>();
        }

        private record AddedAirplaneToTheFleetEvent(
            AirplaneId AirplaneId,
            AirplaneConfiguration AirplaneConfiguration,
            AirplaneRegistration AirplaneRegistration,
            AirplaneType AirplaneType) : IIntegrationEvent;
    }
}