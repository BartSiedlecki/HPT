
using HPT.SharedKernel;
using HTP.App.Core.Abstractions.Mediator;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace HTP.App.Core.Behaviors;

public sealed class CommandHandlerLoggingDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ICommandHandler<TCommand, TResponse> _inner;
    private readonly ILogger<CommandHandlerLoggingDecorator<TCommand, TResponse>> _logger;

    public CommandHandlerLoggingDecorator(
        ICommandHandler<TCommand, TResponse> inner,
        ILogger<CommandHandlerLoggingDecorator<TCommand, TResponse>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken ct)
    {
        string commandName = typeof(TCommand).Name;
        _logger.LogInformation("Processing command {Command}", commandName);

        var result = await _inner.Handle(command, ct);

        if (result.IsSuccess)
        {
            _logger.LogInformation("Completed command {Command}", commandName);
        }
        else
        {
            using (LogContext.PushProperty("Error", result.Error, true))
            {
                _logger.LogError("Completed command {Command} with error: {@Error}", commandName, result.Error);
            }
        }

        return result;
    }
}

public sealed class CommandHandlerLoggingDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _inner;
    private readonly ILogger<CommandHandlerLoggingDecorator<TCommand>> _logger;

    public CommandHandlerLoggingDecorator(
        ICommandHandler<TCommand> inner,
        ILogger<CommandHandlerLoggingDecorator<TCommand>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Result> Handle(TCommand command, CancellationToken ct)
    {
        string commandName = typeof(TCommand).Name;
        _logger.LogInformation("Processing command {Command}", commandName);

        var result = await _inner.Handle(command, ct);

        if (result.IsSuccess)
        {
            _logger.LogInformation("Completed command {Command}", commandName);
        }
        else
        {
            using (LogContext.PushProperty("Error", result.Error, true))
            {
                _logger.LogError("Completed command {Command} with error: {@Error}", commandName, result.Error);
            }
        }

        return result;
    }
}

public sealed class QueryHandlerLoggingDecorator<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    private readonly IQueryHandler<TQuery, TResponse> _inner;
    private readonly ILogger<QueryHandlerLoggingDecorator<TQuery, TResponse>> _logger;

    public QueryHandlerLoggingDecorator(
        IQueryHandler<TQuery, TResponse> inner,
        ILogger<QueryHandlerLoggingDecorator<TQuery, TResponse>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken ct)
    {
        string queryName = typeof(TQuery).Name;
        _logger.LogInformation("Processing query {Query}", queryName);

        var result = await _inner.Handle(query, ct);

        if (result.IsSuccess)
        {
            _logger.LogInformation("Completed query {Query}", queryName);
        }
        else
        {
            using (LogContext.PushProperty("Error", result.Error, true))
            {
                _logger.LogError("Completed command {Command} with error: {@Error}", queryName, result.Error);
            }
        }

        return result;
    }
}
