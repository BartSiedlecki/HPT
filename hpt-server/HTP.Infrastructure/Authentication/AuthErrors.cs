using HPT.SharedKernel;

namespace HTP.Infrastructure.Authentication;

public sealed class AuthErrors
{
    public static Error UnathenticatedUser => Error.Unauthorized(
        "Auth.UnathenticatedUser",
        "The user is not authenticated");   
}
