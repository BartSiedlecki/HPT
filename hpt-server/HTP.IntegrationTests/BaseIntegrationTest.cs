using HTP.App.Core.Abstractions.Mediator;
using HTP.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace HTP.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly IDispatcher Dispatcher;
    protected readonly WriteDbContext WriteDbContext;
    protected readonly ReadDbContext ReadDbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Dispatcher = _scope.ServiceProvider.GetRequiredService<IDispatcher>();
        WriteDbContext = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        ReadDbContext = _scope.ServiceProvider.GetRequiredService<ReadDbContext>();
    }

    protected T GetRequiredService<T>() where T : notnull
    {
        return _scope.ServiceProvider.GetRequiredService<T>();
    }
}
