using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Tests.Services;

public class KeepAliveBackgroundServiceIntegrationTests
{
    [Fact]
    public async Task ExecuteAsync_DisposableService_ExecutesUntilStopIsTriggered()
    {
        /*
         * Arrange
         */
        const int totalRuntimeInMs = 100;
        const int stopHostedServiceAfterMs = 50;
        // The underlying service of the HostedService
        var mockListener = new Mock<ITestListener>();
        // Register all the services
        IServiceCollection services = new ServiceCollection();
        // Add the service that should run for the lifetime of the app
        services.AddSingleton(mockListener.Object);
        // Add the BackgroundService that keeps the nested service alive
        services.AddHostedService<KeepAliveBackgroundService<ITestListener>>();
        // Service provider
        var serviceProvider = services.BuildServiceProvider();
        var s = serviceProvider.GetRequiredService<IHostedService>() as KeepAliveBackgroundService<ITestListener>;
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
        // The underlying service should have been disposed of, even when stopped via a CancellationToken
        mockListener.Verify(x => x.Dispose(), Times.Once);
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

    public interface ITestListener : IDisposable;
}