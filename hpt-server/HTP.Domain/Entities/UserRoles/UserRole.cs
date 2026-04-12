namespace HTP.Domain.Entities.UserRoles;

public class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    private UserRole() { }

    public UserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}
