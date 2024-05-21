using System.Text;
using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq.Exceptions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;

/// <summary>
/// RabbitMQ consumer that wraps a <see cref="IDomainEventConsumer{T}"/>
/// <para>This RabbitMQ consumer wrapper resolves the underlying <see cref="IDomainEventConsumer{T}"/>, consumes
/// any matching messages from RabbitMQ, and delegates them to the underlying consumer</para>
/// </summary>
/// <typeparam name="T">The domain event</typeparam>
public sealed class RabbitMqDomainEventConsumer<T> : IDisposable
{
    /// <summary>
    /// The underlying domain event consumer that handles the RabbitMQ messages
    /// </summary>
    private readonly IDomainEventConsumer<T> _domainEventConsumer;

    /// <summary>
    /// The RabbitMQ channel
    /// </summary>
    private readonly IModel _channel;

    /// <summary>
    /// The queue to consume
    /// </summary>
    public string Queue { get; }

    /// <summary>
    /// The RabbitMQ consumer
    /// </summary>
    private readonly AsyncEventingBasicConsumer _consumer;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="domainEventConsumer">The underlying domain event consumer that handles the RabbitMQ messages</param>
    /// <param name="channel">The RabbitMQ channel</param>
    /// <param name="queue">The queue to consume</param>
    public RabbitMqDomainEventConsumer(IDomainEventConsumer<T> domainEventConsumer, IModel channel, string queue)
    {
        _domainEventConsumer = domainEventConsumer;
        _channel = channel;
        Queue = queue;
        _consumer = new AsyncEventingBasicConsumer(channel);
        _consumer.Received += ReceivedEventHandler;
        _channel.BasicConsume(queue: Queue,
            autoAck: true,
            consumer: _consumer);
    }

    /// <summary>
    /// Event handler for <see cref="_consumer"/>'s Received event handler
    /// </summary>
    /// <param name="sender">The invoker of the delegate event, not to be confused with the Rabbit MQ domain event</param>
    /// <param name="args">The arguments of the Rabbit MQ domain event</param>
    /// <exception cref="RabbitMqEventBodyEmptyException">Thrown if the domain event body is empty</exception>
    /// <exception cref="RabbitMqEventBodyCouldNotBeParsedException">Thrown if the domain event body could not be properly parsed</exception>
    public async Task ReceivedEventHandler(object? sender, BasicDeliverEventArgs args)
    {
        var bodyString = Encoding.UTF8.GetString(args.Body.ToArray());
        if (string.IsNullOrWhiteSpace(bodyString))
        {
            throw new RabbitMqEventBodyEmptyException($"Event body is empty for {nameof(T)}");
        }

        try
        {
            await _domainEventConsumer.Handle(JsonSerializer.Deserialize<T>(bodyString)!);
        }
        catch (JsonException e)
        {
            throw new RabbitMqEventBodyCouldNotBeParsedException($"Could not parse event of type {nameof(T)}", e);
        }
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        // Calling Dispose/Abort/Close twice on RabbitMQ channel leads to null ref exception. Disposal is already handled by connection
        //_channel.Dispose();
    }
}