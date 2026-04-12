using HPT.SharedKernel;

namespace HTP.App.Core.Abstractions.Mediator;

public interface IDispatcher
{
    Task<Result> Send(ICommand command, CancellationToken ct = default);
    Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);
    Task<Result<TResponse>> Query<TResponse>(IQuery<TResponse> query, CancellationToken ct = default);
}

