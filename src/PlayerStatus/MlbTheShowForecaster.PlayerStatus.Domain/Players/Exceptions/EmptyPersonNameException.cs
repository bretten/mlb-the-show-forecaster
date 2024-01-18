using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Exceptions;

/// <summary>
/// Thrown when a <see cref="PersonName"/> is empty
/// </summary>
public sealed class EmptyPersonNameException : Exception
{
    public EmptyPersonNameException(string? message) : base(message)
    {
    }
}