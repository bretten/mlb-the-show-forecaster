namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="NaturalNumber"/> is provided with a number that exceeds its maximum value
/// </summary>
public sealed class NaturalNumberOverflowException : Exception
{
    public NaturalNumberOverflowException(string? message) : base(message)
    {
    }
}