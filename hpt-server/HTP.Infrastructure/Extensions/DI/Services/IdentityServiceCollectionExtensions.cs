using HPT.SharedKernel.Constants;
using HTP.App.Abstractions.Authentication;
using HTP.App.Users;
using HTP.Infrastructure.Authentication;
using HTP.Infrastructure.Identity;
using HTP.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HTP.Infrastructure.Extensions.DI.Services;

public static class IdentityServiceCollectionExtensions
{
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
        services.AddIdentityCore<AppIdentityUser>(options =>
                {
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = FieldLengths.Password.MinLength;
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddDataProtection();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<ITokenIssuer, JwtTokenIssuer>();

        return services;
    }
}
