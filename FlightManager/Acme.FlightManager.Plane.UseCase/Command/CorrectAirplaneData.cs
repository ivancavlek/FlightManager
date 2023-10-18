using Acme.Base.Domain.Command;
using Acme.Base.Domain.CosmosDb.Repository;
using Acme.FlightManager.Common;
using Acme.FlightManager.Common.Domain;
using Acme.FlightManager.Plane.DataTransferObject;
using Acme.FlightManager.Plane.Domain.Entity;
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
        public CorrectAirplaneDataCommandHandler(ICosmosDbRepository repository, ICosmosDbUpsertUnitOfWork unitOfWork)
            : base(repository, unitOfWork) { }

        async Task<AirplaneDto> ICommandHandler<CorrectAirplaneDataCommand, AirplaneDto>.HandleAsync(
            CorrectAirplaneDataCommand command, CancellationToken cancellationToken)
        {
            var airplane = await _repository
                .GetSingleAsync<Airplane>(command.AirplaneId, _partitionKeyFactory)
                .ConfigureAwait(false);

            airplane.SetConfiguration(command.Configuration);

            await _unitOfWork.Upsert(airplane).CommitAsync().ConfigureAwait(false);

            // send an integration event to the destination director that a new airplane is available

            return airplane.ConvertTo<AirplaneDto>();
        }
    }
}