using HTP.App.Abstractions.Repositories.Read;
using Microsoft.EntityFrameworkCore;

namespace HTP.Infrastructure.Persistence.Repositories;

public class UserRoleRepository(WriteDbContext writeDbContext) : IUserRoleRepository
{

    public Task<bool> AddUserToRoleAsync(Guid userId, string roleName, CancellationToken ct = default)
    {
        //writeDbContext.role
        //dokonczyc
        throw new NotImplementedException();
    }

    public Task<RoleDto?> GetRoleAsync(Guid roleId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<HashSet<string>> GetRolesPermissionsAsync(IEnumerable<string> roleNames, CancellationToken ct = default)
    {
        throw new NotImplementedException();
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

    public Task<bool> RemoveUserFromRoleAsync(Guid userId, string roleName, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
