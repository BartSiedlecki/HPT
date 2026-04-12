
using HTP.Domain.Entities.Permissions;
using HTP.Domain.Entities.Roles;
using HTP.Domain.ValueObjects;

namespace HTP.Domain.Entities;

public class RolePermission
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;
    public PermissionId PermissionId { get; set; } = default!;
    public Permission Permission { get; set; } = default!;
}
