namespace HTP.Domain.Entities.Permissions;

public static class PermissionMetadata
{
    public static readonly IReadOnlyDictionary<string, string> Descriptions =
        new Dictionary<string, string>
        {
            // Users
            { PermissionKeys.Users.ViewAll,  "Allows viewing all users." },
            { PermissionKeys.Users.Create,   "Allows creating new users." },
            { PermissionKeys.Users.UpdateAll,"Allows updating any user." },
            { PermissionKeys.Users.Delete,   "Allows deleting users." },

            // Roles
            { PermissionKeys.Roles.ViewAll,  "Allows viewing roles." },
            { PermissionKeys.Roles.Create,   "Allows creating new roles." },
            { PermissionKeys.Roles.Update,   "Allows updating roles." },
            { PermissionKeys.Roles.Delete,   "Allows deleting roles." }
        };
}
