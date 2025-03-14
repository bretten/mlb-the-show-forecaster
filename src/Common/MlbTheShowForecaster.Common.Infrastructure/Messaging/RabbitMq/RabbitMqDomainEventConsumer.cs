﻿using System.Text;
using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq.Exceptions;
using Microsoft.Extensions.Logging;
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
    /// The RabbitMQ consumer
    /// </summary>
    private readonly AsyncEventingBasicConsumer _consumer;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<RabbitMqDomainEventConsumer<T>> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="domainEventConsumer">The underlying domain event consumer that handles the RabbitMQ messages</param>
    /// <param name="channel">The RabbitMQ channel</param>
    /// <param name="consumer">The RabbitMQ consumer</param>
    /// <param name="logger">Logger</param>
    public RabbitMqDomainEventConsumer(IDomainEventConsumer<T> domainEventConsumer, IModel channel,
        AsyncEventingBasicConsumer consumer, ILogger<RabbitMqDomainEventConsumer<T>> logger)
    {
        _domainEventConsumer = domainEventConsumer;
        _channel = channel;
        _logger = logger;
        _consumer = consumer;
        _consumer.Received += ReceivedEventHandler;
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
        var messageType = typeof(T).Name;
        var bodyString = Encoding.UTF8.GetString(args.Body.ToArray());
        if (string.IsNullOrWhiteSpace(bodyString))
        {
            var e = new RabbitMqEventBodyEmptyException($"Event body is empty for {messageType}");
            _logger.LogError(e, e.Message);
            throw e;
        }

        try
        {
            _logger.LogInformation($"Consuming message {messageType}: {bodyString}");
            await _domainEventConsumer.Handle(JsonSerializer.Deserialize<T>(bodyString)!);
            // Acknowledge the message (message is consumed)
            _channel.BasicAck(args.DeliveryTag, false);
        }
        catch (Exception e)
        {
            var ex = new RabbitMqEventBodyCouldNotBeParsedException($"Could not parse event of type {messageType}", e);
            _logger.LogError(ex, ex.Message);
            // Reject the message and send it to the dead letter exchange
            _channel.BasicReject(args.DeliveryTag, false);
            throw ex;
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