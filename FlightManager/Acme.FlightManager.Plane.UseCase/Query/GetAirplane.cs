using Acme.FlightManager.Plane.DataTransferObject;
using Acme.FlightManager.Plane.Domain.Entity;
using Acme.SharedKernel.Domain;
using Acme.SharedKernel.Domain.CosmosDb.Repository;
using Acme.SharedKernel.Domain.Query;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.Plane.UseCase.Query;

public sealed record GetAirplaneQuery(Guid AirplaneId, string AirplaneRegistration)
    : IQuery<AirplaneDto>
{
    internal sealed class GetAirplaneQueryHandler :
        PlaneQueryHandler, IQueryHandler<GetAirplaneQuery, AirplaneDto>
    {
        public GetAirplaneQueryHandler(ICosmosDbRepository repository) : base(repository) { }

        async Task<AirplaneDto> IQueryHandler<GetAirplaneQuery, AirplaneDto>.HandleAsync(
            GetAirplaneQuery query, CancellationToken cancellationToken) =>
            (await _repository.GetSingleAsync<Airplane>(
                    query.AirplaneId, _partitionKeyFactory, cancellationToken).ConfigureAwait(false))
                .ConvertTo<AirplaneDto>();
    }
}