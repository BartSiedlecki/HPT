using HPT.SharedKernel.Constants;
using HTP.Domain.Entities.Permissions;
using HTP.Domain.Entities.RefreshTokens;
using HTP.Domain.Entities.Roles;
using HTP.Domain.Entities.UserRoles;
using HTP.Domain.Entities.Users;
using HTP.Domain.ValueObjects;
using HTP.Infrastructure.Persistence.Permissions;
using HTP.Infrastructure.Persistence.RefreshTokens.Write;
using HTP.Infrastructure.Persistence.RolePermissions.Write;
using HTP.Infrastructure.Persistence.Roles.Write;
using HTP.Infrastructure.Persistence.SharedValueObjectsConverters;
using HTP.Infrastructure.Persistence.UserRoles;
using HTP.Infrastructure.Persistence.Users.Write;
using Microsoft.EntityFrameworkCore;

namespace HTP.Infrastructure.Persistence;


public class WriteDbContext : DbContext
{
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UsersRoles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public WriteDbContext(DbContextOptions<WriteDbContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<Email>()
            .HaveConversion<EmailConverter>()
            .HaveMaxLength(FieldLengths.Email.MaxLength);

        configurationBuilder.Properties<FirstName>()
            .HaveConversion<FirstNameConverter>()
            .HaveMaxLength(FieldLengths.FirstName.MaxLength);

        configurationBuilder.Properties<LastName>()
            .HaveConversion<LastNameConverter>()
            .HaveMaxLength(FieldLengths.LastName.MaxLength);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

    }
}
