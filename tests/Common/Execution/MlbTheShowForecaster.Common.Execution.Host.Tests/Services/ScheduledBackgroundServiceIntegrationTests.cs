using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Tests.Services;

public class ScheduledBackgroundServiceIntegrationTests
{
    [Fact]
    public async Task ExecuteAsync_ServiceOnASchedule_RunsServiceAtDefinedInterval()
    {
        /*
         * Arrange
         */
        const int totalRuntimeInMs = 100;
        const int stopHostedServiceAfterMs = 50;
        const int nestedServiceIntervalInMs = 5;
        // The nested service of the HostedService
        var mockIntervalService = new Mock<IIntervalService>();
        // Register all the services
        IServiceCollection services = new ServiceCollection();
        // Add the service that will run on a schedule
        services.AddSingleton(mockIntervalService.Object);
        // Add the BackgroundService that will handle the scheduling and execution
        services.AddHostedService<ScheduledBackgroundService<IIntervalService>>(sp =>
        {
            return new ScheduledBackgroundService<IIntervalService>(sp.GetRequiredService<IServiceScopeFactory>(),
                async service => { await service.Execute(); }, TimeSpan.FromMilliseconds(nestedServiceIntervalInMs));
        });
        // Service provider
        var serviceProvider = services.BuildServiceProvider();
        var s = serviceProvider.GetRequiredService<IHostedService>() as ScheduledBackgroundService<IIntervalService>;
        // Cancellation token that will be used to in Stop()
        var cts = new CancellationTokenSource();
        var cToken = cts.Token;

        /*
         * Act
         */
        // Start a thread that will call IHostedService.StopAsync() after the specified delay
        CancelHostedServiceAfterDelay(s!, cToken, stopHostedServiceAfterMs);
        // Start the IHostedService
        await s!.StartAsync(cToken);
        // Allow the IHostedService to perform StopAsync()
        await Task.Delay(totalRuntimeInMs, cToken);

        /*
         * Assert
         */
        // The nested service should have executed at least once
        mockIntervalService.Verify(x => x.Execute(), Times.AtLeastOnce);
        // The nested service should have been disposed of, even when stopped via a CancellationToken
        mockIntervalService.Verify(x => x.Dispose(), Times.Once);
    }

    [Fact]
    public async Task Dispose_BeforeStarting_DisposesOfServiceBeforeCancellationToken()
    {
        /*
         * Arrange
         */
        const int nestedServiceIntervalInMs = 5;
        // The nested service of the HostedService
        var mockIntervalService = new Mock<IIntervalService>();
        // Register all the services
        IServiceCollection services = new ServiceCollection();
        // Add the service that will run on a schedule
        services.AddSingleton(mockIntervalService.Object);
        // Add the BackgroundService that will handle the scheduling and execution
        services.AddHostedService<ScheduledBackgroundService<IIntervalService>>(sp =>
        {
            return new ScheduledBackgroundService<IIntervalService>(sp.GetRequiredService<IServiceScopeFactory>(),
                async service => { await service.Execute(); }, TimeSpan.FromMilliseconds(nestedServiceIntervalInMs));
        });
        // Service provider
        var serviceProvider = services.BuildServiceProvider();
        var s = serviceProvider.GetRequiredService<IHostedService>() as ScheduledBackgroundService<IIntervalService>;

        /*
         * Act
         */
        // Start the IHostedService
        await s!.StartAsync(CancellationToken.None);
        s!.Dispose();

        /*
         * Assert
         */
        // The nested service should have been disposed of manually
        mockIntervalService.Verify(x => x.Dispose(), Times.Once);
    }

    private void CancelHostedServiceAfterDelay(IHostedService s, CancellationToken cToken, int delayAmountInMs)
    {
        new Thread(Start).Start();
        return;

        async void Start()
        {
            Thread.CurrentThread.IsBackground = true;
            await Task.Delay(delayAmountInMs, cToken);
            await s.StopAsync(cToken);
        }
    }

    public interface IIntervalService : IDisposable
    {
        Task Execute();
    }
}