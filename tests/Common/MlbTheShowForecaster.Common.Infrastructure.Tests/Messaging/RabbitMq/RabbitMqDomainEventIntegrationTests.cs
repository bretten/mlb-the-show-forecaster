using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq.TestClasses;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Testcontainers.RabbitMq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq;

public class RabbitMqDomainEventIntegrationTests : IAsyncLifetime
{
    private readonly RabbitMqContainer _container;

    public RabbitMqDomainEventIntegrationTests()
    {
        try
        {
            _container = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithName(GetType().Name + Guid.NewGuid())
                .WithPortBinding(56720, 5672)
                .WithPortBinding(15673, 15672)
                .WithCommand("rabbitmq-server", "rabbitmq-plugins enable --offline rabbitmq_management")
                .Build();
        }
        catch (ArgumentException e)
        {
            if (!e.Message.Contains("Docker is either not running or misconfigured"))
            {
                throw;
            }

            throw new DockerNotRunningException($"Docker is required to run tests for {GetType().Name}");
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Dispatch_PublishesDomainEvents_ConsumerReceivesEvents()
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
            { typeof(EventType3), "exC" }
        };
        // Consumer's mapping of domain event types to their RabbitMQ exchanges
        var consumerDomainEventsToExchanges = new Dictionary<Type, string>()
        {
            { typeof(EventType1), "exA" },
            { typeof(EventType2), "exB" },
            { typeof(EventType3), "exC" }
        };

        // Rabbit MQ connection
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(_container.GetConnectionString())
        };

        // Add RabbitMQ dependencies
        serviceCollection.AddRabbitMq(connectionFactory, publisherDomainEventsToExchanges,
            consumerDomainEventsToExchanges, assembliesToScan);

        // Build the services
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Resolve the dispatcher
        var dispatcher = serviceProvider.GetRequiredService<IDomainEventDispatcher>();

        /*
         * Act
         */
        // Send a message
        dispatcher.Dispatch(new List<IDomainEvent>()
        {
            new EventType1(),
            new EventType2(),
            new EventType3()
        });

        /*
         * Assert
         */
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.StopAsync();

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}