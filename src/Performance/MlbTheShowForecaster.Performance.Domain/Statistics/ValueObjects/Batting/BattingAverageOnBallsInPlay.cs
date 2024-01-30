using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

/// <summary>
/// Batting average on balls in play (BABIP)
/// = (H - HR) / (AB - K - HR + SF)
///
/// <para>How often a batted ball results in a hit, excluding home runs</para>
/// </summary>
public sealed class BattingAverageOnBallsInPlay : CalculatedStat
{
    /// <summary>
    /// The number of hits
    /// </summary>
    public NaturalNumber Hits { get; }

    /// <summary>
    /// The number of home runs
    /// </summary>
    public NaturalNumber HomeRuns { get; }

    /// <summary>
    /// The number of at-ats
    /// </summary>
    public NaturalNumber AtBats { get; }

    /// <summary>
    /// The number of strikeouts
    /// </summary>
    public NaturalNumber StrikeOuts { get; }

    /// <summary>
    /// The number of sacrifice flies
    /// </summary>
    public NaturalNumber SacrificeFlies { get; }


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hits">The number of hits</param>
    /// <param name="homeRuns">The number of home runs</param>
    /// <param name="atBats">The number of at-bats</param>
    /// <param name="strikeOuts">The number of strikeouts</param>
    /// <param name="sacrificeFlies">The number of sacrifice flies</param>
    private BattingAverageOnBallsInPlay(NaturalNumber hits, NaturalNumber homeRuns, NaturalNumber atBats,
        NaturalNumber strikeOuts, NaturalNumber sacrificeFlies)
    {
        Hits = hits;
        HomeRuns = homeRuns;
        AtBats = atBats;
        StrikeOuts = strikeOuts;
        SacrificeFlies = sacrificeFlies;
    }

    /// <summary>
    /// Creates <see cref="BattingAverageOnBallsInPlay"/>
    /// </summary>
    /// <param name="hits">The number of hits</param>
    /// <param name="homeRuns">The number of home runs</param>
    /// <param name="atBats">The number of at-bats</param>
    /// <param name="strikeOuts">The number of strikeouts</param>
    /// <param name="sacrificeFlies">The number of sacrifice flies</param>
    /// <returns><see cref="BattingAverageOnBallsInPlay"/></returns>
    public static BattingAverageOnBallsInPlay Create(NaturalNumber hits, NaturalNumber homeRuns, NaturalNumber atBats,
        NaturalNumber strikeOuts, NaturalNumber sacrificeFlies)
    {
        return new BattingAverageOnBallsInPlay(hits, homeRuns, atBats, strikeOuts, sacrificeFlies);
    }

    /// <summary>
    /// Calculates batting average on balls in play
    /// </summary>
    /// <returns>Batting average on balls in play</returns>
    protected override decimal Calculate()
    {
        var n = Hits.Value - HomeRuns.Value;
        var d = AtBats.Value - StrikeOuts.Value - HomeRuns.Value + SacrificeFlies.Value;
        return (decimal)n / d;
    }
}