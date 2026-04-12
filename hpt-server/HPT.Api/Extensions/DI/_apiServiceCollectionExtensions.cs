namespace HPT.Api.Extensions.DI;

public static class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen();
        services.AddCorsPolicy(configuration);

        return services;
    }
}
