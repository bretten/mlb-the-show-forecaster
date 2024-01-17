using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Exceptions;

/// <summary>
/// Thrown when a <see cref="MlbId"/> is provided but does not correspond to a <see cref="Team"/>
/// </summary>
public sealed class UnknownTeamMlbIdException : Exception
{
    public UnknownTeamMlbIdException(string? message) : base(message)
    {
    }
}