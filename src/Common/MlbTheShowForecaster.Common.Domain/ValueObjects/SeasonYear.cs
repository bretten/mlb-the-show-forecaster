using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

/// <summary>
/// Represents a year that a season took place
/// </summary>
public sealed class SeasonYear : ValueObject
{
    /// <summary>
    /// The first year this system considers the MLB to be active. In this case, it is when the AL and NL joined
    /// </summary>
    private const ushort FirstMlbSeasonYear = 1903;

    /// <summary>
    /// The underlying value
    /// </summary>
    public ushort Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The season year</param>
    private SeasonYear(ushort value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="SeasonYear"/>
    /// </summary>
    /// <param name="year">The season year</param>
    /// <returns><see cref="SeasonYear"/></returns>
    public static SeasonYear Create(ushort year)
    {
        if (year < FirstMlbSeasonYear)
        {
            throw new InvalidMlbSeasonYearException(
                $"The MLB was not fully formed at the year: {year}. It must be greater than {FirstMlbSeasonYear}");
        }

        return new SeasonYear(year);
    }
}