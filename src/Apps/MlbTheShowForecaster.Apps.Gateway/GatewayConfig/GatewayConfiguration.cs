using System.Diagnostics.CodeAnalysis;

namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.GatewayConfig;

/// <summary>
/// Represents configuration for the gateway and the apps that it directs traffic to
/// </summary>
/// <param name="PlayerTracker">PlayerTracker <see cref="TargetApp"/></param>
/// <param name="PerformanceTracker">PerformanceTracker <see cref="TargetApp"/></param>
/// <param name="MarketplaceWatcher">MarketplaceWatcher <see cref="TargetApp"/></param>
[ExcludeFromCodeCoverage]
public sealed record GatewayConfiguration(
    TargetApp PlayerTracker,
    TargetApp PerformanceTracker,
    TargetApp MarketplaceWatcher)
{
    /// <summary>
    /// Gets a <see cref="TargetApp"/> by name
    /// </summary>
    /// <param name="name">The name</param>
    /// <returns>The corresponding <see cref="TargetApp"/></returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the name does not match</exception>
    public TargetApp GetByName(string name)
    {
        return name switch
        {
            "PlayerTracker" => PlayerTracker,
            "PerformanceTracker" => PerformanceTracker,
            "MarketplaceWatcher" => MarketplaceWatcher,
            _ => throw new ArgumentOutOfRangeException(nameof(name), name)
        };
    }

    /// <summary>
    /// Returns all <see cref="TargetApp"/>s
    /// </summary>
    public IEnumerable<TargetApp> All =>
        new List<TargetApp>() { PlayerTracker, PerformanceTracker, MarketplaceWatcher };
};