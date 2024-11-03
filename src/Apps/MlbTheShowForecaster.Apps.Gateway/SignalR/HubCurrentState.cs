using System.Collections.Concurrent;

namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.SignalR;

/// <summary>
/// Represents the current state of all methods on the hub (state being the last published payload)
/// </summary>
public sealed class HubCurrentState
{
    /// <summary>
    /// Mapping of all hub method names to their last published message
    /// </summary>
    public readonly ConcurrentDictionary<string, object> MethodsToPayloads = new ConcurrentDictionary<string, object>();
}