namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="NaturalNumber"/> is not provided 0 or a positive integer (1, 2, 3, ...)
/// </summary>
public sealed class NaturalNumberCannotBeLessThanZeroException : Exception
{
    public NaturalNumberCannotBeLessThanZeroException(string? message) : base(message)
    {
    }
}