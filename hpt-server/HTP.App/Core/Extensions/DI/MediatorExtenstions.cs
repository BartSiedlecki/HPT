
using FluentValidation;
using HTP.App.Core.Abstractions.Mediator;
using HTP.App.Core.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace HTP.App.Core.Extensions.DI;

public static class MediatorExtenstions
{
    public static IServiceCollection RegisterMediatorCommandsAndQueries(this IServiceCollection services)
    {
        var applicationAssembly = typeof(MediatorExtenstions).Assembly;

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(MediatorExtenstions))

            // ICommandHandler<T>
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()

            // ICommandHandler<T, TResponse>
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()

            // IQueryHandler<T, TResponse>
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()

            // IValidator<T>
            .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()

        );


        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandHandler<>));

        //// logging decorators
        services.Decorate(typeof(ICommandHandler<,>), typeof(CommandHandlerLoggingDecorator<,>)); 
        services.Decorate(typeof(IQueryHandler<,>), typeof(QueryHandlerLoggingDecorator<,>));

        return services;
    }
}
