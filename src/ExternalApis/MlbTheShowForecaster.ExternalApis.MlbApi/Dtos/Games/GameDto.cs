using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Games;

public record GameDto(
    [property: JsonPropertyName("gamePk")]
    int GamePk
);