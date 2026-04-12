namespace HPT.SharedKernel.Abstractions;

public interface IApplicationEventHandler<in TEvent> where TEvent : IApplicationEvent
{
    Task Handle(TEvent @event, CancellationToken ct);
}
