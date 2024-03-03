using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters.Exceptions;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters;

public class StatJsonConverter : JsonConverter<StatsDto>
{
    public override StatsDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var statType = DetermineStatType(ref reader);


        ReadUntilProperty(ref reader, "splits");

        StatsDto statsDto = ParseStats(ref reader, statType);


        CloseReader(ref reader);

        return statsDto;
    }

    public override void Write(Utf8JsonWriter writer, StatsDto value, JsonSerializerOptions options)
    {
        throw new JsonConverterWriteNotImplementedException("Writing to JSON is not supported");
    }

    private StatType DetermineStatType(ref Utf8JsonReader reader)
    {
        ReadUntilProperty(ref reader, "group");
        reader.Read(); // Start object
        reader.Read(); // display name
        reader.Read();
        var groupType = reader.GetString();
        switch (groupType)
        {
            case "hitting":
                return StatType.Hitting;
            case "pitching":
                return StatType.Pitching;
            case "fielding":
                return StatType.Fielding;
            default:
                throw new Exception("Stat type not found");
        }
    }

    private StatsDto ParseStats(ref Utf8JsonReader reader, StatType statType)
    {
        switch (statType)
        {
            case StatType.Hitting:
                return new StatsDto(new StatsGroupDto("hitting"),
                    JsonSerializer.Deserialize<IEnumerable<GameStatSplitHittingDto>>(ref reader) ??
                    Array.Empty<GameStatSplitHittingDto>());
                break;
            case StatType.Pitching:
                return new StatsDto(new StatsGroupDto("pitching"),
                    JsonSerializer.Deserialize<IEnumerable<GameStatSplitPitchingDto>>(ref reader) ??
                    Array.Empty<GameStatSplitPitchingDto>());
                break;
            case StatType.Fielding:
                return new StatsDto(new StatsGroupDto("fielding"),
                    JsonSerializer.Deserialize<IEnumerable<GameStatSplitFieldingDto>>(ref reader) ??
                    Array.Empty<GameStatSplitFieldingDto>());
                break;
            default:
                throw new Exception("");
        }
    }

    private void ReadUntilProperty(ref Utf8JsonReader reader, string propertyName)
    {
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    if (reader.ValueTextEquals(Encoding.UTF8.GetBytes(propertyName)))
                    {
                        //reader.Skip();
                        return;
                    }

                    break;
                default:
                    reader.Skip();
                    break;
            }
        }
    }

    private void CloseReader(ref Utf8JsonReader reader)
    {
        while (reader.Read())
        {
            reader.Skip();
        }
    }

    private enum StatType
    {
        Hitting,
        Pitching,
        Fielding
    }
}