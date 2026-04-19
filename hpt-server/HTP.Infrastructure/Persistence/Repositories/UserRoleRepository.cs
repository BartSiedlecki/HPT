using HTP.App.Abstractions.Repositories.Read;
using Microsoft.EntityFrameworkCore;

namespace HTP.Infrastructure.Persistence.Repositories;

public class UserRoleRepository(WriteDbContext writeDbContext) : IUserRoleRepository
{
    public async Task<RoleDto?> GetRoleAsync(Guid roleId, CancellationToken ct = default)
    {
        return await writeDbContext.Roles
            .Where(r => r.Id == roleId)
            .Select(r => new RoleDto(r.Id, r.Name.Value, null))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<RoleDto?> GetRoleByNameAsync(string roleName, CancellationToken ct = default)
    {
        return await writeDbContext.Roles
            .Where(r => r.Name.Value == roleName)
            .Select(r => new RoleDto(r.Id, r.Name.Value, null))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<HashSet<string>> GetRolesPermissionsAsync(IEnumerable<string> roleNames, CancellationToken ct = default)
    {
        var permissions = await writeDbContext.Roles
            .Where(r => roleNames.Contains(r.Name.Value))
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.PermissionId.Value)
            .Distinct()
            .ToListAsync(ct);

        return permissions.ToHashSet();
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken ct = default)
    {
        return await writeDbContext.UsersRoles
            .Where(x => x.UserId == userId)
            .Join(writeDbContext.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name.Value)
            .ToListAsync(ct);
    }
}
