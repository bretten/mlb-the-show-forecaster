namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

/// <summary>
/// Thrown when an unsupported value is used for <see cref="MlbId"/>
/// </summary>
public sealed class InvalidMlbIdException : Exception
{
    public InvalidMlbIdException(string? message) : base(message)
    {
    }
}