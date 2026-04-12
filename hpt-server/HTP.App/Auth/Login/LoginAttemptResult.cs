using HPT.SharedKernel;

namespace HTP.App.Auth.Login;

public sealed class LoginAttemptResult
{
    public bool IsSuccess { get; }
    public Guid? UserId { get; }
    public bool IsLockedOut { get; }
    public bool IsBlocked { get; }
    public int? RetryAfterMinutes { get; }
    public int? RemainingAttempts { get; }
    public Error? Error { get; }

    private LoginAttemptResult(
        bool isSuccess,
        Guid? userId,
        bool isLockedOut,
        bool isBlocked,
        int? retryAfterMinutes,
        int? remainingAttempts)
    {
        IsSuccess = isSuccess;
        IsLockedOut = isLockedOut;
        RetryAfterMinutes = retryAfterMinutes;
        RemainingAttempts = remainingAttempts;
    }

    public static LoginAttemptResult Success(Guid userId)
        => new(true, userId, false, false, null, null);

    public static LoginAttemptResult LockedOut(int retryAfterMinutes)
        => new(false, null, true, false, retryAfterMinutes, null);

    public static LoginAttemptResult Invalid(int? remainingAttempts)
        => new(false, null, false, false, null, remainingAttempts);
}

