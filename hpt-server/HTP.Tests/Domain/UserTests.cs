using FluentAssertions;
using HTP.Domain.Entities.Users;
using HTP.Domain.ValueObjects;

namespace HTP.UnitTests.Domain;

public class UserTests
{
    [Fact]
    public void Create_ShouldSucceed_WhenParamsAreValid()
    {
        // arrange
        var firstName = FirstName.Create("John").Value;
        var lastName = LastName.Create("Doe").Value;
        var email = Email.Create("john@example.com").Value;
        var createdAt = DateTimeOffset.UtcNow;

        // act
        var user = User.Create(firstName, lastName, email, createdAt);

        // assert
        user.Should().NotBeNull();
        user.Id.Should().NotBeEmpty();
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
        user.IsActive.Should().Be(true);
        user.IsBlocked.Should().Be(false);
        user.CreatedAt.Should().Be(createdAt);
        user.ActivatedAt.Should().BeNull();
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenParamsAreValid()
    {
        // arrange
        var firstName = FirstName.Create("John").Value;
        var lastName = LastName.Create("Doe").Value;
        var email = Email.Create("john@example.com").Value;
        var createdAt = DateTimeOffset.UtcNow;

        // act
        var user = User.Create(firstName, lastName, email, createdAt);

        // assert
        user.DomainEvents.Should().Contain(new UserCreatedDomainEvent(user.Id, email));
    }

    [Fact]
    public void UpdateProfile_ShouldUpdateFirstNameAndLastName()
    {
        // arrange
        var user = CreateValidUser();
        var newFirstName = FirstName.Create("Jane").Value;
        var newLastName = LastName.Create("Smith").Value;

        // act
        user.UpdateProfile(newFirstName, newLastName);

        // assert
        user.FirstName.Should().Be(newFirstName);
        user.LastName.Should().Be(newLastName);
    }

    [Fact]
    public void UpdateProfile_ShouldRaiseDomainEvent()
    {
        // arrange
        var user = CreateValidUser();
        var newFirstName = FirstName.Create("Jane").Value;
        var newLastName = LastName.Create("Smith").Value;

        // act
        user.UpdateProfile(newFirstName, newLastName);

        // assert
        user.DomainEvents.Should().Contain(new UserProfileUpdatedDomainEvent(user.Id, newFirstName, newLastName));
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        // arrange
        var user = CreateValidUser();

        // act
        user.Deactivate();

        // assert
        user.IsActive.Should().Be(false);
    }

    [Fact]
    public void Deactivate_ShouldRaiseDomainEvent()
    {
        // arrange
        var user = CreateValidUser();

        // act
        user.Deactivate();

        // assert
        user.DomainEvents.Should().Contain(new UserDeactivatedDomainEvent(user.Id));
    }

    [Fact]
    public void Activate_ShouldSetIsActiveTrue()
    {
        // arrange
        var user = CreateValidUser();
        user.Deactivate();
        user.ClearDomainEvents();

        // act
        user.Activate();

        // assert
        user.IsActive.Should().Be(true);
    }

    [Fact]
    public void Activate_ShouldRaiseDomainEvent()
    {
        // arrange
        var user = CreateValidUser();
        user.Deactivate();
        user.ClearDomainEvents();

        // act
        user.Activate();

        // assert
        user.DomainEvents.Should().Contain(new UserActivatedDomainEvent(user.Id));
    }

    [Fact]
    public void MarkPasswordChanged_ShouldRaiseDomainEvent()
    {
        // arrange
        var user = CreateValidUser();
        user.ClearDomainEvents();

        // act
        user.MarkPasswordChanged(DateTimeOffset.UtcNow);

        // assert
        user.DomainEvents.Should().Contain(new UserPasswordChangedDomainEvent(user.Id));
    }

    [Fact]
    public void MarkPasswordChanged_ShouldSetActivatedAt_WhenFirstTime()
    {
        // arrange
        var user = CreateValidUser();

        // act
        user.MarkPasswordChanged(DateTimeOffset.UtcNow);

        // assert
        user.ActivatedAt.Should().NotBeNull();
    }

    [Fact]
    public void MarkPasswordChanged_ShouldNotOverwriteActivatedAt_WhenAlreadySet()
    {
        // arrange
        var user = CreateValidUser();
        user.MarkPasswordChanged(DateTimeOffset.UtcNow);
        var firstActivatedAt = user.ActivatedAt;

        // act
        user.MarkPasswordChanged(DateTimeOffset.UtcNow);

        // assert
        user.ActivatedAt.Should().Be(firstActivatedAt);
    }

    [Fact]
    public void GetFullName_ShouldReturnFirstNameAndLastName()
    {
        // arrange
        var user = CreateValidUser();

        // act
        var fullName = user.GetFullName();

        // assert
        fullName.Should().Be($"{user.FirstName.Value} {user.LastName.Value}");
    }

    [Fact]
    public void AddRole_ShouldAddRole_WhenRoleIsNew()
    {
        // arrange
        var user = CreateValidUser();
        var roleId = Guid.NewGuid();

        // act
        user.AddRole(roleId);

        // assert
        user.Roles.Should().HaveCount(1);
        user.Roles.Should().Contain(r => r.RoleId == roleId);
    }

    [Fact]
    public void AddRole_ShouldIgnoreRole_WhenAlreadyExists()
    {
        // arrange
        var user = CreateValidUser();
        var roleId = Guid.NewGuid();

        // act
        user.AddRole(roleId);
        user.AddRole(roleId);

        // assert
        user.Roles.Should().HaveCount(1);
    }

    [Fact]
    public void AddRole_ShouldRaiseDomainEvent_WhenRoleIsNew()
    {
        // arrange
        var user = CreateValidUser();
        user.ClearDomainEvents();
        var roleId = Guid.NewGuid();

        // act
        user.AddRole(roleId);

        // assert
        user.DomainEvents.Should().Contain(new UserRoleAddedDomainEvent(user.Id, roleId));
    }

    [Fact]
    public void AddRole_ShouldNotRaiseEvent_WhenRoleAlreadyExists()
    {
        // arrange
        var user = CreateValidUser();
        var roleId = Guid.NewGuid();
        user.AddRole(roleId);
        user.ClearDomainEvents();

        // act
        user.AddRole(roleId);

        // assert
        user.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void RemoveRole_ShouldRemoveRole_WhenExists()
    {
        // arrange
        var user = CreateValidUser();
        var roleId1 = Guid.NewGuid();
        var roleId2 = Guid.NewGuid();
        user.AddRole(roleId1);
        user.AddRole(roleId2);

        // act
        user.RemoveRole(roleId1);

        // assert
        user.Roles.Should().HaveCount(1);
        user.Roles.Should().Contain(r => r.RoleId == roleId2);
        user.Roles.Should().NotContain(r => r.RoleId == roleId1);
    }

    [Fact]
    public void RemoveRole_ShouldRaiseDomainEvent_WhenRoleRemoved()
    {
        // arrange
        var user = CreateValidUser();
        var roleId = Guid.NewGuid();
        user.AddRole(roleId);
        user.ClearDomainEvents();

        // act
        user.RemoveRole(roleId);

        // assert
        user.DomainEvents.Should().Contain(new UserRoleRemovedDomainEvent(user.Id, roleId));
    }

    [Fact]
    public void RemoveRole_ShouldDoNothing_WhenRoleDoesNotExist()
    {
        // arrange
        var user = CreateValidUser();
        user.ClearDomainEvents();
        var roleId = Guid.NewGuid();

        // act
        user.RemoveRole(roleId);

        // assert
        user.Roles.Should().BeEmpty();
        user.DomainEvents.Should().BeEmpty();
    }

    private User CreateValidUser()
    {
        var firstName = FirstName.Create("John").Value;
        var lastName = LastName.Create("Doe").Value;
        var email = Email.Create("john@example.com").Value;
        var createdAt = DateTimeOffset.UtcNow;

        return User.Create(firstName, lastName, email, createdAt);
    }
}
