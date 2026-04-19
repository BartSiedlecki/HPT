using HPT.SharedKernel;

namespace HTP.App.Users.Errors;

/// <summary>
/// Application errors related to user operations.
/// </summary>
public static class UserApplicationErrors
{
    public static Error EmailAlreadyExists =>
        Error.Conflict("User.EmailAlreadyExists", "A user with this email already exists.");

    public static Error UserCreationFailed =>
        Error.Internal("User.CreationFailed");

    public static Error UserNotFound =>
        Error.NotFound("User.NotFound", "User not found.");

    public static Error InvalidEmail =>
        Error.Validation("User.InvalidEmail", "Invalid email address.");

    public static Error RoleNotFound(string roleName) =>
        Error.NotFound("User.RoleNotFound", $"Role '{roleName}' was not found.");
}
