using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq.TestClasses;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RabbitMQ.Client;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq;

public class DependenciesTests
{
    [Fact]
    public void AddRabbitMq_ServicesCollection_RegistersRabbitMqDispatcherAndConsumers()
    {
        /*
         * Arrange
         */
        // Service collection
        var mockServices = new Mock<IServiceCollection>();
        // Assemblies that will be scanned for implementations of domain event consumers
        var assembliesToScan = new List<Assembly>() { GetType().Assembly };
        // Publisher's mapping of domain event types to their RabbitMQ exchanges
        var domainEventPublisherTypes = new Dictionary<Type, Publisher>()
        {
            { typeof(EventType1), new Publisher("ex", "type.1") },
            { typeof(EventType2), new Publisher("ex", "type.2") },
            { typeof(EventType3), new Publisher("ex", "type.3") },
        };
        // Consumer's mapping of domain event types to their RabbitMQ queues
        var domainEventConsumerTypes = new Dictionary<Type, Subscriber>()
        {
            { typeof(EventType1), new Subscriber("ex", "queue-1", "type.1") },
            { typeof(EventType2), new Subscriber("ex", "queue-2", "type.2") },
            { typeof(EventType3), new Subscriber("ex", "queue-3", "type.3") },
            { typeof(EventType4), new Subscriber("ex", "queue-4", "type.4") },
        };
        // Mock channel
        var mockChannel = new Mock<IModel>();

        // Stub connection
        var stubConnection = new Mock<IConnection>();
        stubConnection.Setup(x => x.CreateModel())
            .Returns(mockChannel.Object);

        // Stubbed connection factory
        var stubConnectionFactory = new Mock<IConnectionFactory>();
        stubConnectionFactory.Setup(x => x.CreateConnection())
            .Returns(stubConnection.Object);

        /*
         * Act
         */
        mockServices.Object.AddRabbitMq(stubConnectionFactory.Object, domainEventPublisherTypes,
            domainEventConsumerTypes, assembliesToScan);

        /*
         * Assert
         */
        // A single instance of the connection factory was registered
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IConnectionFactory) &&
            x.ImplementationInstance == stubConnectionFactory.Object &&
            x.Lifetime == ServiceLifetime.Singleton)), Times.Once);

        // A single instance of the connection was registered
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IConnection) &&
            x.ImplementationInstance == stubConnection.Object &&
            x.Lifetime == ServiceLifetime.Singleton)), Times.Once);

        // The Rabbit MQ channels should be registered as a transient factory so each service gets their own instance
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IModel) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);

        // Verify the exchange was set up
        mockChannel.Verify(x => x.ExchangeDeclare("ex", ExchangeType.Topic, true, false, null), Times.AtLeastOnce);
        // Verify the dead letter exchange was set up
        mockChannel.Verify(x => x.ExchangeDeclare("ex-poison", ExchangeType.Direct, true, false, null),
            Times.AtLeastOnce);
        // Verify the queues were set up
        mockChannel.Verify(x => x.QueueDeclare("queue-1", true, false, false, new Dictionary<string, object>()
        {
            { "x-dead-letter-exchange", "ex-poison" },
        }), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueDeclare("queue-2", true, false, false, new Dictionary<string, object>()
        {
            { "x-dead-letter-exchange", "ex-poison" },
        }), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueDeclare("queue-3", true, false, false, new Dictionary<string, object>()
        {
            { "x-dead-letter-exchange", "ex-poison" },
        }), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueDeclare("queue-4", true, false, false, new Dictionary<string, object>()
        {
            { "x-dead-letter-exchange", "ex-poison" },
        }), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueBind("queue-1", "ex", "type.1", null), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueBind("queue-2", "ex", "type.2", null), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueBind("queue-3", "ex", "type.3", null), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueBind("queue-4", "ex", "type.4", null), Times.AtLeastOnce);
        // Verify the dead letter queues were set up
        mockChannel.Verify(x => x.QueueDeclare("queue-1-poison", true, false, false, null), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueDeclare("queue-2-poison", true, false, false, null), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueDeclare("queue-3-poison", true, false, false, null), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueDeclare("queue-4-poison", true, false, false, null), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueBind("queue-1-poison", "ex-poison", "type.1", null), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueBind("queue-2-poison", "ex-poison", "type.2", null), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueBind("queue-3-poison", "ex-poison", "type.3", null), Times.AtLeastOnce);
        mockChannel.Verify(x => x.QueueBind("queue-4-poison", "ex-poison", "type.4", null), Times.AtLeastOnce);

        // The domain event dispatcher should be registered as a transient so it gets a new channel from the connection every time
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IDomainEventDispatcher) &&
            x.Lifetime == ServiceLifetime.Singleton)), Times.Once);

        // The Rabbit MQ consumer wrapper should have been registered for each domain event type
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(RabbitMqDomainEventConsumer<EventType1>) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(RabbitMqDomainEventConsumer<EventType2>) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(RabbitMqDomainEventConsumer<EventType3>) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);

        // The domain event consumers should have been registered
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IDomainEventConsumer<EventType1>) &&
            x.ImplementationType == typeof(DomainEventConsumer1) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IDomainEventConsumer<EventType2>) &&
            x.ImplementationType == typeof(DomainEventConsumer2) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IDomainEventConsumer<EventType3>) &&
            x.ImplementationType == typeof(DomainEventConsumer3) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);
    }

    [Fact]
    public void AddRabbitMq_MultipleConsumers_NewChannelIsCreatedForEachConsumer()
    {
        /*
         * Arrange
         */
        // Service collection
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        // Assemblies that will be scanned for implementations of domain event consumers
        var assembliesToScan = new List<Assembly>() { GetType().Assembly };
        // Publisher's mapping of domain event types to their RabbitMQ exchanges
        var domainEventPublisherTypes = new Dictionary<Type, Publisher>()
        {
            { typeof(EventType1), new Publisher("ex", "type.1") },
            { typeof(EventType2), new Publisher("ex", "type.2") },
            { typeof(EventType3), new Publisher("ex", "type.3") },
        };
        // Consumer's mapping of domain event types to their RabbitMQ queues
        var domainEventConsumerTypes = new Dictionary<Type, Subscriber>()
        {
            { typeof(EventType1), new Subscriber("ex", "queue-1", "type.1") },
            { typeof(EventType2), new Subscriber("ex", "queue-2", "type.2") },
            { typeof(EventType3), new Subscriber("ex", "queue-3", "type.3") },
        };

        // Mock channel
        var mockChannel = Mock.Of<IModel>();

        // Stub connection
        var stubConnection = new Mock<IConnection>();
        stubConnection.Setup(x => x.CreateModel())
            .Returns(mockChannel);

        // Stubbed connection factory
        var stubConnectionFactory = new Mock<IConnectionFactory>();
        stubConnectionFactory.Setup(x => x.CreateConnection())
            .Returns(stubConnection.Object);

        // Add Rabbit MQ and register domain event consumers
        serviceCollection.AddRabbitMq(stubConnectionFactory.Object, domainEventPublisherTypes, domainEventConsumerTypes,
            assembliesToScan);

        /*
         * Act
         */
        var actual = serviceCollection.BuildServiceProvider();

        /*
         * Assert
         */
        // Resolve the domain event consumers
        Assert.IsAssignableFrom<DomainEventConsumer1>(actual.GetRequiredService<IDomainEventConsumer<EventType1>>());
        Assert.IsAssignableFrom<DomainEventConsumer1>(actual.GetRequiredService<IDomainEventConsumer<EventType1>>());
        Assert.IsAssignableFrom<DomainEventConsumer2>(actual.GetRequiredService<IDomainEventConsumer<EventType2>>());
        Assert.IsAssignableFrom<DomainEventConsumer3>(actual.GetRequiredService<IDomainEventConsumer<EventType3>>());
        // Resolve the Rabbit MQ domain event consumer wrappers so that it invokes the service provider that provides channels
        var wrapperEventType1 = actual.GetRequiredService<RabbitMqDomainEventConsumer<EventType1>>();
        Assert.IsType<RabbitMqDomainEventConsumer<EventType1>>(wrapperEventType1);

        var wrapperEventType1Alt = actual.GetRequiredService<RabbitMqDomainEventConsumer<EventType1>>();
        Assert.IsType<RabbitMqDomainEventConsumer<EventType1>>(wrapperEventType1Alt);

        var wrapperEventType2 = actual.GetRequiredService<RabbitMqDomainEventConsumer<EventType2>>();
        Assert.IsType<RabbitMqDomainEventConsumer<EventType2>>(wrapperEventType2);

        var wrapperEventType3 = actual.GetRequiredService<RabbitMqDomainEventConsumer<EventType3>>();
        Assert.IsType<RabbitMqDomainEventConsumer<EventType3>>(wrapperEventType3);

        // Resolve the dispatcher so it gets its own channel
        Assert.IsAssignableFrom<IDomainEventDispatcher>(actual.GetRequiredService<IDomainEventDispatcher>());

        // The connection singleton should have CreateModel called one time for each consumer, once for the dispatcher, and once to set up the exchanges and queues
        // The real IConnection will provide a new channel for each call to CreateModel
        stubConnection.Verify(x => x.CreateModel(), Times.Exactly(6));
    }
}