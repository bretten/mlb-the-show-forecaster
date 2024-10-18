namespace com.brettnamba.MlbTheShowForecaster.Common.Application.RealTime;

/// <summary>
/// Defines a service that communicates in real time between server and client
/// </summary>
public interface IRealTimeCommService
{
    /// <summary>
    /// Broadcasts the payload to all clients on the specified channel
    /// </summary>
    /// <param name="channel">The channel to broadcast on</param>
    /// <param name="payload">The payload to broadcast</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task Broadcast(string channel, object payload, CancellationToken cancellationToken = default);
}