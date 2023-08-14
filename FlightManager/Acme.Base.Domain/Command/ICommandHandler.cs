using System.Threading;
using System.Threading.Tasks;

namespace Acme.Base.Domain.Command;

/// <summary>
/// <see href="https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=91">Command credits</see>
/// </summary>
public interface ICommandHandler<in TCommand, TCommandResult> where TCommand : ICommand<TCommandResult>
{
    Task<TCommandResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}