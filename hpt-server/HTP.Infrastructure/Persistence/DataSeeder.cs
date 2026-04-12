using HTP.App.Core.Settings;
using HTP.Domain.Entities.Permissions;
using HTP.Domain.Entities.Roles;
using HTP.Domain.Entities.Roles.ValueObjects;
using HTP.Domain.Entities.Users;
using HTP.Domain.ValueObjects;
using HTP.Infrastructure.Identity;
using HTP.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

public static class DataSeeder
{
    public static async Task SeedAsync(
        WriteDbContext writeDbContext,
        UserManager<AppIdentityUser> userManager,
        IConfiguration configuration,
        CancellationToken ct = default)
    {
        await SeedPermissions(writeDbContext, ct);
        await SeedRoles(writeDbContext, ct);
        await RemoveObsoletePermissions(writeDbContext, ct);
        await SeedAdminUser(writeDbContext, userManager, configuration);
    }

    private static readonly IReadOnlyList<SeederRolePermissions> _rolePersmissionSet = [
        new SeederRolePermissions(SeederRole.Admin, GetAllPermissions()),
        new SeederRolePermissions(SeederRole.User, [
            PermissionKeys.Projects.Create,
            PermissionKeys.Projects.ViewOwn,
            PermissionKeys.Projects.UpdateOwn,
            PermissionKeys.Projects.DeleteOwn
        ]),
    ];

    private static async Task SeedPermissions(
    WriteDbContext writeDbContext,
    CancellationToken ct = default)
    {
        var allPermissions = GetAllPermissions();
        var metadata = PermissionMetadata.Descriptions;

        var dbPermissions = await writeDbContext.Permissions
            .ToListAsync(ct);

        var dbPermissionIds = dbPermissions
            .Select(p => (string)p.Id)
            .ToHashSet();

        var codePermissionIds = allPermissions
            .ToHashSet();

        // Update descriptions
        foreach (var dbPermission in dbPermissions)
        {
            var permissionId = dbPermission.Id;

            if (metadata.TryGetValue(permissionId, out var newDescription))
            {
                var newDescriptionResult = PermissionDescription.Create(newDescription);
                if (newDescriptionResult.IsFailure)
                {
                    throw new InvalidOperationException(
                        $"Failed to create description for permission {permissionId}: {newDescriptionResult.Error}");
                }

                var newDescriptionVo = newDescriptionResult.Value;

                if (dbPermission.Description != newDescriptionVo)
                {
                    var result = dbPermission.SetDescription(newDescriptionVo);
                    if (result.IsFailure)
                    {
                        throw new InvalidOperationException(
                            $"Failed to update description for permission {permissionId}: {result.Error}");
                    }
                }
            }
        }

        // Add new permissions
        var toAdd = codePermissionIds
            .Where(id => !dbPermissionIds.Contains(id));

        foreach (var id in toAdd)
        {

            var permissionIdResult = PermissionId.Create(id);
            if (permissionIdResult.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Failed to create permissionId: {id}.");
            }

            metadata.TryGetValue(id, out var description);

            var descriptionResult = PermissionDescription.Create(description);
            if (descriptionResult.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Failed to create description for permission {id}: {descriptionResult.Error}.");
            }



            var createResult = Permission.Create(permissionIdResult.Value, descriptionResult.Value);
            if (createResult.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Failed to create permission {id}: {createResult.Error}");
            }

            await writeDbContext.Permissions
                .AddAsync(createResult.Value, ct);
        }

        await writeDbContext.SaveChangesAsync(ct);
    }

    private static async Task RemoveObsoletePermissions(
        WriteDbContext writeDbContext,
        CancellationToken ct = default)
    {
        var allPermissions = GetAllPermissions();
        var codePermissionIds = allPermissions.ToHashSet();

        var dbPermissions = await writeDbContext.Permissions
            .ToListAsync(ct);

        var toRemove = dbPermissions
            .Where(p => !codePermissionIds.Contains((string)p.Id))
            .ToList();

        if (toRemove.Count > 0)
        {
            writeDbContext.Permissions.RemoveRange(toRemove);
            await writeDbContext.SaveChangesAsync(ct);
        }
    }

