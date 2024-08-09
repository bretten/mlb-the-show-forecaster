using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq.Exceptions;
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
        var publisherDomainEventsToExchanges = new Dictionary<Type, string>()
        {
            { typeof(EventType1), "exA" },
            { typeof(EventType2), "exB" },
            { typeof(EventType3), "exC" },
        };
        // Consumer's mapping of domain event types to their RabbitMQ exchanges
        var consumerDomainEventsToExchanges = new Dictionary<Type, string>()
        {
            { typeof(EventType1), "exA" },
            { typeof(EventType2), "exB" },
            { typeof(EventType3), "exC" },
            { typeof(EventType4), "exD" },
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
        mockServices.Object.AddRabbitMq(stubConnectionFactory.Object, publisherDomainEventsToExchanges,
            consumerDomainEventsToExchanges, assembliesToScan);

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

        // Verify the exchanges were setup
        mockChannel.Verify(x => x.ExchangeDeclare("exA", ExchangeType.Fanout, false, true, null), Times.Once);
        mockChannel.Verify(x => x.ExchangeDeclare("exB", ExchangeType.Fanout, false, true, null), Times.Once);
        mockChannel.Verify(x => x.ExchangeDeclare("exC", ExchangeType.Fanout, false, true, null), Times.Once);
        mockChannel.Verify(x => x.ExchangeDeclare("exD", ExchangeType.Fanout, false, true, null), Times.Once);
        // Verify the queues were setup
        mockChannel.Verify(x => x.QueueDeclare("exA", true, false, true, null), Times.Once);
        mockChannel.Verify(x => x.QueueDeclare("exB", true, false, true, null), Times.Once);
        mockChannel.Verify(x => x.QueueDeclare("exC", true, false, true, null), Times.Once);
        mockChannel.Verify(x => x.QueueDeclare("exD", true, false, true, null), Times.Once);
        mockChannel.Verify(x => x.QueueBind("exA", "exA", "exA", null), Times.Once);
        mockChannel.Verify(x => x.QueueBind("exB", "exB", "exB", null), Times.Once);
        mockChannel.Verify(x => x.QueueBind("exC", "exC", "exC", null), Times.Once);
        mockChannel.Verify(x => x.QueueBind("exD", "exD", "exD", null), Times.Once);

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
        var publisherDomainEventsToExchanges = new Dictionary<Type, string>()
        {
            { typeof(EventType1), "exA" },
            { typeof(EventType2), "exB" },
            { typeof(EventType3), "exC" },
        };
        // Consumer's mapping of domain event types to their RabbitMQ exchanges
        var consumerDomainEventsToExchanges = new Dictionary<Type, string>()
        {
            { typeof(EventType1), "exA" },
            { typeof(EventType2), "exB" },
            { typeof(EventType3), "exC" },
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
        serviceCollection.AddRabbitMq(stubConnectionFactory.Object, publisherDomainEventsToExchanges,
            consumerDomainEventsToExchanges, assembliesToScan);

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
        Assert.Equal("exA", wrapperEventType1.Queue);

        var wrapperEventType1Alt = actual.GetRequiredService<RabbitMqDomainEventConsumer<EventType1>>();
        Assert.IsType<RabbitMqDomainEventConsumer<EventType1>>(wrapperEventType1Alt);
        Assert.Equal("exA", wrapperEventType1Alt.Queue);

        var wrapperEventType2 = actual.GetRequiredService<RabbitMqDomainEventConsumer<EventType2>>();
        Assert.IsType<RabbitMqDomainEventConsumer<EventType2>>(wrapperEventType2);
        Assert.Equal("exB", wrapperEventType2.Queue);

        var wrapperEventType3 = actual.GetRequiredService<RabbitMqDomainEventConsumer<EventType3>>();
        Assert.IsType<RabbitMqDomainEventConsumer<EventType3>>(wrapperEventType3);
        Assert.Equal("exC", wrapperEventType3.Queue);

        // Resolve the dispatcher so it gets its own channel
        Assert.IsAssignableFrom<IDomainEventDispatcher>(actual.GetRequiredService<IDomainEventDispatcher>());

        // The connection singleton should have CreateModel called one time for each consumer, once for the dispatcher, and once to setup the exchanges
        // The real IConnection will provide a new channel for each call to CreateModel
        stubConnection.Verify(x => x.CreateModel(), Times.Exactly(6));
    }

    [Fact]
    public void AddRabbitMq_ExchangeNotDefinedForConsumer_ThrowsException()
    {
        /*
         * Arrange
         */
        // Service collection
        var serviceCollection = new ServiceCollection();
        // Assemblies that will be scanned for implementations of domain event consumers
        var assembliesToScan = new List<Assembly>() { GetType().Assembly };
        // Publisher's mapping of domain event types to their RabbitMQ exchanges
        var publisherDomainEventsToExchanges = new Dictionary<Type, string>()
        {
            { typeof(EventType1), "exA" },
            { typeof(EventType2), "exB" },
            { typeof(EventType3), "exC" },
        };
        // Consumer's mapping of domain event types to their RabbitMQ exchanges
        var consumerDomainEventsToExchanges = new Dictionary<Type, string>()
        {
            { typeof(EventType1), "exA" },
            //{ typeof(EventType2), "exB" }, // Not defined
            { typeof(EventType3), "exC" },
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
        var action = () => serviceCollection.AddRabbitMq(stubConnectionFactory.Object, publisherDomainEventsToExchanges,
            consumerDomainEventsToExchanges, assembliesToScan);

        /*
         * Act
         */
        var actual = Record.Exception(action);

        /*
         * Assert
         */
        Assert.NotNull(actual);
        Assert.IsType<RabbitMqExchangeNotDefinedException>(actual);
    }
}