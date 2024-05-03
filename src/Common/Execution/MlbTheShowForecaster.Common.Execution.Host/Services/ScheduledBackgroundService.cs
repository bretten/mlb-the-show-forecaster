using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;

/// <summary>
/// A <see cref="BackgroundService"/> that will run the containing service on the specified time interval for the entirety
/// of the application's lifetime
/// <para>The <see cref="BackgroundService"/> will run an action on <see cref="T"/> and will be stopped when the host is shut down</para>
/// </summary>
/// <typeparam name="T"><inheritdoc /></typeparam>
public sealed class ScheduledBackgroundService<T> : NestedBackgroundService<T> where T : IDisposable
{
    /// <summary>
    /// The work that the service will do on an interval
    /// </summary>
    private readonly Func<T, Task> _action;

    /// <summary>
    /// The interval to run the nested service's work on
    /// </summary>
    private readonly TimeSpan _interval;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="service"><inheritdoc /></param>
    /// <param name="action">The work that the service will do on an interval</param>
    /// <param name="interval">The interval to run the nested service's work on</param>
    public ScheduledBackgroundService(T service, Func<T, Task> action, TimeSpan interval) : base(service)
    {
        _action = action;
        _interval = interval;
    }

    /// <summary>
    /// Runs the nested service's work on the specified interval until the host is stopped (<see cref="IHostedService.StopAsync(CancellationToken)"/>)
    /// </summary>
    /// <param name="stoppingToken"><inheritdoc cref="BackgroundService.ExecuteAsync" path="/param[@name='stoppingToken']"/></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // The base ExecuteAsync will ensure cleanup is done for the nested service
        await base.ExecuteAsync(stoppingToken);

        // Timer for the nested service
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Check the timer and run the nested service on an interval
        while (!stoppingToken.IsCancellationRequested)
        {
            if (stopwatch.ElapsedMilliseconds <= _interval.TotalMilliseconds) continue;

            // Get the task that the nested service will perform
            var task = _action.Invoke(Service);
            // If the task has already been completed, force the method to complete asynchronously and return to the caller
            if (task.GetAwaiter().IsCompleted)
            {
                await Task.Yield();
            }

            // The task is not complete yet, so execute it
            await task;

            // Restart the timer now that the nested service has executed
            stopwatch.Restart();
        }
    }
}