namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.GatewayConfig;

/// <summary>
/// Represents an app that the gateway directs traffic to
/// </summary>
/// <param name="Scheme">The scheme</param>
/// <param name="Host">The hostname</param>
/// <param name="Port">The port</param>
/// <param name="Methods">The SignalR methods that the app listens for</param>
public sealed record TargetApp(string Scheme, string Host, int Port, HashSet<string> Methods)
{
    /// <summary>
    /// The url of the <see cref="TargetApp"/>
    /// </summary>
    public string Url => $"{Scheme}://{Host}:{Port}";
};