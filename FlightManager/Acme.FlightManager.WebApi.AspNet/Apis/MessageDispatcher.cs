using Acme.Base.Domain.Command;
using Acme.Base.Domain.Query;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.WebApi.AspNet.Apis;

public sealed class MessageDispatcher
{
    private readonly IServiceProvider _provider;

    public MessageDispatcher(IServiceProvider provider) =>
        _provider = provider;

    public async Task<TCommandResult> Dispatch<TCommandResult>(
        ICommand<TCommandResult> command, CancellationToken cancellationToken)
    {
        Type type = typeof(ICommandHandler<,>);
        Type[] typeArgs = { command.GetType(), typeof(TCommandResult) };
        Type handlerType = type.MakeGenericType(typeArgs);

        dynamic handler = _provider.GetService(handlerType);
        TCommandResult result = await handler.HandleAsync((dynamic)command, cancellationToken);

        return result;
    }

    public async Task<TQueryResult> Dispatch<TQueryResult>(
        IQuery<TQueryResult> query, CancellationToken cancellationToken)
    {
        Type type = typeof(IQueryHandler<,>);
        Type[] typeArgs = { query.GetType(), typeof(TQueryResult) };
        Type handlerType = type.MakeGenericType(typeArgs);

        dynamic handler = _provider.GetService(handlerType);
        TQueryResult result = await handler.HandleAsync((dynamic)query, cancellationToken);

        return result;
    }
}