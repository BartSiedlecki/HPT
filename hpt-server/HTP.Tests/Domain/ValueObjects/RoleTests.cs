using FluentAssertions;
using HTP.Domain.Entities.Permissions;
using HTP.Domain.Entities.Roles;
using HTP.Domain.Entities.Roles.Events;
using HTP.Domain.Entities.Roles.ValueObjects;
using HTP.Domain.ValueObjects;

namespace HTP.UnitTests.Domain.ValueObjects;

public class RoleTests
{
    [Fact]
    public void Create_ShouldSucceed_WhenParamsAreValid()
    {
        // arrange
        var roleName = RoleName.Create("Admin").Value;
        var createdAt = DateTimeOffset.UtcNow;
        var userId = Guid.NewGuid();

        // act
        var createdRole = Role.Create(roleName, createdAt, userId);

        // assert
        createdRole.Should().NotBeNull();
        createdRole.Name.Should().Be(roleName);
        createdRole.CreatedAt.Should().Be(createdAt);
        createdRole.CreatedByUserId.Should().Be(userId);
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenParamsAreValid()
    {
        // arrange
        var roleName = RoleName.Create("Admin").Value;
        var createdAt = DateTimeOffset.UtcNow;
        var userId = Guid.NewGuid();

        // act
        var createdRole = Role.Create(roleName, createdAt, userId);

        // assert
        createdRole.DomainEvents.Should().Contain(new RoleCreatedDomainEvent(createdRole.Id, roleName));
    }

    [Fact]
    public void Grant_ShouldAddPermission_WhenPermissionIsNew()
    {
        // arrange
        var role = CreateValidRole();
        var permissionId1 = PermissionId.Create(PermissionKeys.Users.Delete).Value;
        var permissionId2 = PermissionId.Create(PermissionKeys.Users.ViewAll).Value;

        // act
        role.Grant(permissionId1);
        role.Grant(permissionId2);

        // assert
        role.RolePermissions.Should().HaveCount(2);
        role.RolePermissions.Should().Contain(x => x.PermissionId == permissionId1);
        role.RolePermissions.Should().Contain(x => x.PermissionId == permissionId2);
    }

    [Fact]
    public void Grant_ShouldIgnorePermission_WhenAlreadyExists()
    {
        // arrange
        var role = CreateValidRole();
        var permissionId1 = PermissionId.Create(PermissionKeys.Users.Delete).Value;
        var permissionId2 = PermissionId.Create(PermissionKeys.Users.Delete).Value;

        // act
        role.Grant(permissionId1);
        role.Grant(permissionId2);

        // assert
        role.RolePermissions.Should().HaveCount(1);
        role.RolePermissions.Should().Contain(x => x.PermissionId == permissionId1);
    }

    [Fact]
    public void Grant_ShouldRaiseDomainEvent_WhenPermissionChanged()
    {
        // arrange
        var role = CreateValidRole();
        var permissionId = PermissionId.Create(PermissionKeys.Users.Delete).Value;

        // act
        role.Grant(permissionId);

        // assert
        role.DomainEvents.Should().Contain(new RolePermissionsChangedDomainEvents(role.Id, role.Name, permissionId));
    }

    [Fact]
    public void Grant_ShouldNotRaiseEvent_WhenPermissionAlreadyExists()
    {
        var role = CreateValidRole();
        var permissionId = PermissionId.Create(PermissionKeys.Users.Delete).Value;

        role.Grant(permissionId);
        role.ClearDomainEvents();

        role.Grant(permissionId);

        role.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Revoke_ShouldRemovePermission_WhenExists()
    {
        // arrange
        var role = CreateValidRole();
        var permissionId1 = PermissionId.Create(PermissionKeys.Users.Create).Value;
        var permissionId2 = PermissionId.Create(PermissionKeys.Users.Delete).Value;

        // act
        role.Grant(permissionId1);
        role.Grant(permissionId2);
        role.Revoke(permissionId2);

        // assert
        role.RolePermissions.Should().HaveCount(1);
        role.RolePermissions.Should().Contain(x => x.PermissionId == permissionId1);
        role.RolePermissions.Should().NotContain(x => x.PermissionId == permissionId2);
    }

    [Fact]
    public void Revoke_ShouldRaiseDomainEvent_WhenPermissionChanged()
    {
        // arrange
        var role = CreateValidRole();
        var permissionId1 = PermissionId.Create(PermissionKeys.Users.Create).Value;

        // act
        role.Grant(permissionId1);
        role.Revoke(permissionId1);

        // assert
        role.DomainEvents.Should().Contain(new RolePermissionsChangedDomainEvents(role.Id, role.Name, permissionId1));
    }

    [Fact]
    public void Revoke_ShouldDoNothing_WhenPermissionDoesNotExist()
    {
        // arrange
        var role = CreateValidRole();
        var permissionId1 = PermissionId.Create(PermissionKeys.Users.Create).Value;

        // act
        role.Revoke(permissionId1);

        // assert
        role.DomainEvents.Should().NotContain(new RolePermissionsChangedDomainEvents(role.Id, role.Name, permissionId1));
    }

    private Role CreateValidRole()
    {
        var roleName = RoleName.Create("Admin").Value;
        var createdAt = DateTimeOffset.UtcNow;
        var userId = Guid.NewGuid();

        return Role.Create(roleName, createdAt, userId);
    }
}
