using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.RealTime;

/// <summary>
/// Hub that provides real time job updates to clients
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class JobHub : Hub
{
    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<JobHub> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    public JobHub(ILogger<JobHub> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    /// <inheritdoc />
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}