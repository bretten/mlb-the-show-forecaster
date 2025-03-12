using System.Diagnostics;
using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq.TestClasses;
using DotNet.Testcontainers.Builders;
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
                .WithPortBinding(5672, true)
                .WithPortBinding(15672, true)
                .WithCommand("rabbitmq-server", "rabbitmq-plugins enable --offline rabbitmq_management")
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilPortIsAvailable(5672, o => o.WithTimeout(TimeSpan.FromMinutes(1)))
                )
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
        serviceCollection.AddLogging();
        // Assemblies that will be scanned for implementations of domain event consumers
        var assembliesToScan = new List<Assembly>() { GetType().Assembly };
        // Publisher's mapping of domain event types to their RabbitMQ exchanges
        var domainEventPublisherTypes = new Dictionary<Type, Publisher>()
        {
            { typeof(EventType1), new Publisher("exchange", "type.1") },
            { typeof(EventType2), new Publisher("exchange", "type.2") },
            { typeof(EventType3), new Publisher("exchange", "type.3") },
        };
        // Consumer's mapping of domain event types to their RabbitMQ queues
        var domainEventConsumerTypes = new Dictionary<Type, Subscriber>()
        {
            { typeof(EventType1), new Subscriber("exchange", "type.1") },
            { typeof(EventType2), new Subscriber("exchange", "type.2") },
            { typeof(EventType3), new Subscriber("exchange", "type.3") },
        };

        // Rabbit MQ connection
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(_container.GetConnectionString()),
            DispatchConsumersAsync = true
        };

        // Add RabbitMQ dependencies
        serviceCollection.AddRabbitMq(connectionFactory, domainEventPublisherTypes, domainEventConsumerTypes,
            assembliesToScan);

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

        // Coverage for Dispose
        (dispatcher as RabbitMqDomainEventDispatcher)!.Dispose();
        consumer1.Dispose();

        /*
         * Assert
         */
        // Each consumer should have received a message
        Assert.Equal(3, invokedCount);
        Assert.True(consumer1Invoked);
        Assert.True(consumer2Invoked);
        Assert.True(consumer3Invoked);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task RabbitMq_FailedMessage_SendsToDeadLetterQueue()
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
            { typeof(DeadLetterEvent1), new Publisher("exchange", "queue-1") },
            { typeof(DeadLetterEvent2), new Publisher("exchange", "queue-2") }
        };
        // Consumer's mapping of domain event types to their RabbitMQ queues
        var domainEventConsumerTypes = new Dictionary<Type, Subscriber>()
        {
            { typeof(DeadLetterEvent1), new Subscriber("exchange", "queue-1") },
            { typeof(DeadLetterEvent2), new Subscriber("exchange", "queue-2") },
        };

        // Rabbit MQ connection
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(_container.GetConnectionString()),
            DispatchConsumersAsync = true
        };

        // Add RabbitMQ dependencies
        serviceCollection.AddRabbitMq(connectionFactory, domainEventPublisherTypes, domainEventConsumerTypes,
            assembliesToScan);

        // Build the services
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Resolve the dispatcher
        var dispatcher = serviceProvider.GetRequiredService<IDomainEventDispatcher>();

        // Resolve the consumer wrappers
        var consumer1 = serviceProvider.GetRequiredService<RabbitMqDomainEventConsumer<DeadLetterEvent1>>();
        var consumer2 = serviceProvider.GetRequiredService<RabbitMqDomainEventConsumer<DeadLetterEvent2>>();

        /*
         * Act
         */
        // Send a message
        dispatcher.Dispatch(new List<IDomainEvent>()
        {
            new DeadLetterEvent1(),
            new DeadLetterEvent1(),
            new DeadLetterEvent2(),
        });

        /*
         * Assert
         */
        // Make sure the messages were moved to the dead letter queues
        var channel = serviceProvider.GetRequiredService<IModel>();

        var conditionsMet = false;
        var timeLimit = new TimeSpan(0, 1, 0);
        var stopwatch = Stopwatch.StartNew();
        while (!conditionsMet)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None);
            if (stopwatch.Elapsed > timeLimit)
            {
                throw new TimeoutException($"Timeout waiting {nameof(RabbitMq_FailedMessage_SendsToDeadLetterQueue)}");
            }

            // There should be no messages in the main queues
            var mainQueueHasNoMessages = channel.MessageCount("queue-1") == 0
                                         && channel.MessageCount("queue-2") == 0;
            // There should be 2 messages in the dead letter queue for queue-1 and 1 for queue-2
            var deadLetterQueueHasMessage = channel.MessageCount("queue-1-poison") == 2
                                            && channel.MessageCount("queue-2-poison") == 1;

            conditionsMet = mainQueueHasNoMessages && deadLetterQueueHasMessage;
        }

        stopwatch.Stop();
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.DisposeAsync();

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}