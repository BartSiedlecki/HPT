using HPT.SharedKernel;
using HPT.SharedKernel.Constants;
using HTP.App.Auth.Login;

namespace HTP.App.Errors;

public class AuthApplicationErrors
{
    public static Error UserNotFound =>
        Error.Unauthorized(ErrorCodes.Auth.UserNotFound);

    public static Error Unauthenticated =>
        Error.Unauthorized(ErrorCodes.Auth.Unauthenticated);

    public static Error Forbidden =>
        Error.Forbidden(ErrorCodes.Auth.Forbidden);

    public static Error UserBlocked =>
        Error.Forbidden(ErrorCodes.Auth.UserBlocked);
    public static Error UserUnconfired =>
        Error.Forbidden(ErrorCodes.Auth.UserUnconfirmed);

    public static Error UserRegistrationFailed =>
        Error.Forbidden(ErrorCodes.Auth.UserRegistrationFailed);

    public static Error PasswordLockedOut(int tryInMinutes) =>
    Error.Unauthorized(
        ErrorCodes.Auth.PasswordLockedOut,
        args: new Dictionary<string, object?>
        {
            ["Minutes"] = tryInMinutes
        });

    public static Error InvalidCredentials =>
    Error.Unauthorized(ErrorCodes.Auth.InvalidCredentials,
       fieldErrors: new Dictionary<string, string[]>
       {
            { nameof(LoginUserCommand.Login), [] },
            { nameof(LoginUserCommand.Password), [] }
       });

    public static Error InvalidCredentialsWithAttempts(int attempts) =>
    Error.Unauthorized(
        ErrorCodes.Auth.InvalidCredentialsWithAttempts(attempts),
        fieldErrors: new Dictionary<string, string[]>
        {
            { nameof(LoginUserCommand.Login), [] },
            { nameof(LoginUserCommand.Password), [] }
        },
        args: new Dictionary<string, object?>
        {
            ["RemainingAttempts"] = attempts
        });

    public static Error InvalidRefreshToken =>
        Error.Problem(ErrorCodes.Auth.InvalidRefreshToken);

    public static Error InvalidPasswordRecoveryToken =>
        Error.Problem(ErrorCodes.Auth.InvalidPasswordRecoveryToken);

    public static Error PasswordChangeFailed =>
        Error.Problem(ErrorCodes.Auth.PasswordChangeFailed);

    public static Error AccountInactive =>
        Error.Problem(ErrorCodes.Auth.AccountInactive);

    public static Error AuthFailed =>
        Error.Problem(ErrorCodes.Auth.GeneralFailed);
}