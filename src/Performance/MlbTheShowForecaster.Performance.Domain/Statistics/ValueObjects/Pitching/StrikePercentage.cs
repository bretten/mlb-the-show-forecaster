using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

/// <summary>
/// Strike percentage = S%
///
/// <para>Percentage of pitches thrown that are strikes</para>
/// </summary>
public sealed class StrikePercentage : CalculatedStat
{
    /// <summary>
    /// The number of strikes
    /// </summary>
    public NaturalNumber Strikes { get; }

    /// <summary>
    /// The number of pitches thrown
    /// </summary>
    public NaturalNumber NumberOfPitches { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="strikes">The number of strikes</param>
    /// <param name="numberOfPitches">The number of pitches thrown</param>
    public StrikePercentage(NaturalNumber strikes, NaturalNumber numberOfPitches)
    {
        Strikes = strikes;
        NumberOfPitches = numberOfPitches;
    }

    /// <summary>
    /// Calculates strike percentage
    /// </summary>
    /// <returns>Strike percentage</returns>
    protected override decimal Calculate()
    {
        return (decimal)Strikes.Value / NumberOfPitches.Value;
    }

    /// <summary>
    /// Creates <see cref="StrikePercentage"/>
    /// </summary>
    /// <param name="strikes">The number of strikes</param>
    /// <param name="numberOfPitches">The number of pitches thrown</param>
    /// <returns><see cref="StrikePercentage"/></returns>
    public static StrikePercentage Create(uint strikes, uint numberOfPitches)
    {
        return new StrikePercentage(NaturalNumber.Create(strikes), NaturalNumber.Create(numberOfPitches));
    }
}