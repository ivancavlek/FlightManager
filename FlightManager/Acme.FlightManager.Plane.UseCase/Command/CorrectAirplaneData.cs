using Acme.Base.Domain;
using Acme.Base.Domain.Command;
using Acme.Base.Domain.CosmosDb.Repository;
using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Messaging;
using Acme.FlightManager.Common;
using Acme.FlightManager.Plane.DataTransferObject;
using Acme.FlightManager.Plane.Domain.Entity;
using Acme.FlightManager.Plane.Domain.ValueObject;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.Plane.UseCase.Command;

public sealed record CorrectAirplaneDataCommand(Guid AirplaneId, PlaneConfigurationType Configuration)
    : ICommand<AirplaneDto>
{
    internal sealed class CorrectAirplaneDataCommandHandler :
        PlaneCommandHandler, ICommandHandler<CorrectAirplaneDataCommand, AirplaneDto>
    {
        public CorrectAirplaneDataCommandHandler(
            IMessagePublisher messagePublisher, ICosmosDbRepository repository, ICosmosDbUpsertUnitOfWork unitOfWork)
            : base(messagePublisher, repository, unitOfWork) { }

        async Task<AirplaneDto> ICommandHandler<CorrectAirplaneDataCommand, AirplaneDto>.HandleAsync(
            CorrectAirplaneDataCommand command, CancellationToken cancellationToken)
        {
            var airplane = await _repository
                .GetSingleAsync<Airplane>(command.AirplaneId, _partitionKeyFactory)
                .ConfigureAwait(false);

            airplane.SetConfiguration(command.Configuration);

            await _unitOfWork.Upsert(airplane).CommitAsync().ConfigureAwait(false);

            _messagePublisher.PublishMessage(
                new CorrectedAirplaneDataEvent(airplane.Id, airplane.Configuration));

            return airplane.ConvertTo<AirplaneDto>();
        }

        private record CorrectedAirplaneDataEvent(
            AirplaneId AirplaneId, AirplaneConfiguration AirplaneConfiguration) : IIntegrationEvent;
    }
}