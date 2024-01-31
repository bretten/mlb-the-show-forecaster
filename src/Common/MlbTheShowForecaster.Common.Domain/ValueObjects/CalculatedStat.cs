using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

/// <summary>
/// Represents a MLB statistic that is calculated from multiple individual statistics in batting, pitching or fielding.
/// Since the calculation components are countable scores such as hits and home runs, the underlying value is a decimal.
/// </summary>
public abstract class CalculatedStat : ValueObject
{
    /// <summary>
    /// Holds the underlying value of the stat after it is calculated
    /// </summary>
    private decimal? _value;

    /// <summary>
    /// The underlying value of the stat
    /// </summary>
    public decimal Value
    {
        get
        {
            if (!_value.HasValue)
            {
                _value = Math.Round(Calculate(), FractionalDigitCount, MidpointRounding.AwayFromZero);
            }

            return _value.Value;
        }
    }

    /// <summary>
    /// The number of fractional digits to round the decimal value to
    /// </summary>
    protected virtual int FractionalDigitCount => 3;

    /// <summary>
    /// Calculates the stat
    /// </summary>
    /// <returns>The stat</returns>
    protected abstract decimal Calculate();
}