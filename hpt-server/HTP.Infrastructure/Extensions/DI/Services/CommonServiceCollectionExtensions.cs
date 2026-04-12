using HPT.SharedKernel.Abstractions;
using HTP.App.Abstractions;
using HTP.App.Abstractions.Authentication;
using HTP.App.Core.Abstractions.Mediator;
using HTP.Infrastructure.Identity;
using HTP.Infrastructure.Services;
using HTP.Infrastructure.Services.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace HTP.Infrastructure.Extensions.DI.Services;

public static class CommonServiceCollectionExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        
        services.AddScoped<IDispatcher, Dispatcher>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        services.AddSingleton<ITokenProvider, TokenProvider>();
        services.AddSingleton<IUrlProvider, UrlProvider>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
