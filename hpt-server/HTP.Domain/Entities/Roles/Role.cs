using HTP.Domain.Entities.Roles.Events;
using HTP.Domain.Entities.Roles.ValueObjects;
using HTP.Domain.ValueObjects;
using HTP.SharedKernel;

namespace HTP.Domain.Entities.Roles;

public class Role : Entity
{
    public Guid Id { get; init; }
    public RoleName Name { get; private set; } = null!;
    private Role()  { } // EF Core

    private readonly List<RolePermission> _rolePermissions = new();
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions;
    public DateTimeOffset CreatedAt { get; init; }
    public Guid CreatedByUserId { get; init; }
    public DateTimeOffset? LastModifiedAt { get; private set; }
    public Guid? LastModifiedByUserId { get; set; }

    public static Role Create(RoleName name, DateTimeOffset createdAt, Guid createdByUserId)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = name,
            CreatedAt = createdAt,
            CreatedByUserId = createdByUserId
        };

        role.AddDomainEvent(new RoleCreatedDomainEvent(role.Id, role.Name));

        return role;
    }

    public void Grant(PermissionId permissionId)
    {
        if (RolePermissions.Any(p => p.PermissionId == permissionId))
            return;
        
        _rolePermissions.Add(new RolePermission { RoleId = Id, PermissionId = permissionId });
        AddDomainEvent(new RolePermissionsChangedDomainEvents(Id, Name, permissionId));
    }

    public void Revoke(PermissionId permissionId)
    {
        var rp = RolePermissions.FirstOrDefault(p => p.PermissionId == permissionId);
        if (rp is null)
            return;
        
        _rolePermissions.Remove(rp);
        AddDomainEvent(new RolePermissionsChangedDomainEvents(Id, Name, permissionId));
    }
}