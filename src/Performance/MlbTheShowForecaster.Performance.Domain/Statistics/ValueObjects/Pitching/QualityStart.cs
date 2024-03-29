﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

/// <summary>
/// Quality start (QS)
///
/// <para>A game where a pitcher completes at least 6 innings and has no more than 3 earned runs</para>
/// </summary>
public sealed class QualityStart : ValueObject
{
    /// <summary>
    /// True if it was a quality start, otherwise false
    /// </summary>
    public bool Value => InningsPitched.Value >= 6 && EarnedRuns.Value <= 3;

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public InningsCount InningsPitched { get; }

    /// <summary>
    /// The number of earned runs
    /// </summary>
    public NaturalNumber EarnedRuns { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <param name="earnedRuns">The number of earned runs</param>
    private QualityStart(InningsCount inningsPitched, NaturalNumber earnedRuns)
    {
        InningsPitched = inningsPitched;
        EarnedRuns = earnedRuns;
    }

    /// <summary>
    /// Creates <see cref="QualityStart"/>
    /// </summary>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <param name="earnedRuns">The number of earned runs</param>
    /// <returns><see cref="QualityStart"/></returns>
    public static QualityStart Create(decimal inningsPitched, int earnedRuns)
    {
        return new QualityStart(InningsCount.Create(inningsPitched), NaturalNumber.Create(earnedRuns));
    }
}