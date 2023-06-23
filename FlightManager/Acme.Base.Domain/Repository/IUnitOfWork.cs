using System.Threading.Tasks;

namespace Acme.Base.Domain.Repository;

public interface IUnitOfWork
{
    Task CommitAsync();
}