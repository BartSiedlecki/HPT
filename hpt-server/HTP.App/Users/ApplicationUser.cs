using HTP.Domain.ValueObjects;

namespace HTP.App.Users;

public class ApplicationUser
{
    public Guid UserId { get; init; }
    public Email Email { get; init; }
    public IReadOnlyCollection<string> Roles { get; init; } = [];
    public IReadOnlyCollection<string> Permissions { get; init; } = [];

    public bool HasPermission(string permission)
    {
        return Permissions.Contains(permission);
    }

    public bool HasRole(string role)
    {
        return Roles.Contains(role);
    }
}
