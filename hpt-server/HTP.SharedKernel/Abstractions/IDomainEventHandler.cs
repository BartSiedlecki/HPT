using HPT.SharedKernel.Abstractions;

namespace HTP.SharedKernel.Abstractions;

public interface IDomainEventHandler<in T> where T : IDomainEvent // todo: sprawdzić czy mogę przenieść do aplikacji
{
    Task Handle(T @event, CancellationToken ct);
}
