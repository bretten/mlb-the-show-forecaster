using System.Text;
using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq.TestClasses;
using Moq;
using RabbitMQ.Client;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq;

public class RabbitMqDomainEventDispatcherTests
{
    [Fact]
    public void DispatchEvents_EventsOfVaryingTypes_EventsDispatchedToCorrespondingConsumers()
    {
        // Arrange
        var domainEventPublisherTypes = new Dictionary<Type, Publisher>()
        {
            { typeof(EventType1), new Publisher("exchange", "type.1") },
            { typeof(EventType2), new Publisher("exchange", "type.2") },
            { typeof(EventType3), new Publisher("exchange", "type.3") },
        };
        var mockPublisher = new Mock<IModel>();
        var dispatcher = new RabbitMqDomainEventDispatcher(mockPublisher.Object, domainEventPublisherTypes);

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
        mockPublisher.Verify(x => x.BasicPublish("exchange", "type.1", false, null, ItIs(event1Body)), Times.Once);
        mockPublisher.Verify(x => x.BasicPublish("exchange", "type.2", false, null, ItIs(event2Body)),
            Times.Exactly(2));
        mockPublisher.Verify(x => x.BasicPublish("exchange", "type.3", false, null, ItIs(event3Body)),
            Times.Exactly(3));
    }

    private static ReadOnlyMemory<byte> ItIs(ReadOnlyMemory<byte> byteArray)
    {
        return It.Is<ReadOnlyMemory<byte>>(x => x.ToArray().SequenceEqual(byteArray.ToArray()));
    }
}