using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Contracts;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

/// <summary>
/// Represents a MLB statistic that is calculated from multiple individual statistics in batting, pitching or fielding.
/// Since the calculation components are countable scores such as hits and home runs, the underlying value is a decimal.
/// </summary>
public abstract class CalculatedStat : ValueObject, IStat
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
            if (_value.HasValue)
            {
                return _value.Value;
            }

            // If the calculation involves dividing by zero, consider it to be zero as MLB does
            try
            {
                _value = Math.Round(Calculate(), FractionalDigitCount, MidpointRounding.AwayFromZero);
            }
            catch (DivideByZeroException)
            {
                _value = 0;
            }

            return _value.Value;
        }
    }

    /// <summary>
    /// Returns the stat's value as an int
    /// </summary>
    /// <returns>The stat value as an int</returns>
    /// <exception cref="CalculatedStatCannotBeConvertedToIntException">Thrown if the decimal value does not fit in an int</exception>
    public int ToInt()
    {
        if (Value < int.MinValue || Value > int.MaxValue)
        {
            throw new CalculatedStatCannotBeConvertedToIntException(
                $"CalculatedStat value of {Value} does not fit in an int");
        }

        return (int)Value;
    }

    /// <summary>
    /// Returns the stat's value as a NaturalNumber
    /// </summary>
    /// <returns>The stat value as a NaturalNumber</returns>
    public NaturalNumber ToNaturalNumber()
    {
        return NaturalNumber.Create(ToInt());
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