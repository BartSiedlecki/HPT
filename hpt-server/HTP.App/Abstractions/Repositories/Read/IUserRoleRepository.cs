namespace HTP.App.Abstractions.Repositories.Read;

public interface IUserRoleRepository
{
    Task<RoleDto?> GetRoleAsync(Guid roleId, CancellationToken ct = default);
    Task<RoleDto?> GetRoleByNameAsync(string roleName, CancellationToken ct = default);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken ct = default);
    Task<HashSet<string>> GetRolesPermissionsAsync(IEnumerable<string> roleNames, CancellationToken ct = default);
}

public record RoleDto(Guid Id, string Name, string? Description);
