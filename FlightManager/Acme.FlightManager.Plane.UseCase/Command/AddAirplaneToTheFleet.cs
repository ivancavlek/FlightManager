using Acme.Base.Domain.Command;
using Acme.Base.Domain.CosmosDb.Repository;
using Acme.Base.Domain.Service;
using Acme.FlightManager.Common;
using Acme.FlightManager.Common.Domain;
using Acme.FlightManager.Plane.DataTransferObject;
using Acme.FlightManager.Plane.Domain.Entity;
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
            ITimeService timeService, ICosmosDbRepository repository, ICosmosDbUpsertUnitOfWork unitOfWork)
            : base(repository, unitOfWork) =>
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));

        async Task<AirplaneDto> ICommandHandler<AddAirplaneToTheFleetCommand, AirplaneDto>.HandleAsync(
            AddAirplaneToTheFleetCommand command, CancellationToken cancellationToken)
        {
            var newAirplaneInTheFleet = Airplane.AddAirplaneToTheFleet(
                command.Type, command.Configuration, command.Country, command.AirplaneRegistration, _timeService);

            await _unitOfWork.Upsert(newAirplaneInTheFleet).CommitAsync().ConfigureAwait(false);

            // send an integration event to the destination director that a new airplane is available

            return newAirplaneInTheFleet.ConvertTo<AirplaneDto>();
        }
    }
}