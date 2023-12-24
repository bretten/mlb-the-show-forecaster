using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Exceptions;

/// <summary>
/// Thrown when a <see cref="PersonName"/> is empty
/// </summary>
public sealed class EmptyPersonNameException : Exception
{
    public EmptyPersonNameException(string? message) : base(message)
    {
    }
}