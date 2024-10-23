using System.Diagnostics.CodeAnalysis;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.SignalR;

/// <summary>
/// Hub that acts as a gateway to internal SignalR hubs and relays messages to external clients
/// </summary>
[ExcludeFromCodeCoverage]
[Authorize(Policy = AuthConstants.AnyRolePolicy)]
public sealed class GatewayHub : Hub
{
    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<GatewayHub> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    public GatewayHub(ILogger<GatewayHub> logger)
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