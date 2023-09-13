using Acme.Base.Domain.CosmosDb.Repository;

namespace Acme.FlightManager.Plane.UseCase.Command;

public abstract class PlaneCommandHandler
{
    protected ICosmosDbRepository Repository { get; }
    protected ICosmosDbUpsertUnitOfWork UnitOfWork { get; }

    protected PlaneCommandHandler(ICosmosDbRepository repository, ICosmosDbUpsertUnitOfWork unitOfWork)
    {
        Repository = repository;
        UnitOfWork = unitOfWork;
    }
}