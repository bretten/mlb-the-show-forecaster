using System.Text;
using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq.Exceptions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;

/// <summary>
/// Base <see cref="IDomainEventConsumer{T}"/> using RabbitMQ. Inheriting classes should implement <see cref="Handle"/>
/// for the specific type of message that is being consumed
/// </summary>
/// <typeparam name="T">The domain event</typeparam>
public abstract class RabbitMqDomainEventConsumer<T> : IDomainEventConsumer<T>
{
    /// <summary>
    /// The RabbitMQ channel
    /// </summary>
    protected readonly IModel Model;

    /// <summary>
    /// The RabbitMQ consumer
    /// </summary>
    protected readonly AsyncEventingBasicConsumer Consumer;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="model">The RabbitMQ channel</param>
    public RabbitMqDomainEventConsumer(IModel model)
    {
        Model = model;
        Consumer = new AsyncEventingBasicConsumer(model);
        Consumer.Received += ReceivedEventHandler;
    }

    /// <summary>
    /// Should handle the domain event of type <see cref="T"/>
    /// </summary>
    /// <param name="e">The domain event</param>
    /// <returns>The completed task</returns>
    public abstract Task Handle(T e);

    /// <summary>
    /// Event handler for <see cref="Consumer"/>'s Received event handler
    /// </summary>
    /// <param name="sender">The invoker of the delegate event, not to be confused with the Rabbit MQ domain event</param>
    /// <param name="args">The arguments of the Rabbit MQ domain event</param>
    /// <exception cref="RabbitMqEventBodyEmptyException">Thrown if the domain event body is empty</exception>
    /// <exception cref="RabbitMqEventBodyCouldNotBeParsedException">Thrown if the domain event body could not be properly parsed</exception>
    protected virtual async Task ReceivedEventHandler(object? sender, BasicDeliverEventArgs args)
    {
        var bodyString = Encoding.UTF8.GetString(args.Body.ToArray());
        if (string.IsNullOrWhiteSpace(bodyString))
        {
            throw new RabbitMqEventBodyEmptyException($"Event body is empty for {nameof(T)}");
        }

        try
        {
            await Handle(JsonSerializer.Deserialize<T>(bodyString)!);
        }
        catch (JsonException e)
        {
            throw new RabbitMqEventBodyCouldNotBeParsedException($"Could not parse event of type {nameof(T)}", e);
        }
    }
}