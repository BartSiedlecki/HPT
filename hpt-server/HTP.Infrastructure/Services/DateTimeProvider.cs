
using HPT.SharedKernel.Abstractions;

namespace HTP.Infrastructure.Services;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset DateTimeOffsetUtcNow => DateTimeOffset.UtcNow;
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
}
