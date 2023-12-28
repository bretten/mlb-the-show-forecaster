﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer;

/// <summary>
/// Updates a <see cref="Player"/>
/// </summary>
/// <param name="Player">The <see cref="Player"/> to update</param>
/// <param name="PlayerStatusChanges">The status changes for the <see cref="Player"/></param>
internal readonly record struct UpdatePlayerCommand(Player Player, PlayerStatusChanges PlayerStatusChanges) : ICommand;