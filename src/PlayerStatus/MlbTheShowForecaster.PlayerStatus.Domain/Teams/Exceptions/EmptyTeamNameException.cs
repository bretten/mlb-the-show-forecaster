using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Exceptions;

/// <summary>
/// Thrown when <see cref="TeamName"/> is given an empty name
/// </summary>
public sealed class EmptyTeamNameException : Exception
{
    public EmptyTeamNameException(string? message) : base(message)
    {
    }
}