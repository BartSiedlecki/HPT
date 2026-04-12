using HTP.SharedKernel.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace HTP.App.Core.Extensions.DI;

public static class DomainEventHandlersExtensions
{
        public static IServiceCollection RegisterDomainEventHandlers(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(DomainEventHandlersExtensions))
    
                // IDomainEventHandler<T>
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
            );
    
            return services;
    }
}
