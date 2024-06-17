using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.PerformanceChanges;

/// <summary>
/// Represents a player that had a performance change
/// </summary>
public sealed record PlayerPerformanceChange : IPerformanceChange
{
    /// <summary>
    /// A comparison of the player's previous and new performance
    /// </summary>
    public PerformanceScoreComparison Comparison { get; }

    /// <summary>
    /// The player's <see cref="MlbId"/>
    /// </summary>
    public MlbId PlayerId { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="comparison">A comparison of the player's previous and new performance</param>
    /// <param name="playerId">The player's <see cref="MlbId"/></param>
    private PlayerPerformanceChange(PerformanceScoreComparison comparison, MlbId playerId)
    {
        Comparison = comparison;
        PlayerId = playerId;
    }

    /// <summary>
    /// Creates a <see cref="PlayerPerformanceChange"/>
    /// </summary>
    /// <param name="comparison">A comparison of the player's previous and new performance</param>
    /// <param name="playerId">The player's <see cref="MlbId"/></param>
    /// <returns><see cref="PlayerPerformanceChange"/></returns>
    public static PlayerPerformanceChange Create(PerformanceScoreComparison comparison, MlbId playerId)
    {
        return new PlayerPerformanceChange(comparison, playerId);
    }
}