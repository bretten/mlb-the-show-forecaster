using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.SignalR;

/// <summary>
/// Connects to internal SignalR hubs and relays their messages to external clients
/// </summary>
public sealed class SignalRMultiplexer : BackgroundService
{
    /// <summary>
    /// <see cref="GatewayHub"/> context
    /// </summary>
    private readonly IHubContext<GatewayHub> _hubContext;

    /// <summary>
    /// The SignalR hubs that are being relayed by this <see cref="SignalRMultiplexer"/>
    /// </summary>
    private readonly HashSet<RelayedHub> _relayedHubs;

    /// <summary>
    /// How often the background service should check the hub connection states
    /// </summary>
    private readonly TimeSpan _keepAliveInterval;

    /// <summary>
    /// Configuration
    /// </summary>
    private readonly Options _options;

    /// <summary>
    /// The last published messages of all methods across all hubs
    /// </summary>
    private readonly HubCurrentState _hubCurrentState;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<SignalRMultiplexer> _logger;

    /// <summary>
    /// Holds active connections to the internal SignalR hubs. Thread-safe
    /// </summary>
    private static readonly ConcurrentDictionary<RelayedHub, HubConnection> Connections = new();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hubContext"><see cref="GatewayHub"/> context</param>
    /// <param name="options">Configuration <see cref="Options"/></param>
    /// <param name="hubCurrentState">The last published messages of all methods across all hubs</param>
    /// <param name="logger">Logger</param>
    public SignalRMultiplexer(IHubContext<GatewayHub> hubContext, Options options, HubCurrentState hubCurrentState,
        ILogger<SignalRMultiplexer> logger)
    {
        _hubContext = hubContext;
        _relayedHubs = options.Hubs;
        _keepAliveInterval = options.Interval;
        _options = options;
        _logger = logger;
        _hubCurrentState = hubCurrentState;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Used to poll the hub connection states on an interval
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var initialRunDone = false; // Run immediately on startup
        while (!stoppingToken.IsCancellationRequested)
        {
            if (initialRunDone && stopwatch.ElapsedMilliseconds <= _keepAliveInterval.TotalMilliseconds) continue;

            var task = ConnectHubs();
            // If the task has already been completed, force the method to complete asynchronously and return to the caller
            if (task.GetAwaiter().IsCompleted)
            {
                await Task.Yield();
            }

            // The task is not complete yet, so execute it
            await task;

            // Restart the timer
            stopwatch.Restart();

            // No longer need to run immediately after startup
            if (!initialRunDone) initialRunDone = true;
        }
    }

    /// <inheritdoc />
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // Close all connections
        foreach (var connection in Connections)
        {
            if (connection.Value.State == HubConnectionState.Connected)
            {
                await connection.Value.StopAsync(cancellationToken);
            }
        }

        await base.StopAsync(cancellationToken);
    }

    /// <summary>
    /// Connects all the specified relayed hubs
    /// </summary>
    private async Task ConnectHubs()
    {
        foreach (var hub in _relayedHubs)
        {
            var exists = Connections.TryGetValue(hub, out var existingConnection);
            if (!exists || existingConnection == null)
            {
                var newConnection = BuildConnection(hub);
                Connections.AddOrUpdate(hub, newConnection, (_, _) => newConnection);
            }

            var connection = Connections[hub];

            // No action needed if already connected/connecting
            if (connection.State != HubConnectionState.Disconnected) continue;

            await Connect(connection, hub, isReconnect: false);
            _logger.LogInformation($"Hub connected: {hub.Url}");
        }
    }

    /// <summary>
    /// Builds a connection for the <see cref="RelayedHub"/>
    /// </summary>
    /// <param name="hub"><see cref="RelayedHub"/></param>
    /// <returns><see cref="HubConnection"/></returns>
    private HubConnection BuildConnection(RelayedHub hub)
    {
        var connectionBuilder = new HubConnectionBuilder()
            .WithUrl(hub.Url, options => { options.Transports = HttpTransportType.WebSockets; })
            .WithKeepAliveInterval(_keepAliveInterval);
        if (_options.RetryPolicy != null)
        {
            connectionBuilder.WithAutomaticReconnect(_options.RetryPolicy);
        }
        else
        {
            connectionBuilder.WithAutomaticReconnect();
        }

        var connection = connectionBuilder.Build();

        connection.Closed += async (e) =>
        {
            _logger.LogWarning(e, $"Hub connection closed due to error: {hub.Url}");
            await Task.Delay(_keepAliveInterval);
            await Connect(connection, hub, isReconnect: true);
        };

        // Setup listeners for the messages that will be relayed
        foreach (var method in hub.Methods)
        {
            connection.On<object>(method, async payload =>
            {
                _logger.LogInformation(
                    $"Received message from {hub.Url} {method}: {JsonSerializer.Serialize(payload)} ");
                // Relay the message
                await _hubContext.Clients.All.SendAsync(method, payload);
                // Store the last relayed message
                _hubCurrentState.MethodsToPayloads.AddOrUpdate(method, () => payload, (_, __) => payload);
            });
        }

        return connection;
    }

    /// <summary>
    /// Starts a <see cref="HubConnection"/> for the specified <see cref="RelayedHub"/>
    /// </summary>
    /// <param name="connection">The connection to start</param>
    /// <param name="hub">The hub the connection corresponds to</param>
    /// <param name="isReconnect">True if this is a reconnect</param>
    private async Task Connect(HubConnection connection, RelayedHub hub, bool isReconnect = false)
    {
        var re = isReconnect ? "re" : "";
        try
        {
            await connection.StartAsync();
            _logger.LogInformation($"Hub {re}connected: {hub.Url}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Hub {re}connection failed: {hub.Url}");
        }
    }

    /// <summary>
    /// Configuration
    /// </summary>
    /// <param name="Interval">How often the background service should check the hub connection states</param>
    /// <param name="Hubs">The SignalR hubs that are being relayed by this <see cref="SignalRMultiplexer"/></param>
    /// <param name="RetryPolicy"><see cref="IRetryPolicy"/> for connecting</param>
    public sealed record Options(TimeSpan Interval, HashSet<RelayedHub> Hubs, IRetryPolicy? RetryPolicy = null);
}