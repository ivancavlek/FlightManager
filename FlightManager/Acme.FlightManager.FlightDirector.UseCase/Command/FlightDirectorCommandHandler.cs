using Acme.SharedKernel.Domain.CosmosDb.Repository;

namespace Acme.FlightManager.FlightDirector.UseCase.Command;

public abstract class FlightDirectorCommandHandler
{
    protected ICosmosDbRepository Repository { get; }
    protected ICosmosDbUpsertUnitOfWork UnitOfWork { get; }

    protected FlightDirectorCommandHandler(ICosmosDbRepository repository, ICosmosDbUpsertUnitOfWork unitOfWork)
    {
        Repository = repository;
        UnitOfWork = unitOfWork;
    }
}