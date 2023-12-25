using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Exceptions;

/// <summary>
/// Thrown when a <see cref="MlbId"/> is provided but does not correspond to a <see cref="Team"/>
/// </summary>
public sealed class UnknownTeamMlbIdException : Exception
{
    public UnknownTeamMlbIdException(string? message) : base(message)
    {
    }
}