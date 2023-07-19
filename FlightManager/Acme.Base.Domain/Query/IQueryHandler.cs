using System.Threading;
using System.Threading.Tasks;

namespace Acme.Base.Domain.Query;

/// <summary>
/// <see href="https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=92">Query credits</see>
/// </summary>
public interface IQueryHandler<TQuery, TQueryResult> where TQuery : IQuery<TQueryResult>
{
    Task<TQueryResult> Handle(TQuery query, CancellationToken cancellationToken);
}