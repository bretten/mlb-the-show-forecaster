using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Api.Responses;

/// <summary>
/// Represents a player response
/// </summary>
/// <param name="MlbId">The MLB ID of the Player</param>
/// <param name="FirstName">Player's first name</param>
/// <param name="LastName">Player's last name</param>
/// <param name="Position">Player's primary position</param>
/// <param name="Team">The Player's Team</param>
public readonly record struct PlayerResponse(
    int MlbId,
    string FirstName,
    string LastName,
    string Position,
    string? Team)
{
    /// <summary>
    /// Creates a <see cref="PlayerResponse"/> from a <see cref="Player"/>
    /// </summary>
    /// <param name="player"><see cref="Player"/></param>
    /// <returns><see cref="PlayerResponse"/></returns>
    public static PlayerResponse From(Player player)
    {
        return new PlayerResponse(player.MlbId.Value, player.FirstName.Value, player.LastName.Value,
            player.Position.GetDisplayName(), player.Team?.Abbreviation.Value);
    }
};