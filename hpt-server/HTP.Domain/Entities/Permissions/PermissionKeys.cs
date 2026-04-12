namespace HTP.Domain.Entities.Permissions;

public static class PermissionKeys
{
    public static class Users
    {
        
        public const string ViewAll = "users.read.all";
        public const string Create = "users.create";
        public const string UpdateAll = "users.update.all";
        public const string Delete = "users.delete";
    }

    public static class Roles
    {
        public const string ViewAll = "roles.read";
        public const string Create = "roles.create";
        public const string Update = "roles.update";
        public const string Delete = "roles.delete";
    }

    public static class Projects
    {
        public const string ViewAll = "projects.read.all";
        public const string ViewOwn = "projects.read.own";
        public const string Create = "projects.create";
        public const string UpdateAll = "projects.update.all";
        public const string UpdateOwn = "projects.update.own";
        public const string Delete = "projects.delete";
        public const string DeleteOwn = "projects.delete.own";
    }
}
