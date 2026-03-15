using Microsoft.Extensions.DependencyInjection;

namespace BIT.Application.CQRS;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public async Task<TResponse> DispatchAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse));

        var handler = serviceProvider.GetRequiredService(handlerType);

        var handleMethod = handlerType.GetMethod("HandleAsync");

        var task = (Task<TResponse>)handleMethod!.Invoke(handler, [command, cancellationToken])!;

        return await task;
    }
}

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public async Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));

        var handler = serviceProvider.GetRequiredService(handlerType);

        var handleMethod = handlerType.GetMethod("HandleAsync");

        var task = (Task<TResponse>)handleMethod!.Invoke(handler, [query, cancellationToken])!;

        return await task;
    }
}
