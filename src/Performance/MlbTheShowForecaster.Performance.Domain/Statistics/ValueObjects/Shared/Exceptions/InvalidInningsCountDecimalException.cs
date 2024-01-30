namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared.Exceptions;

/// <summary>
/// Thrown when an invalid decimal value is provided to <see cref="InningsCount"/>
/// </summary>
public sealed class InvalidInningsCountDecimalException : Exception
{
    public InvalidInningsCountDecimalException(string? message) : base(message)
    {
    }
}