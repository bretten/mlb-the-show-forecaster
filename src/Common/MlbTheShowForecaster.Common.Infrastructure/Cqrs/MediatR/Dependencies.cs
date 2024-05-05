using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;

/// <summary>
/// Registers the MediatR CQRS module
/// </summary>
public static class Dependencies
{
    /// <summary>
    /// Registers the MediatR CQRS module
    /// </summary>
    /// <param name="services">The registered services</param>
    /// <param name="assembliesToScan">Assemblies to scan for <see cref="ICommand"/>, <see cref="IQuery{TResponse}"/>
    /// and their corresponding handlers</param>
    public static void AddMediatRCqrs(this IServiceCollection services, IList<Assembly> assembliesToScan)
    {
        // Register MediatR
        services.AddMediatR(mediatrConfig =>
        {
            mediatrConfig.RegisterServicesFromAssemblies(assembliesToScan.ToArray());
        });

        // Register the ICommandSender
        services.TryAddTransient<ICommandSender, MediatRCommandSender>();

        // Get any implementations of ICommandHandler
        assembliesToScan.SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces() // Only concrete ICommandHandlers
                .Where(i => i.IsGenericType)
                .Any(i => i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)) && !t.IsAbstract && !t.IsInterface)
            .ToList()
            .ForEach(handlerType => // For each concrete implementation of ICommandHandler
            {
                // The type as an interface, eg. ICommandHandler<TCommand>
                var handlerInterfaceType = handlerType.GetInterfaces()
                    .First(x => x.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

                // Get the command type (implementation of ICommand) for the ICommandHandler
                var commandType = handlerInterfaceType.GetGenericArguments().First();

                // Register the command handler
                services.TryAddTransient(handlerInterfaceType, handlerType);

                // Register the command handler wrapper
                var commandWrapperType = typeof(MediatRCommand<>).MakeGenericType(commandType);
                var handlerWrapperType = typeof(MediatRCommandHandler<>).MakeGenericType(commandType);
                var mediatRHandlerType = typeof(IRequestHandler<>).MakeGenericType(commandWrapperType);
                services.TryAddTransient(mediatRHandlerType, handlerWrapperType);
            });

        // Register the IQuerySender
        services.TryAddTransient<IQuerySender, MediatRQuerySender>();

        // Get any implementations of IQueryHandler
        assembliesToScan.SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces() // Only concrete IQueryHandlers
                .Where(i => i.IsGenericType)
                .Any(i => i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)) && !t.IsAbstract && !t.IsInterface)
            .ToList()
            .ForEach(handlerType => // For each concrete implementation of IQueryHandler
            {
                // The type as an interface, eg. IQueryHandler<TQuery>
                var handlerInterfaceType = handlerType.GetInterfaces()
                    .First(x => x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

                // Get the query type (implementation of IQuery) for the IQueryHandler
                var queryType = handlerInterfaceType.GetGenericArguments().First();
                var responseType = queryType.GetInterfaces().First().GetGenericArguments().First();

                // Register the query handler
                services.TryAddTransient(handlerInterfaceType, handlerType);

                // Register the query handler wrapper
                var queryWrapperType = typeof(MediatRQuery<,>).MakeGenericType(queryType, responseType);
                var handlerWrapperType = typeof(MediatRQueryHandler<,>).MakeGenericType(queryType, responseType);
                var mediatRHandlerType = typeof(IRequestHandler<,>).MakeGenericType(queryWrapperType, responseType);
                services.TryAddTransient(mediatRHandlerType, handlerWrapperType);
            });
    }
}