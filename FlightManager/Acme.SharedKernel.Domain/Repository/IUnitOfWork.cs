using System.Threading.Tasks;

namespace Acme.SharedKernel.Domain.Repository;

public interface IUnitOfWork
{
    Task CommitAsync();
}