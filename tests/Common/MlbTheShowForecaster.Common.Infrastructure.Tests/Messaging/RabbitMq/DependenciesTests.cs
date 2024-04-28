using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
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
        // Mapping of domain event types to their RabbitMQ exchanges
        var domainEventsToExchanges = new Dictionary<Type, string>()
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

        /*
         * Act
         */
        mockServices.Object.AddRabbitMq(stubConnectionFactory.Object, domainEventsToExchanges, assembliesToScan);

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

        // The domain event dispatcher should be registered as a transient so it gets a new channel from the connection every time
        mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IDomainEventDispatcher) &&
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
        // Assemblies that will be scanned for implementations of domain event consumers
        var assembliesToScan = new List<Assembly>() { GetType().Assembly };
        // Mapping of domain event types to their RabbitMQ exchanges
        var domainEventsToExchanges = new Dictionary<Type, string>()
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
        serviceCollection.AddRabbitMq(stubConnectionFactory.Object, domainEventsToExchanges, assembliesToScan);

        /*
         * Act
         */
        var actual = serviceCollection.BuildServiceProvider();
        // Get the domain event consumers so that it invokes the service provider that provides channels
        var consumer1 = actual.GetRequiredService<IDomainEventConsumer<EventType1>>();
        var consumer1B = actual.GetRequiredService<IDomainEventConsumer<EventType1>>();
        var consumer2 = actual.GetRequiredService<IDomainEventConsumer<EventType2>>();
        var consumer3 = actual.GetRequiredService<IDomainEventConsumer<EventType3>>();

        /*
         * Assert
         */
        // The connection singleton should have CreateModel called one time for each consumer
        // The real IConnection will provide a new channel for each call to CreateModel
        stubConnection.Verify(x => x.CreateModel(), Times.Exactly(4));
    }

    private sealed class DomainEventConsumer1(IModel model) : IDomainEventConsumer<EventType1>
    {
        public Task Handle(EventType1 e)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class DomainEventConsumer2(IModel model) : IDomainEventConsumer<EventType2>
    {
        public Task Handle(EventType2 e)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class DomainEventConsumer3(IModel model) : IDomainEventConsumer<EventType3>
    {
        public Task Handle(EventType3 e)
        {
            return Task.CompletedTask;
        }
    }

    private sealed record EventType1 : IDomainEvent;

    private sealed record EventType2 : IDomainEvent;

    private sealed record EventType3 : IDomainEvent;
}