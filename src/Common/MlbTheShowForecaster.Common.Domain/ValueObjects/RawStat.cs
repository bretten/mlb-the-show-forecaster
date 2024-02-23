namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

/// <summary>
/// A raw stat with no calculation
/// </summary>
public sealed class RawStat : CalculatedStat
{
    /// <summary>
    /// The underlying raw value
    /// </summary>
    private readonly decimal _rawValue;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rawValue">The underlying raw value</param>
    private RawStat(decimal rawValue)
    {
        _rawValue = rawValue;
    }

    /// <summary>
    /// Returns the raw stat
    /// </summary>
    /// <returns>The raw stat</returns>
    protected override decimal Calculate()
    {
        return _rawValue;
    }

    /// <summary>
    /// Creates <see cref="RawStat"/>
    /// </summary>
    /// <param name="rawValue">The raw stat</param>
    /// <returns><see cref="RawStat"/></returns>
    public static RawStat Create(decimal rawValue)
    {
        return new RawStat(rawValue);
    }
}