namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="CalculatedStat"/> cannot be cast to an integer due to it being either too large or too small
/// </summary>
public class CalculatedStatCannotBeConvertedToIntException : Exception
{
    public CalculatedStatCannotBeConvertedToIntException(string? message) : base(message)
    {
    }
}