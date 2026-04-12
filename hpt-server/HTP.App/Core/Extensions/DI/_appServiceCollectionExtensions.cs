using Microsoft.Extensions.DependencyInjection;

namespace HTP.App.Core.Extensions.DI;

public static class AppServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.RegisterMediatorCommandsAndQueries();
        services.RegisterDomainEventHandlers();
        
        services.AddPolicies();

        return services;
    }
}
