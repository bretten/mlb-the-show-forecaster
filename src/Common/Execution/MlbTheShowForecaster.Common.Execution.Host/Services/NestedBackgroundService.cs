using Microsoft.Extensions.Hosting;

namespace com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;

/// <summary>
/// <see cref="BackgroundService"/> that hosts a nested, underlying service <see cref="T"/> so that it can do work
/// throughout the application's lifetime
/// </summary>
/// <typeparam name="T">The nested, underlying service that will run for the lifetime of the application</typeparam>
public abstract class NestedBackgroundService<T> : BackgroundService where T : IDisposable
{
    /// <summary>
    /// The nested, underlying service that will run for the lifetime of the application
    /// </summary>
    protected readonly T Service;

    /// <summary>
    /// True if the nested service has already been disposed
    /// </summary>
    protected bool IsNestedServiceDisposed;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="service">The nested, underlying service that will run for the lifetime of the application</param>
    protected NestedBackgroundService(T service)
    {
        Service = service;
    }

    /// <summary>
    /// Registers clean up duties on the <see cref="stoppingToken"/>. Inheriting classes can override and reference
    /// the base method to automatically setup clean up
    /// </summary>
    /// <param name="stoppingToken"><inheritdoc /></param>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Make sure cleanup is either down via Dispose or the CancellationToken
        stoppingToken.Register(CleanUpNestedService);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Disposes of the <see cref="BackgroundService"/> and the nested service <see cref="T"/>
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();
        // Clean up the nested service if it has not already been disposed
        if (!IsNestedServiceDisposed)
        {
            CleanUpNestedService();
        }
    }

    /// <summary>
    /// Disposes of the nested service
    /// </summary>
    private void CleanUpNestedService()
    {
        IsNestedServiceDisposed = true;
        Service.Dispose();
    }
}