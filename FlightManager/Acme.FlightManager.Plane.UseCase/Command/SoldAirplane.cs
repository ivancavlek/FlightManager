using Acme.Base.Domain.Command;
using Acme.Base.Domain.CosmosDb.Repository;
using Acme.FlightManager.Common.Domain;
using Acme.FlightManager.Plane.DataTransferObject;
using Acme.FlightManager.Plane.Domain.Entity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.Plane.UseCase.Command;

public sealed record SoldAirplaneCommand(Guid AirplaneId, string AirplaneRegistration)
    : ICommand<AirplaneDto>
{
    internal sealed class SoldAirplaneCommandHandler :
        PlaneCommandHandler, ICommandHandler<SoldAirplaneCommand, AirplaneDto>
    {
        public SoldAirplaneCommandHandler(ICosmosDbRepository repository, ICosmosDbUpsertUnitOfWork unitOfWork)
            : base(repository, unitOfWork) { }

        async Task<AirplaneDto> ICommandHandler<SoldAirplaneCommand, AirplaneDto>.HandleAsync(
            SoldAirplaneCommand command, CancellationToken cancellationToken)
        {
            var airplane = await _repository
                .GetSingleAsync<Airplane>(command.AirplaneId, _partitionKeyFactory)
                .ConfigureAwait(false);

            airplane.Sold();

            await _unitOfWork.Upsert(airplane).CommitAsync().ConfigureAwait(false);

            // send an integration event to the destination director that an airplane is no longer available

            return airplane.ConvertTo<AirplaneDto>();
        }
    }
}