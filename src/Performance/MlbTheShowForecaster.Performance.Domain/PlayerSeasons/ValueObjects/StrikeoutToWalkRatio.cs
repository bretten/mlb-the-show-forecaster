using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// Strikeout-to-walk ratio (K/BB)
/// = K / BB
///
/// <para>Number of strikeouts divided by walks</para>
/// </summary>
public sealed class StrikeoutToWalkRatio : CalculatedStat
{
    /// <summary>
    /// The number of strikeouts
    /// </summary>
    public NaturalNumber Strikeouts { get; }

    /// <summary>
    /// The number of base on balls
    /// </summary>
    public NaturalNumber BaseOnBalls { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="strikeouts">The number of strikeouts</param>
    /// <param name="baseOnBalls">The number of base on balls</param>
    private StrikeoutToWalkRatio(NaturalNumber strikeouts, NaturalNumber baseOnBalls)
    {
        Strikeouts = strikeouts;
        BaseOnBalls = baseOnBalls;
    }

    /// <summary>
    /// Calculates strikeout-to-walk ratio
    /// </summary>
    /// <returns>Strikeout-to-walk ratio</returns>
    protected override decimal Calculate()
    {
        return (decimal)Strikeouts.Value / BaseOnBalls.Value;
    }

    /// <summary>
    /// Creates <see cref="StrikeoutToWalkRatio"/>
    /// </summary>
    /// <param name="strikeouts">The number of strikeouts</param>
    /// <param name="baseOnBalls">The number of base on balls</param>
    /// <returns><see cref="StrikeoutToWalkRatio"/></returns>
    public static StrikeoutToWalkRatio Create(uint strikeouts, uint baseOnBalls)
    {
        return new StrikeoutToWalkRatio(NaturalNumber.Create(strikeouts), NaturalNumber.Create(baseOnBalls));
    }
}