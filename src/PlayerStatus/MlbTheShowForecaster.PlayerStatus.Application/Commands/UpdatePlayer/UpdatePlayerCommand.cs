using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer;

/// <summary>
/// Updates a <see cref="Player"/>
/// </summary>
/// <param name="Year">The year the player is being updated for</param>
/// <param name="Player">The <see cref="Player"/> to update</param>
/// <param name="PlayerStatusChanges">The status changes for the <see cref="Player"/></param>
/// <param name="Date">The date</param>
internal readonly record struct UpdatePlayerCommand(
    SeasonYear Year,
    Player Player,
    PlayerStatusChanges PlayerStatusChanges,
    DateOnly Date) : ICommand;