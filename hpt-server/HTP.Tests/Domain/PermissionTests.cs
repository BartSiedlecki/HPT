using FluentAssertions;
using HTP.Domain.Entities.Permissions;
using HTP.Domain.ValueObjects;

namespace HTP.UnitTests.Domain;

public class PermissionTests
{
    [Fact]
    public void Create_ShouldSucceed_WhenParamsAreValid()
    {
        // arrange
        var permissionId = PermissionId.Create(PermissionKeys.Users.Delete).Value;
        var description = PermissionDescription.Create("Allows deleting users").Value;

        // act
        var result = Permission.Create(permissionId, description);

        // assert
        result.IsSuccess.Should().Be(true);
    }

    [Fact]
    public void Create_ShouldSucceed_WhenDescriptionIsNull()
    {
        // arrange
        var permissionId = PermissionId.Create(PermissionKeys.Users.ViewAll).Value;

        // act
        var result = Permission.Create(permissionId, null);

        // assert
        result.IsSuccess.Should().Be(true);
        result.Value.Description.Should().BeNull();
    }

    [Fact]
    public void Create_ShouldSetProperties_WhenParamsAreValid()
    {
        // arrange
        var permissionId = PermissionId.Create(PermissionKeys.Roles.Create).Value;
        var description = PermissionDescription.Create("Allows creating roles").Value;

        // act
        var result = Permission.Create(permissionId, description);

        // assert
        result.Value.Id.Should().Be(permissionId);
        result.Value.Description.Should().Be(description);
    }

    [Fact]
    public void SetDescription_ShouldUpdateDescription_WhenValid()
    {
        // arrange
        var permission = CreateValidPermission();
        var newDescription = PermissionDescription.Create("Updated description").Value;

        // act
        var result = permission.SetDescription(newDescription);

        // assert
        result.IsSuccess.Should().Be(true);
        permission.Description.Should().Be(newDescription);
    }

    [Fact]
    public void SetDescription_ShouldSetNull_WhenNullPassed()
    {
        // arrange
        var permission = CreateValidPermission();

        // act
        var result = permission.SetDescription(null);

        // assert
        result.IsSuccess.Should().Be(true);
        permission.Description.Should().BeNull();
    }

    private Permission CreateValidPermission()
    {
        var permissionId = PermissionId.Create(PermissionKeys.Users.Delete).Value;
        var description = PermissionDescription.Create("Allows deleting users").Value;

        return Permission.Create(permissionId, description).Value;
    }
}