    private static async Task SeedRoles(WriteDbContext writeDbContext, CancellationToken ct = default)
    {
        var allPerms = await writeDbContext.Permissions.ToListAsync(ct);
        var existingPermissionIds = allPerms.Select(p => (string)p.Id).ToHashSet();

        foreach (var item in _rolePersmissionSet)
        {
            var roleName = item.Role;
            var targetPermissionIds = item.Permissions
                .Where(p => existingPermissionIds.Contains(p))
                .Select(p =>
                {
                    var createResult = PermissionId.Create(p);
                    if (createResult.IsFailure)
                    {
                        throw new InvalidOperationException(
                            $"Invalid permission id '{p}' for role {roleName}: {createResult.Error}");
                    }

                    return createResult.Value;
                })
                .ToHashSet();

            var role = await writeDbContext.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(x => x.Name == roleName.ToString(), ct);

            if (role is null)
            {
                var newRoleNameResult = RoleName.Create(roleName.ToString());
                if (newRoleNameResult.IsFailure)
                    throw new InvalidOperationException($"Invalid role name {roleName}: {newRoleNameResult.Error}");

                var newRole = Role.Create(newRoleNameResult.Value, DateTimeOffset.UtcNow, Guid.NewGuid());

                writeDbContext.Roles.Add(newRole);
                await writeDbContext.SaveChangesAsync(ct);

                role = newRole;
            }

            var currentPermissions = role.RolePermissions
                .Select(rp => rp.PermissionId)
                .ToHashSet();

            var permissionsToRevoke = currentPermissions
                .Where(p => !targetPermissionIds.Contains(p))
                .ToList();

            foreach (var permId in permissionsToRevoke)
            {
                role.Revoke(permId);
            }

            var permissionsToGrant = targetPermissionIds
                .Where(p => !currentPermissions.Contains(p))
                .ToList();

            foreach (var perm in permissionsToGrant)
            {
                role.Grant(perm);
            }

            await writeDbContext.SaveChangesAsync(ct);
        }
    }

    private static async Task SeedAdminUser(
        WriteDbContext writeDb,
        UserManager<AppIdentityUser> userManager,
        IConfiguration configuration)
    {
        var seederSetting = configuration
            .GetSection(SeederSettings.SectionName)
            .Get<SeederSettings>()
                ?? throw new InvalidOperationException("Missing admin user settings.");

        var adminEmail = seederSetting.AdminLogin;

        var identityUser = await userManager.FindByEmailAsync(adminEmail);
        if (identityUser != null)
            return;

        // 1️⃣ Domain User
        var email = Email.Create(adminEmail).Value;



        var domainUser = User.Create(
            firstName: FirstName.Create("Admin").Value,
            lastName: LastName.Create("User").Value,
            email: email,
            DateTimeOffset.UtcNow);

        writeDb.Users.Add(domainUser);
        await writeDb.SaveChangesAsync();

        // 2️⃣ Identity User
        identityUser = new AppIdentityUser
        {
            Id = Guid.NewGuid(),
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            DomainUserId = domainUser.Id
        };

        var result = await userManager.CreateAsync(identityUser, seederSetting.AdminPassword);
        if (!result.Succeeded)
            throw new Exception("Failed to create identity admin user.");

        // 3️⃣ Przypisanie roli przez Domain
        var adminRole = await writeDb.Roles
            .FirstAsync(r => r.Name == SeederRole.Admin.ToString());



        domainUser.AddRole(adminRole.Id);

        await writeDb.SaveChangesAsync();
    }

    public static IReadOnlyList<string> GetAllPermissions()
    {
        return typeof(PermissionKeys)
            .GetNestedTypes(BindingFlags.Public)
            .SelectMany(t =>
                t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                 .Where(f => f.IsLiteral && !f.IsInitOnly)
                 .Select(f => (string)f.GetRawConstantValue()!))
            .ToList();
    }

    private record SeederRolePermissions(
        SeederRole Role,
        IReadOnlyList<string> Permissions
        );

    private enum SeederRole
    {
        Admin,
        User
    }
}
