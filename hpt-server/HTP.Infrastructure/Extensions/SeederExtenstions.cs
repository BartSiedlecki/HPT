using HTP.Infrastructure.Identity;
using HTP.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HTP.Infrastructure.Extensions;

public static class SeederExtensions
{
    public static async Task SeedDataAsync(this IServiceProvider serviceProvider, IConfiguration configuration, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var writeDbContext = services.GetRequiredService<WriteDbContext>();
        var userManager = services.GetRequiredService<UserManager<AppIdentityUser>>();
        await DataSeeder.SeedAsync(writeDbContext, userManager, configuration, ct);
    }
}
