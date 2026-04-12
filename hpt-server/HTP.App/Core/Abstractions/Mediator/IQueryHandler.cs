using HPT.SharedKernel;

namespace HTP.App.Core.Abstractions.Mediator;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken ct);
}
