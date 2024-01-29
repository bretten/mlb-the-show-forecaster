using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// Opponents' on-base percentage
/// = (H + BB + HB) / (AB + BB + HB + SF)
///
/// <para>The on-base percentage of batters against the pitcher</para>
/// </summary>
public sealed class OpponentsOnBasePercentage : CalculatedStat
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
    public NaturalNumber HitBatsmen { get; }

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
    /// <param name="hits">The number of hits</param>
    /// <param name="baseOnBalls">The number of base on balls</param>
    /// <param name="hitBatsmen">The number of hit by pitches</param>
    /// <param name="atBats">The number of at-bats</param>
    /// <param name="sacrificeFlies">The number of sacrifice flies</param>
    private OpponentsOnBasePercentage(NaturalNumber hits, NaturalNumber baseOnBalls, NaturalNumber hitBatsmen,
        NaturalNumber atBats, NaturalNumber sacrificeFlies)
    {
        Hits = hits;
        BaseOnBalls = baseOnBalls;
        HitBatsmen = hitBatsmen;
        AtBats = atBats;
        SacrificeFlies = sacrificeFlies;
    }

    /// <summary>
    /// Calculates opponents' on-base percentage
    /// </summary>
    /// <returns>Opponent's on-base percentage</returns>
    protected override decimal Calculate()
    {
        return (decimal)(Hits.Value + BaseOnBalls.Value + HitBatsmen.Value) /
               (AtBats.Value + BaseOnBalls.Value + HitBatsmen.Value + SacrificeFlies.Value);
    }

    /// <summary>
    /// Creates <see cref="OpponentsOnBasePercentage"/>
    /// </summary>
    /// <param name="hits">The number of hits</param>
    /// <param name="baseOnBalls">The number of base on balls</param>
    /// <param name="hitBatsmen">The number of hit by pitches</param>
    /// <param name="atBats">The number of at-bats</param>
    /// <param name="sacrificeFlies">The number of sacrifice flies</param>
    /// <returns><see cref="OpponentsOnBasePercentage"/></returns>
    public static OpponentsOnBasePercentage Create(uint hits, uint baseOnBalls, uint hitBatsmen, uint atBats,
        uint sacrificeFlies)
    {
        return new OpponentsOnBasePercentage(NaturalNumber.Create(hits), NaturalNumber.Create(baseOnBalls),
            NaturalNumber.Create(hitBatsmen), NaturalNumber.Create(atBats), NaturalNumber.Create(sacrificeFlies));
    }
}