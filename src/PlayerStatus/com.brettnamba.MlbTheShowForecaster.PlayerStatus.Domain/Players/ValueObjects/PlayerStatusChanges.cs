using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;

/// <summary>
/// Represents status changes for a <see cref="Player"/>, who may have more than one change
/// at any given time
/// </summary>
/// <param name="Changes">All the status changes for the <see cref="Player"/></param>
/// <param name="NewTeam">The <see cref="Team"/> if the status change involves a new <see cref="Team"/></param>
public readonly record struct PlayerStatusChanges(List<PlayerStatusChangeType> Changes, Team? NewTeam);