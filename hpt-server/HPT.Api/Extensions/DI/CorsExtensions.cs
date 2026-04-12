using HTP.App.Core.Settings;

namespace HPT.Api.Extensions.DI;

public static class CorsExtensions
{
    private const string corsPolicy = "CorsPolicy";

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = configuration
            .GetSection(CorsSettings.SectionName)
            .Get<CorsSettings>()
                ?? throw new InvalidOperationException("Missing CORS setting.");

        services.AddCors(options =>
        {
            options.AddPolicy(corsPolicy, policyBuilder =>
            {
                policyBuilder
                .WithOrigins(corsSettings.AllowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });


        return services;
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder appBuilder)
    {
        appBuilder.UseWhen(
            ctx => ctx.Request.Path.StartsWithSegments("/hangfire"),
            config => config.UseCors(corsPolicy));

        return appBuilder;
    }
}
