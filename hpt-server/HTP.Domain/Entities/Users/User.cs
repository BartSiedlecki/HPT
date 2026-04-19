using HTP.Domain.Entities.UserRoles;
using HTP.Domain.ValueObjects;
using HTP.SharedKernel;

namespace HTP.Domain.Entities.Users;

public class User : Entity
{
    public Guid Id { get; private set; }
    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public Email Email { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsBlocked { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ActivatedAt { get; private set; }

    private readonly List<UserRole> _roles = new();
    public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();

    private User() { }

    public static User Create(
        FirstName firstName,
        LastName lastName,
        Email email,
        DateTimeOffset createdAt)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            IsActive = true,
            CreatedAt = createdAt
        };

        user.AddDomainEvent(new UserCreatedDomainEvent(user.Id, user.Email));

        return user;
    }

    public void UpdateProfile(FirstName firstName, LastName lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        AddDomainEvent(new UserProfileUpdatedDomainEvent(Id, firstName, lastName));
    }

    public void Deactivate()
    {
        IsActive = false;
        AddDomainEvent(new UserDeactivatedDomainEvent(Id));
    }

    public void Activate()
    {
        IsActive = true;
        AddDomainEvent(new UserActivatedDomainEvent(Id));
    }

    public void MarkPasswordChanged(DateTimeOffset changedAt)
    {
        ActivatedAt ??= DateTimeOffset.UtcNow;
        AddDomainEvent(new UserPasswordChangedDomainEvent(Id));
    }

    public string GetFullName() => $"{FirstName.Value} {LastName.Value}";

    public void AddRole(Guid roleId)
    {
        if (_roles.Any(r => r.RoleId == roleId))
            return;

        _roles.Add(new UserRole(Id, roleId));

        AddDomainEvent(new UserRoleAddedDomainEvent(Id, roleId));
    }

    public void RemoveRole(Guid roleId)
    {
        var role = _roles.FirstOrDefault(r => r.RoleId == roleId);
        if (role == null)
            return;

        _roles.Remove(role);

        AddDomainEvent(new UserRoleRemovedDomainEvent(Id, roleId));
    }
}
