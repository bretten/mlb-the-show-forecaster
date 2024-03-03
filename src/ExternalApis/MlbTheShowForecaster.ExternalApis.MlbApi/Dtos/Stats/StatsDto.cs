using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

[JsonConverter(typeof(StatJsonConverter))]
public record StatsDto(
    [property: JsonPropertyName("group")]
    StatsGroupDto Group,
    [property: JsonPropertyName("splits")]
    IEnumerable<GameStatSplitDto> Splits
);