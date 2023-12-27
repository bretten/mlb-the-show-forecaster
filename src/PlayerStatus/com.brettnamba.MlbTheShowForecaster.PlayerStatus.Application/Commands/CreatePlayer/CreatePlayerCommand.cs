using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.CreatePlayer;

/// <summary>
/// Command that creates a <see cref="Player"/>
/// </summary>
/// <param name="PlayerStatus">The current status of the player</param>
public readonly record struct CreatePlayerCommand(Dtos.PlayerStatus PlayerStatus) : ICommand;