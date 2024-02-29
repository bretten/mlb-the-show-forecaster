using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason;

/// <summary>
/// Command that updates a <see cref="PlayerStatsBySeason"/>
/// </summary>
/// <param name="PlayerStatsBySeason">The <see cref="PlayerStatsBySeason"/> to update</param>
/// <param name="PlayerSeason">The stats for a player's season</param>
internal readonly record struct UpdatePlayerStatsBySeasonCommand(
    PlayerStatsBySeason PlayerStatsBySeason,
    PlayerSeason PlayerSeason) : ICommand;