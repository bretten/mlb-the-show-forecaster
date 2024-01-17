using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Exceptions;

/// <summary>
/// Thrown when <see cref="IPlayerRoster"/> can only find an empty roster
/// </summary>
public sealed class EmptyRosterException : Exception
{
    public EmptyRosterException(string? message) : base(message)
    {
    }
}