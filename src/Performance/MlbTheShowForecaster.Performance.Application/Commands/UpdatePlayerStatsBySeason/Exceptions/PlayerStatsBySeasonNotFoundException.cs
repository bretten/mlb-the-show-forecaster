using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason.Exceptions;

/// <summary>
/// Thrown when <see cref="UpdatePlayerStatsBySeasonCommandHandler"/> cannot find the specified
/// <see cref="PlayerStatsBySeason"/> to update
/// </summary>
public sealed class PlayerStatsBySeasonNotFoundException : Exception
{
    public PlayerStatsBySeasonNotFoundException(string? message) : base(message)
    {
    }
}