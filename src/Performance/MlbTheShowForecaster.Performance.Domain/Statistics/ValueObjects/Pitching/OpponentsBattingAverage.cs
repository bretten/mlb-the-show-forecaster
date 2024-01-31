﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

/// <summary>
/// Opponents' batting average (OBA)
/// = H / (BF - BB - HBP - SH - SF - CINT)
///
/// <para>The batting average of hitters against a pitcher. Measures the pitcher's ability to prevent hits during
/// at-bats</para>
/// </summary>
public sealed class OpponentsBattingAverage : CalculatedStat
{
    /// <summary>
    /// The number of hits
    /// </summary>
    public NaturalNumber Hits { get; }

    /// <summary>
    /// The number of batters faced
    /// </summary>
    public NaturalNumber BattersFaced { get; }

    /// <summary>
    /// The number of walks
    /// </summary>
    public NaturalNumber BaseOnBalls { get; }

    /// <summary>
    /// The number of hit by pitches
    /// </summary>
    public NaturalNumber HitBatsmen { get; }

    /// <summary>
    /// The number of sacrifice hits
    /// </summary>
    public NaturalNumber SacrificeHits { get; }

    /// <summary>
    /// The number of sacrifice flies
    /// </summary>
    public NaturalNumber SacrificeFlies { get; }

    /// <summary>
    /// The number of catcher's interferences
    /// </summary>
    public NaturalNumber CatchersInterferences { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hits">The number of hits</param>
    /// <param name="battersFaced">The number of batters faced</param>
    /// <param name="baseOnBalls">The number of walks</param>
    /// <param name="hitBatsmen">The number of hit by pitches</param>
    /// <param name="sacrificeHits">The number of sacrifice hits</param>
    /// <param name="sacrificeFlies">The number of sacrifice flies</param>
    /// <param name="catchersInterferences">The number of catcher's interferences</param>
    private OpponentsBattingAverage(NaturalNumber hits, NaturalNumber battersFaced, NaturalNumber baseOnBalls,
        NaturalNumber hitBatsmen, NaturalNumber sacrificeHits, NaturalNumber sacrificeFlies,
        NaturalNumber catchersInterferences)
    {
        Hits = hits;
        BattersFaced = battersFaced;
        BaseOnBalls = baseOnBalls;
        HitBatsmen = hitBatsmen;
        SacrificeHits = sacrificeHits;
        SacrificeFlies = sacrificeFlies;
        CatchersInterferences = catchersInterferences;
    }

    /// <summary>
    /// Calculates opponents' batting average
    /// </summary>
    /// <returns>Opponents' batting average</returns>
    protected override decimal Calculate()
    {
        return (decimal)Hits.Value / (BattersFaced.Value - BaseOnBalls.Value - HitBatsmen.Value -
                                      SacrificeHits.Value - SacrificeFlies.Value - CatchersInterferences.Value);
    }

    /// <summary>
    /// Creates <see cref="OpponentsBattingAverage"/>
    /// </summary>
    /// <param name="hits">The number of hits</param>
    /// <param name="battersFaced">The number of batters faced</param>
    /// <param name="baseOnBalls">The number of walks</param>
    /// <param name="hitBatsmen">The number of hit by pitches</param>
    /// <param name="sacrificeHits">The number of sacrifice hits</param>
    /// <param name="sacrificeFlies">The number of sacrifice flies</param>
    /// <param name="catchersInterferences">The number of catcher's interferences</param>
    /// <returns><see cref="OpponentsBattingAverage"/></returns>
    public static OpponentsBattingAverage Create(uint hits, uint battersFaced, uint baseOnBalls, uint hitBatsmen,
        uint sacrificeHits, uint sacrificeFlies, uint catchersInterferences)
    {
        var hitsValue = NaturalNumber.Create(hits);
        var battersFacedValue = NaturalNumber.Create(battersFaced);
        var baseOnBallsValue = NaturalNumber.Create(baseOnBalls);
        var hitByPitchesValue = NaturalNumber.Create(hitBatsmen);
        var sacrificeHitsValue = NaturalNumber.Create(sacrificeHits);
        var sacrificeFliesValue = NaturalNumber.Create(sacrificeFlies);
        var catchersInterferencesValue = NaturalNumber.Create(catchersInterferences);
        return new OpponentsBattingAverage(hitsValue, battersFacedValue, baseOnBallsValue, hitByPitchesValue,
            sacrificeHitsValue, sacrificeFliesValue, catchersInterferencesValue);
    }
}