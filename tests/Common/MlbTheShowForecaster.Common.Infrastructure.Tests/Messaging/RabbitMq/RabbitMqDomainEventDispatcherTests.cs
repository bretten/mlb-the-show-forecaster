using System.Text;
using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using Moq;
using RabbitMQ.Client;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq;

public class RabbitMqDomainEventDispatcherTests
{
    [Fact]
    public void DispatchEvents_EventsOfVaryingTypes_EventsDispatchedToCorrespondingConsumers()
    {
        // Arrange
        var eventTypeToExchange = new Dictionary<Type, string>()
        {
            { typeof(EventType1), "exA" },
            { typeof(EventType2), "exB" },
            { typeof(EventType3), "exC" },
        };
        var mockPublisher = new Mock<IModel>();
        var dispatcher = new RabbitMqDomainEventDispatcher(mockPublisher.Object, eventTypeToExchange);

        var events = new List<IDomainEvent>()
        {
            new EventType1(), new EventType2(), new EventType2(), new EventType3(), new EventType3(), new EventType3()
        };

        // Act
        dispatcher.Dispatch(events);

        // Assert
        var event1Body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new EventType1())));
        var event2Body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new EventType2())));
        var event3Body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new EventType3())));
        mockPublisher.Verify(x => x.BasicPublish("exA", "exA", false, null, ItIs(event1Body)), Times.Once);
        mockPublisher.Verify(x => x.BasicPublish("exB", "exB", false, null, ItIs(event2Body)), Times.Exactly(2));
        mockPublisher.Verify(x => x.BasicPublish("exC", "exC", false, null, ItIs(event3Body)), Times.Exactly(3));
    }

    private static ReadOnlyMemory<byte> ItIs(ReadOnlyMemory<byte> byteArray)
    {
        return It.Is<ReadOnlyMemory<byte>>(x => x.ToArray().SequenceEqual(byteArray.ToArray()));
    }

    private sealed record EventType1 : IDomainEvent;

    private sealed record EventType2 : IDomainEvent;

    private sealed record EventType3 : IDomainEvent;
}