using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Exceptions;

/// <summary>
/// Thrown when an unsupported value is provided to a <see cref="TeamAbbreviation"/>
/// </summary>
public sealed class InvalidTeamAbbreviationException : Exception
{
    public InvalidTeamAbbreviationException(string? message) : base(message)
    {
    }
}