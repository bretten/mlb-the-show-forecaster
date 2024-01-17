using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.CreatePlayer;

/// <summary>
/// Command that creates a <see cref="Player"/>
/// </summary>
/// <param name="RosterEntry">The current information on the player from the MLB</param>
internal readonly record struct CreatePlayerCommand(RosterEntry RosterEntry) : ICommand;