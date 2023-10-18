using Acme.Base.Domain.CosmosDb.Repository;
using Acme.Base.Domain.Query;
using Acme.FlightManager.Common;
using Acme.FlightManager.Common.Domain;
using Acme.FlightManager.Plane.DataTransferObject;
using Acme.FlightManager.Plane.Domain.Entity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.Plane.UseCase.Query;

public sealed record GetFleetQuery()
    : IQuery<ReadOnlyCollection<AirplaneDto>>
{
    internal sealed class GetFleetQueryHandler :
        PlaneQueryHandler, IQueryHandler<GetFleetQuery, ReadOnlyCollection<AirplaneDto>>
    {
        public GetFleetQueryHandler(ICosmosDbRepository repository) : base(repository) { }

        async Task<ReadOnlyCollection<AirplaneDto>> IQueryHandler<GetFleetQuery, ReadOnlyCollection<AirplaneDto>>.HandleAsync(
            GetFleetQuery query, CancellationToken cancellationToken) =>
            (await _repository.GetAllAsync(GetQuery(), _partitionKeyFactory).ConfigureAwait(false))
                .Select(ape => ape.ConvertTo<AirplaneDto>())
                .ToList()
                .AsReadOnly();
    }

    private static Expression<Func<Airplane, bool>> GetQuery() =>
        PredicateBuilder.True<Airplane>()
            .And(ape => ape.Status == AirplaneStatus.Active || ape.Status == AirplaneStatus.Maintenance);
}