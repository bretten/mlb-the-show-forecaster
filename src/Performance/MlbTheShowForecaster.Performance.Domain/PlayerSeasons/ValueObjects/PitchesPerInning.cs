using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// Pitches per inning (P/IP)
/// = NP / IP
///
/// <para>The number of pitches per inning</para>
/// </summary>
public sealed class PitchesPerInning : CalculatedStat
{
    /// <summary>
    /// The number of pitches thrown
    /// </summary>
    public NaturalNumber NumberOfPitches { get; }

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public InningsCount InningsPitched { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="numberOfPitches">The number of pitches thrown</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    private PitchesPerInning(NaturalNumber numberOfPitches, InningsCount inningsPitched)
    {
        NumberOfPitches = numberOfPitches;
        InningsPitched = inningsPitched;
    }

    /// <summary>
    /// Calculates pitches per inning
    /// </summary>
    /// <returns>Pitches per inning</returns>
    protected override decimal Calculate()
    {
        return NumberOfPitches.Value / InningsPitched.Value;
    }

    /// <summary>
    /// Creates <see cref="PitchesPerInning"/>
    /// </summary>
    /// <param name="numberOfPitches">The number of pitches thrown</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <returns><see cref="PitchesPerInning"/></returns>
    public static PitchesPerInning Create(uint numberOfPitches, decimal inningsPitched)
    {
        return new PitchesPerInning(NaturalNumber.Create(numberOfPitches), InningsCount.Create(inningsPitched));
    }
}