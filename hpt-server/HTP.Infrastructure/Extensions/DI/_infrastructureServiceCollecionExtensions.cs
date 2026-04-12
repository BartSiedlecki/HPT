using HTP.Infrastructure.Extensions.DI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HTP.Infrastructure.Extensions.DI;

public static class InfrastructureServiceCollecionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabases(configuration);
        services.AddIdentityServices();
        services.AddCommonServices();
        services.AddRepositories();

        return services;
    }
}
