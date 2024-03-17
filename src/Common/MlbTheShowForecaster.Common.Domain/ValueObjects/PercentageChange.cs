using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

/// <summary>
/// Represents the percentage change from a <see cref="ReferenceValue"/> to a <see cref="NewValue"/>
/// </summary>
public class PercentageChange : ValueObject
{
    /// <summary>
    /// The underlying percentage change
    /// </summary>
    private decimal? _percentageChange;

    /// <summary>
    /// The reference value that the percentage change will be calculated with respect to
    /// </summary>
    public decimal ReferenceValue { get; }

    /// <summary>
    /// The new value that determines the percentage change with respect to the <see cref="ReferenceValue"/>
    /// </summary>
    public decimal NewValue { get; }

    /// <summary>
    /// The number of fractional digits to round the decimal value to
    /// </summary>
    protected virtual int FractionalDigitCount => 2;

    /// <summary>
    /// The percentage change from the <see cref="ReferenceValue"/> to the <see cref="NewValue"/>
    /// </summary>
    public decimal PercentageChangeValue
    {
        get
        {
            if (!_percentageChange.HasValue)
            {
                _percentageChange = Math.Round(CalculatePercentageChange(), FractionalDigitCount,
                    MidpointRounding.AwayFromZero);
            }

            return _percentageChange.Value;
        }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="referenceValue">The reference value that the percentage change will be calculated with respect to</param>
    /// <param name="newValue">The new value that determines the percentage change with respect to the <see cref="ReferenceValue"/></param>
    protected PercentageChange(decimal referenceValue, decimal newValue)
    {
        ReferenceValue = referenceValue;
        NewValue = newValue;
    }

    /// <summary>
    /// Calculates the percentage change from the <see cref="ReferenceValue"/> to the <see cref="NewValue"/>
    /// </summary>
    private decimal CalculatePercentageChange()
    {
        return 100 * ((NewValue - ReferenceValue) / ReferenceValue);
    }

    /// <summary>
    /// Creates a <see cref="PercentageChange"/>
    /// </summary>
    /// <param name="referenceValue">The reference value that the percentage change will be calculated with respect to</param>
    /// <param name="newValue">The new value that determines the percentage change with respect to the <see cref="ReferenceValue"/></param>
    /// <returns><see cref="PercentageChange"/></returns>
    public static PercentageChange Create(decimal referenceValue, decimal newValue)
    {
        return new PercentageChange(referenceValue, newValue);
    }

    /// <summary>
    /// Creates a <see cref="PercentageChange"/>
    /// </summary>
    /// <param name="referenceValue">The reference value that the percentage change will be calculated with respect to</param>
    /// <param name="newValue">The new value that determines the percentage change with respect to the <see cref="ReferenceValue"/></param>
    /// <returns><see cref="PercentageChange"/></returns>
    public static PercentageChange Create(NaturalNumber referenceValue, NaturalNumber newValue)
    {
        return new PercentageChange(referenceValue.Value, newValue.Value);
    }
}