using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

/// <summary>
/// Base on balls per 9 innings pitched (BB/9)
/// = 9 * (BB / IP)
///
/// <para>The average number of walks per 9 innings pitched</para>
/// </summary>
public sealed class BaseOnBallsPerNine : CalculatedStat
{
    /// <summary>
    /// The number of walks
    /// </summary>
    public NaturalNumber BaseOnBalls { get; }

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public InningsCount InningsPitched { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="baseOnBalls">The number of walks</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    private BaseOnBallsPerNine(NaturalNumber baseOnBalls, InningsCount inningsPitched)
    {
        BaseOnBalls = baseOnBalls;
        InningsPitched = inningsPitched;
    }

    /// <summary>
    /// Calculates base on balls per 9 innings pitched
    /// </summary>
    /// <returns>Base on balls per 9 innings pitched</returns>
    protected override decimal Calculate()
    {
        return 9 * (BaseOnBalls.Value / InningsPitched.Value);
    }

    /// <summary>
    /// Creates <see cref="BaseOnBallsPerNine"/>
    /// </summary>
    /// <param name="baseOnBalls">The number of walks</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <returns><see cref="BaseOnBallsPerNine"/></returns>
    public static BaseOnBallsPerNine Create(int baseOnBalls, decimal inningsPitched)
    {
        return new BaseOnBallsPerNine(NaturalNumber.Create(baseOnBalls), InningsCount.Create(inningsPitched));
    }
}