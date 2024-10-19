using System.Diagnostics.CodeAnalysis;
using com.brettnamba.MlbTheShowForecaster.Common.Application.RealTime;
using Microsoft.AspNetCore.SignalR;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.RealTime;

/// <summary>
/// Broadcasts updates in real time to clients
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class SignalRCommService : IRealTimeCommService
{
    /// <summary>
    /// HubContext for notifying clients about job state
    /// </summary>
    private readonly IHubContext<JobHub> _hubContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hubContext">HubContext for notifying clients about job state</param>
    public SignalRCommService(IHubContext<JobHub> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <inheritdoc />
    public async Task Broadcast(string channel, object payload, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients.All.SendAsync(channel, payload, cancellationToken);
    }
}