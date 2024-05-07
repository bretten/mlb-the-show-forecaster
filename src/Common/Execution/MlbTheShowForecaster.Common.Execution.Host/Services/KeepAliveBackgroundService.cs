using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;

/// <summary>
/// A <see cref="BackgroundService"/> that will keep a nested service <see cref="T"/> alive until the host is shut down
/// <para>Allows the nested service of type <see cref="T"/> to be kept alive for the duration of the application.
/// For example, <see cref="T"/> can be a listener in a message broker system</para>
/// </summary>
/// <typeparam name="T"><inheritdoc /></typeparam>
public sealed class KeepAliveBackgroundService<T>(IServiceScopeFactory serviceScopeFactory)
    : NestedBackgroundService<T>(serviceScopeFactory)
    where T : IDisposable
{
    /// <summary>
    /// Keeps the service alive by running until the host is stopped (<see cref="IHostedService.StopAsync(CancellationToken)"/>)
    /// </summary>
    /// <param name="stoppingToken"><inheritdoc cref="BackgroundService.ExecuteAsync" path="/param[@name='stoppingToken']"/></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Resolve the service in this scope
        using var scope = serviceScopeFactory.CreateScope();
        Service = scope.ServiceProvider.GetRequiredService<T>();

        // The base ExecuteAsync will ensure cleanup is done for the nested service
        await base.ExecuteAsync(stoppingToken);

        // Keep the service alive until the application is terminated
        while (!stoppingToken.IsCancellationRequested)
        {
            // Add a delay so that control is given back to the caller in BackgroundService.StartAsync()
            await Task.Delay(1, stoppingToken);
        }
    }
}