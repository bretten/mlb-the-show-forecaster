namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="CalculatedStat"/> is provided a negative value
/// </summary>
public sealed class CalculatedStatBelowZeroException : Exception
{
    public CalculatedStatBelowZeroException(string? message) : base(message)
    {
    }
}