
using HTP.App.Abstractions.Repositories.Read;
using HTP.App.Users.Services.Repositories;
using HTP.Domain.Users;
using HTP.Infrastructure.Persistence.RefreshTokens.Write;
using HTP.Infrastructure.Persistence.Repositories;
using HTP.Infrastructure.Persistence.Users.Read;
using Microsoft.Extensions.DependencyInjection;

namespace HTP.Infrastructure.Extensions.DI.Services;

public static class RepositoriesServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        //var assembly = typeof(RepositoriesServiceCollectionExtensions).Assembly;
        //var repositoryTypes = assembly.GetTypes()
        //    .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
        //    .ToList();

        //foreach (var repoType in repositoryTypes)
        //{
        //    var interfaceType = repoType.GetInterfaces()
        //        .FirstOrDefault(i => i.Name == $"I{repoType.Name}");
        //    if (interfaceType != null)
        //    {
        //        services.AddScoped(interfaceType, repoType);
        //    }
        //}

        // Write
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // Read
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();

        return services;
    }

}
