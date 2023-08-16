using Acme.Base.Domain.Command;
using Acme.Base.Domain.Query;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.WebApi.AspNet.Apis;

internal sealed class CommandValidationDispatcher : ICommandDispatcher
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IValidator _validator;

    public CommandValidationDispatcher(ICommandDispatcher commandDispatcher, IValidator validator)
    {
        _commandDispatcher = commandDispatcher;
        _validator = validator;
    }

    async Task<TCommandResult> ICommandDispatcher.DispatchCommand<TCommand, TCommandResult>(
        TCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(new ValidationContext<TCommand>(command), cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _commandDispatcher.DispatchCommand<TCommand, TCommandResult>(command, cancellationToken);
    }
}

internal sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _provider;

    public CommandDispatcher(IServiceProvider provider) =>
        _provider = provider;

    async Task<TCommandResult> ICommandDispatcher.DispatchCommand<TCommand, TCommandResult>(
        TCommand command, CancellationToken cancellationToken)
    {
        var handler = _provider.GetService<ICommandHandler<TCommand, TCommandResult>>();
        return await handler.HandleAsync(command, cancellationToken);
    }
}

internal sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _provider;

    public QueryDispatcher(IServiceProvider provider) =>
        _provider = provider;

    async Task<TQueryResult> IQueryDispatcher.DispatchQuery<TQuery, TQueryResult>(
        TQuery query, CancellationToken cancellationToken)
    {
        var handler = _provider.GetService<IQueryHandler<TQuery, TQueryResult>>();
        return await handler.HandleAsync(query, cancellationToken);
    }
}

internal interface ICommandDispatcher
{
    Task<TCommandResult> DispatchCommand<TCommand, TCommandResult>(
        TCommand command, CancellationToken cancellationToken) where TCommand : ICommand<TCommandResult>;
}

internal interface IQueryDispatcher
{
    Task<TQueryResult> DispatchQuery<TQuery, TQueryResult>(
        TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TQueryResult>;
}