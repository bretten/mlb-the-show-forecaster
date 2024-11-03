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
    /// The last published messages of all methods across all hubs
    /// </summary>
    private readonly HubCurrentState _state;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="state">The last published messages of all methods across all hubs</param>
    public GatewayHub(ILogger<GatewayHub> logger, HubCurrentState state)
    {
        _logger = logger;
        _state = state;
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

    /// <summary>
    /// Returns the current state of all the methods
    /// </summary>
    /// <returns>The last published messages of all methods across all hubs</returns>
    public Task<Dictionary<string, object>> CurrentState()
    {
        return Task.FromResult(_state.MethodsToPayloads.ToDictionary());
    }
}