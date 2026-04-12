using HTP.App.Users.Policies;
using Microsoft.Extensions.DependencyInjection;

namespace HTP.App.Core.Extensions.DI;

public static class PoliciesServiceCollectionExtensions
{
    public static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddScoped<IUserLoginPolicy, UserLoginPolicy>();

        return services;
    }
}
