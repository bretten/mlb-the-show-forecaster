namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Contracts;

/// <summary>
/// Represents a stat
/// </summary>
public interface IStat
{
    /// <summary>
    /// The value of the stat
    /// </summary>
    decimal Value { get; }
}