using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement;

/// <summary>
/// Represents the total of a stat for a period of time
/// </summary>
/// <param name="Value">The stat total</param>
/// <param name="PeriodStart">The start of the period</param>
/// <param name="PeriodEnd">The end of the period</param>
public sealed record StatSnapshot(decimal Value, DateTime PeriodStart, DateTime PeriodEnd)
{
    /// <summary>
    /// Creates <see cref="StatSnapshot"/> with <see cref="NaturalNumber"/>
    /// </summary>
    /// <param name="value">The stat total</param>
    /// <param name="startDate">The start of the period</param>
    /// <param name="endDate">The end of the period</param>
    /// <returns><see cref="StatSnapshot"/></returns>
    public static StatSnapshot Create(NaturalNumber value, DateTime startDate, DateTime endDate)
    {
        return new StatSnapshot(value.Value, startDate, endDate);
    }

    /// <summary>
    /// Creates <see cref="StatSnapshot"/> with <see cref="CalculatedStat"/>
    /// </summary>
    /// <param name="value">The stat total</param>
    /// <param name="startDate">The start of the period</param>
    /// <param name="endDate">The end of the period</param>
    /// <returns><see cref="StatSnapshot"/></returns>
    public static StatSnapshot Create(CalculatedStat value, DateTime startDate, DateTime endDate)
    {
        return new StatSnapshot(value.Value, startDate, endDate);
    }
};