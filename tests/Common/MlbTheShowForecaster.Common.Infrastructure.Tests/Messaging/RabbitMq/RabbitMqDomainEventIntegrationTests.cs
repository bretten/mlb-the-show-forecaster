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
    public void Dispatch_PublishesDomainEvents_ConsumerReceivesEvents()
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
            Uri = new Uri(_container.GetConnectionString()),
            DispatchConsumersAsync = true
        };

        // Add RabbitMQ dependencies
        serviceCollection.AddRabbitMq(connectionFactory, publisherDomainEventsToExchanges,
            consumerDomainEventsToExchanges, assembliesToScan);

        // Register the underlying consumers with callbacks to verify the message was called
        var consumer1Invoked = false;
        var consumer2Invoked = false;
        var consumer3Invoked = false;
        EventWaitHandle waitHandle = new ManualResetEvent(false);
        var invokedCount = 0; // When == consumer count, set the wait handle
        serviceCollection.AddTransient<IDomainEventConsumer<EventType1>>(sp =>
        {
            return new DomainEventConsumer1()
            {
                Callback = () =>
                {
                    invokedCount++;
                    consumer1Invoked = true;
                    if (invokedCount >= 3) waitHandle.Set();
                }
            };
        });
        serviceCollection.AddTransient<IDomainEventConsumer<EventType2>>(sp =>
        {
            return new DomainEventConsumer2()
            {
                Callback = () =>
                {
                    invokedCount++;
                    consumer2Invoked = true;
                    if (invokedCount >= 3) waitHandle.Set();
                }
            };
        });
        serviceCollection.AddTransient<IDomainEventConsumer<EventType3>>(sp =>
        {
            return new DomainEventConsumer3()
            {
                Callback = () =>
                {
                    invokedCount++;
                    consumer3Invoked = true;
                    if (invokedCount >= 3) waitHandle.Set();
                }
            };
        });

        // Build the services
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Resolve the dispatcher
        var dispatcher = serviceProvider.GetRequiredService<IDomainEventDispatcher>();

        // Resolve the consumer wrappers
        var consumer1 = serviceProvider.GetRequiredService<RabbitMqDomainEventConsumer<EventType1>>();
        var consumer2 = serviceProvider.GetRequiredService<RabbitMqDomainEventConsumer<EventType2>>();
        var consumer3 = serviceProvider.GetRequiredService<RabbitMqDomainEventConsumer<EventType3>>();

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
        // Block the thread until the consumers have signaled they have received their messages
        waitHandle.WaitOne(TimeSpan.FromSeconds(30));

        /*
         * Assert
         */
        // Each consumer should have received a message
        Assert.Equal(3, invokedCount);
        Assert.True(consumer1Invoked);
        Assert.True(consumer2Invoked);
        Assert.True(consumer3Invoked);
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