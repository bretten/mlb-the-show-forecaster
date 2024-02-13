using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

/// <summary>
/// On-base percentage (OBP)
/// = (H + BB + HBP) / (AB + BB + HBP + SF)
///
/// <para>How frequently a batter reaches base</para>
/// </summary>
public sealed class OnBasePercentage : CalculatedStat
{
    /// <summary>
    /// The number of hits
    /// </summary>
    public NaturalNumber Hits { get; }

    /// <summary>
    /// The number of base on balls
    /// </summary>
    public NaturalNumber BaseOnBalls { get; }

    /// <summary>
    /// The number of hit by pitches
    /// </summary>
    public NaturalNumber HitByPitches { get; }

    /// <summary>
    /// The number of at-bats
    /// </summary>
    public NaturalNumber AtBats { get; }

    /// <summary>
    /// The number of sacrifice flies
    /// </summary>
    public NaturalNumber SacrificeFlies { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hits">Number of hits</param>
    /// <param name="baseOnBalls">Number of base on balls or walks</param>
    /// <param name="hitByPitches">Number of hit by pitches</param>
    /// <param name="atBats">Number of at-bats</param>
    /// <param name="sacrificeFlies">Number of sacrifice flies</param>
    private OnBasePercentage(NaturalNumber hits, NaturalNumber baseOnBalls, NaturalNumber hitByPitches,
        NaturalNumber atBats, NaturalNumber sacrificeFlies)
    {
        Hits = hits;
        BaseOnBalls = baseOnBalls;
        HitByPitches = hitByPitches;
        AtBats = atBats;
        SacrificeFlies = sacrificeFlies;
    }

    /// <summary>
    /// Calculates on-base percentage
    /// </summary>
    /// <returns>On-base percentage</returns>
    protected override decimal Calculate()
    {
        var n = Hits.Value + BaseOnBalls.Value + HitByPitches.Value;
        var d = AtBats.Value + BaseOnBalls.Value + HitByPitches.Value + SacrificeFlies.Value;
        return (decimal)n / d;
    }

    /// <summary>
    /// Creates <see cref="OnBasePercentage"/>
    /// </summary>
    /// <param name="hits">Number of hits</param>
    /// <param name="baseOnBalls">Number of base on balls or walks</param>
    /// <param name="hitByPitches">Number of hit by pitches</param>
    /// <param name="atBats">Number of at-bats</param>
    /// <param name="sacrificeFlies">Number of sacrifice flies</param>
    /// <returns><see cref="OnBasePercentage"/></returns>
    public static OnBasePercentage Create(int hits, int baseOnBalls, int hitByPitches, int atBats,
        int sacrificeFlies)
    {
        return new OnBasePercentage(NaturalNumber.Create(hits), NaturalNumber.Create(baseOnBalls),
            NaturalNumber.Create(hitByPitches), NaturalNumber.Create(atBats), NaturalNumber.Create(sacrificeFlies));
    }
}