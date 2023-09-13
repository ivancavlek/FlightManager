using Acme.Base.Domain.Command;
using Acme.Base.Domain.CosmosDb.Repository;
using Acme.Base.Domain.Service;
using Acme.FlightManager.Common;
using Acme.FlightManager.Plane.Domain.Entity;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.Plane.UseCase.Command;

public sealed record AddAirplaneToTheFleetCommand(
    AirplaneTypeAbbreviation Type, PlaneConfigurationType Configuration, Country Country, string AircraftRegistration)
    : ICommand<AirplaneId>
{
    internal sealed class AddAirplaneToTheFleetCommandHandler :
        PlaneCommandHandler, ICommandHandler<AddAirplaneToTheFleetCommand, AirplaneId>
    {
        private readonly ITimeService _timeService;

        public AddAirplaneToTheFleetCommandHandler(
            ITimeService timeService, ICosmosDbRepository repository, ICosmosDbUpsertUnitOfWork unitOfWork)
            : base(repository, unitOfWork) =>
            _timeService = timeService;

        async Task<AirplaneId> ICommandHandler<AddAirplaneToTheFleetCommand, AirplaneId>.HandleAsync(
            AddAirplaneToTheFleetCommand command, CancellationToken cancellationToken)
        {
            var newAirplaneInTheFleet = Airplane.AddAirplaneToTheFleet(
                command.Type, command.Configuration, command.Country, command.AircraftRegistration, _timeService);

            await UnitOfWork.Upsert(newAirplaneInTheFleet).CommitAsync().ConfigureAwait(false);

            // send an integration event to the destination director that a new airplane is available

            return newAirplaneInTheFleet.Id;
        }
    }
}

public class AddAirplaneToTheFleetCommandValidator : AbstractValidator<AddAirplaneToTheFleetCommand>
{
    public AddAirplaneToTheFleetCommandValidator()
    {
        RuleFor(tst => tst.AircraftRegistration)
            .NotNull()
            .NotEmpty();
    }
}