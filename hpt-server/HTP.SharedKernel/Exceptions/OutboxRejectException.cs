namespace HPT.SharedKernel.Exceptions;

public sealed class OutboxRejectException : Exception
{
    public OutboxRejectException(string message)
        : base(message) { }

    public OutboxRejectException(Error error)
        : base($"Outbox rejected with code:{error.Code}, message:{error.Message}") { }

    public OutboxRejectException(string message, Exception innerException)
        : base(message, innerException) { }
}
