namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.SignalR;

/// <summary>
/// Represents a SignalR hub that is being relayed by <see cref="SignalRMultiplexer"/>
/// </summary>
/// <param name="Url">The hub URL</param>
/// <param name="Methods">The methods that the hub broadcasts to</param>
public sealed record RelayedHub(string Url, HashSet<string> Methods);