using BIT.Application.Common.Behaviors;
using BIT.Application.CQRS;
using BIT.Application.Users;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BIT.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register Mapster
        services.AddMapster();

        // Register Services
        services.AddScoped<IUserService, UserService>();

        // Register CQRS Dispatchers
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();

        // Register All Command Handlers
        var applicationAssembly = typeof(ApplicationServiceExtensions).Assembly;
        RegisterHandlers(services, applicationAssembly);

        // Register validation behaviors
        services.AddTransient(typeof(IValidationBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    {
        // Register Command Handlers
        var commandHandlerTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
            .ToList();

        foreach (var handlerType in commandHandlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

            services.AddScoped(interfaceType, handlerType);
        }

        // Register Query Handlers
        var queryHandlerTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            .ToList();

        foreach (var handlerType in queryHandlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

            services.AddScoped(interfaceType, handlerType);
        }
    }

}
