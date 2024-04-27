using System.Text;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq.Exceptions;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq;

public class RabbitMqDomainEventConsumerTests
{
    [Fact]
    public void ReceivedEventHandler_EmptyEventBody_ThrowsException()
    {
        // Arrange
        var domainEvent = new TestDomainEvent(string.Empty);

        var mockModel = Mock.Of<IModel>();
        var mockCallVerifier = Mock.Of<ICallVerifier>();

        var consumer = new TestRabbitMqDomainEventConsumer(mockModel, mockCallVerifier);

        // Act
        consumer.InvokeReceived(domainEvent.Message);
        var actual = consumer.ReceivedEventHandlerException;

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<RabbitMqEventBodyEmptyException>(actual);
    }

    [Fact]
    public void ReceivedEventHandler_BodyIsOfDifferentType_ThrowsException()
    {
        // Arrange
        var domainEvent = new TestDomainEvent("{\"Property1\": 1, \"Property2\": \"content\"}");

        var mockModel = Mock.Of<IModel>();
        var mockCallVerifier = Mock.Of<ICallVerifier>();

        var consumer = new TestRabbitMqDomainEventConsumer(mockModel, mockCallVerifier);

        // Act
        consumer.InvokeReceived(domainEvent.Message);
        var actual = consumer.ReceivedEventHandlerException;

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<RabbitMqEventBodyCouldNotBeParsedException>(actual);
    }

    [Fact]
    public void Handle_DomainEvent_EnsuresHandleIsInvoked()
    {
        // Arrange
        var domainEvent = new TestDomainEvent("{\"Message\": \"DomainEventContent\"}");

        var mockModel = Mock.Of<IModel>();
        var mockCallVerifier = Mock.Of<ICallVerifier>();

        var consumer = new TestRabbitMqDomainEventConsumer(mockModel, mockCallVerifier);

        // Act
        consumer.InvokeReceived(domainEvent.Message);

        // Assert
        Mock.Get(mockCallVerifier).Verify(x => x.WasInvoked("DomainEventContent"));
    }

    private sealed record TestDomainEvent(
        [property: JsonRequired]
        string Message
    );

    private sealed class TestRabbitMqDomainEventConsumer : RabbitMqDomainEventConsumer<TestDomainEvent>
    {
        /// <summary>
        /// Placed within the body of the abstract method <see cref="Handle"/> to verify that it was invoked
        /// </summary>
        private readonly ICallVerifier _callVerifier;

        /// <summary>
        /// Used to capture an exception that was thrown by the event consumer's received event handler in
        /// <see cref="RabbitMqDomainEventConsumer{T}"/>
        /// </summary>
        public Exception ReceivedEventHandlerException { get; private set; } = null!;

        public TestRabbitMqDomainEventConsumer(IModel model, ICallVerifier callVerifier) : base(model)
        {
            _callVerifier = callVerifier;
        }

        public override Task Handle(TestDomainEvent e)
        {
            _callVerifier.WasInvoked(e.Message);
            return Task.CompletedTask;
        }

        protected override async Task ReceivedEventHandler(object? sender, BasicDeliverEventArgs args)
        {
            try
            {
                await base.ReceivedEventHandler(sender, args);
            }
            catch (Exception e)
            {
                ReceivedEventHandlerException = e;
            }
        }

        public void InvokeReceived(string body)
        {
            Consumer.HandleBasicDeliver("consumerTag", 1, false, "exchange", "routingKey", null,
                Encoding.UTF8.GetBytes(body));
        }
    }

    /// <summary>
    /// Placed within the body of the abstract method <see cref="TestRabbitMqDomainEventConsumer.Handle"/> to verify that it was invoked
    /// </summary>
    public interface ICallVerifier
    {
        void WasInvoked(string message);
    }
}