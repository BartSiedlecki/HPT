namespace HPT.SharedKernel.Exceptions;

public sealed class OutboxMissingEventTypeException : Exception
{
    public OutboxMissingEventTypeException(string eventType)
        : base($"Missing event type: {eventType}.") { }
}
