using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.Exceptions;

/// <summary>
/// Thrown when an unsupported value is used for <see cref="MlbId"/>
/// </summary>
public sealed class InvalidMlbIdException : Exception
{
    public InvalidMlbIdException(string? message) : base(message)
    {
    }
}