using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.DomainApis.PlayerStatusApi.Responses;

/// <summary>
/// Represents a player response
/// </summary>
/// <param name="MlbId">The MLB ID of the Player</param>
/// <param name="FirstName">Player's first name</param>
/// <param name="LastName">Player's last name</param>
/// <param name="Position">Player's primary position</param>
/// <param name="Team">The Player's Team</param>
public readonly record struct PlayerResponse(
    [property: JsonPropertyName("mlbId")]
    int MlbId,
    [property: JsonPropertyName("firstName")]
    string FirstName,
    [property: JsonPropertyName("lastName")]
    string LastName,
    [property: JsonPropertyName("position")]
    string Position,
    [property: JsonPropertyName("team")]
    string? Team);