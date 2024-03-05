using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Games;

/// <summary>
/// Models a MLB game
/// </summary>
/// <param name="GamePk">The ID of the game</param>
public readonly record struct GameDto(
    [property: JsonPropertyName("gamePk")]
    int GamePk
);