namespace HPT.SharedKernel.Abstractions;

public interface IDateTimeProvider
{
    DateTimeOffset DateTimeOffsetUtcNow { get; }
    DateTime DateTimeUtcNow { get; }
}
