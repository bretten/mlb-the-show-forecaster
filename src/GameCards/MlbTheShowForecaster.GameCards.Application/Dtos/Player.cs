using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a MLB player
/// </summary>
/// <param name="MlbId">The MLB ID</param>
/// <param name="Name">The player's name</param>
/// <param name="Position">The player's position</param>
/// <param name="Team">The player's team</param>
public readonly record struct Player(MlbId MlbId, string Name, Position Position, TeamShortName Team);