using Acme.Base.Domain.Command;
using Acme.Base.Domain.CosmosDb.Repository;
using Acme.FlightManager.Common;
using Acme.FlightManager.Common.Domain;
using Acme.FlightManager.FlightDirector.DataTransferObject;
using Acme.FlightManager.FlightDirector.Domain.Entity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.FlightDirector.UseCase.Command;

public sealed record BuyTicketCommand(
    FlightIdDto FlightId, DateOnly DateOfBirth, Gender Gender, string FirstName, string LastName, int Seat)
    : ICommand<TicketDto>
{
    internal sealed class BuyTicketCommandHandler :
        FlightDirectorCommandHandler, ICommandHandler<BuyTicketCommand, TicketDto>
    {
        public BuyTicketCommandHandler(ICosmosDbRepository repository, ICosmosDbUpsertUnitOfWork unitOfWork)
            : base(repository, unitOfWork) { }

        async Task<TicketDto> ICommandHandler<BuyTicketCommand, TicketDto>.HandleAsync(
            BuyTicketCommand command, CancellationToken cancellationToken)
        {
            var flight = await Repository.GetSingleAsync<Flight>(command.FlightId.Value); // ToDo: Guid or StronglyTypedId(! due to DTO)

            var ticket = flight.BuyTicketForGuest(
                command.DateOfBirth, command.Gender, command.FirstName, command.LastName, command.Seat);

            await UnitOfWork.Upsert(flight).CommitAsync().ConfigureAwait(false);

            return ticket.ConvertTo<TicketDto>();
        }
    }
}