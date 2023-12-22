using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Exceptions;

/// <summary>
/// Thrown when an unsupported value is provided to a <see cref="Team"/> abbreviation
/// </summary>
public sealed class InvalidTeamAbbreviationException : Exception
{
    public InvalidTeamAbbreviationException(string? message) : base(message)
    {
    }
}