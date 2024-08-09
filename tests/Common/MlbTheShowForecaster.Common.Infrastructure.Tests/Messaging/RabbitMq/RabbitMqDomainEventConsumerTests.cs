using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq;

public class RabbitMqDomainEventConsumerTests
{
    [Fact]
    public async Task ReceivedEventHandler_EmptyEventBody_ThrowsException()
    {
        // Arrange
        var mockUnderlyingConsumer = Mock.Of<IDomainEventConsumer<TestDomainEvent>>();
        var mockModel = Mock.Of<IModel>();
        var mockLogger = Mock.Of<ILogger<RabbitMqDomainEventConsumer<TestDomainEvent>>>();

        var consumerWrapper =
            new RabbitMqDomainEventConsumer<TestDomainEvent>(mockUnderlyingConsumer, mockModel, "queue1", mockLogger);

        var body = Encoding.UTF8.GetBytes("");
        var action = async () => await consumerWrapper.ReceivedEventHandler(consumerWrapper, CreateDelivery(body));

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<RabbitMqEventBodyEmptyException>(actual);
        Mock.Get(mockLogger).Verify(x => x.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Equals(actual.Message)),
                actual,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ReceivedEventHandler_BodyIsOfDifferentType_ThrowsException()
    {
        // Arrange
        var domainEvent = new AnotherDomainEvent(1, "Content");

        var mockUnderlyingConsumer = Mock.Of<IDomainEventConsumer<TestDomainEvent>>();
        var mockModel = Mock.Of<IModel>();
        var mockLogger = Mock.Of<ILogger<RabbitMqDomainEventConsumer<TestDomainEvent>>>();

        var consumerWrapper =
            new RabbitMqDomainEventConsumer<TestDomainEvent>(mockUnderlyingConsumer, mockModel, "queue1", mockLogger);

        var body = JsonSerializer.SerializeToUtf8Bytes(domainEvent);
        var action = async () => await consumerWrapper.ReceivedEventHandler(consumerWrapper, CreateDelivery(body));

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<RabbitMqEventBodyCouldNotBeParsedException>(actual);
        Mock.Get(mockLogger).Verify(x => x.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Equals(actual.Message)),
                actual,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ReceivedEventHandler_ValidDomainEvent_UnderlyingConsumerIsInvoked()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var domainEvent = new TestDomainEvent("{\"Message\": \"DomainEventContent\"}");

        var mockUnderlyingConsumer = Mock.Of<IDomainEventConsumer<TestDomainEvent>>();
        var mockModel = Mock.Of<IModel>();
        var mockLogger = Mock.Of<ILogger<RabbitMqDomainEventConsumer<TestDomainEvent>>>();

        var consumerWrapper =
            new RabbitMqDomainEventConsumer<TestDomainEvent>(mockUnderlyingConsumer, mockModel, "queue1", mockLogger);

        var body = JsonSerializer.SerializeToUtf8Bytes(domainEvent);

        // Act
        await consumerWrapper.ReceivedEventHandler(consumerWrapper, CreateDelivery(body));

        // Assert
        Mock.Get(mockUnderlyingConsumer).Verify(x => x.Handle(domainEvent, cToken));
    }

    public sealed record TestDomainEvent(
        [property: JsonRequired]
        string Message
    ) : IDomainEvent;

    public sealed record AnotherDomainEvent(
        [property: JsonRequired]
        int Property1,
        string Property2
    ) : IDomainEvent;

    private static BasicDeliverEventArgs CreateDelivery(byte[] body)
    {
        return new BasicDeliverEventArgs("consumerTag", 1, false, "exchange", "routingKey", null, body);
    }
}