using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.CreatePlayerStatsBySeason;

/// <summary>
/// Command that creates a <see cref="PlayerStatsBySeason"/>
/// </summary>
/// <param name="PlayerSeason">The stats for a player's season</param>
internal readonly record struct CreatePlayerStatsBySeasonCommand(PlayerSeason PlayerSeason) : ICommand;