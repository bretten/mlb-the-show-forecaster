using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters.Exceptions;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters;

/// <summary>
/// <see cref="JsonConverter"/> used to parse <see cref="StatsDto"/>
/// </summary>
public class StatJsonConverter : JsonConverter<StatsDto>
{
    /// <summary>
    /// Makes sure the specified type can be converted by this converter
    /// </summary>
    /// <param name="typeToConvert">The type to check</param>
    /// <returns>True if it can be converted, otherwise false</returns>
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(StatsDto);

    /// <summary>
    /// Uses the reader to parse the current JSON scope as a <see cref="StatsDto"/>
    /// </summary>
    /// <param name="reader">The reader that is parsing the JSON</param>
    /// <param name="typeToConvert">The type being converted</param>
    /// <param name="options">The options for the serializer</param>
    /// <returns>The parsed object</returns>
    public override StatsDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse the stat group
        var statGroup = ParseStatGroup(ref reader);

        // Parse the stats
        var stats = ParseStatSplitsByGame(ref reader, statGroup);

        return new StatsDto(new StatsGroupDto(statGroup.ToString().ToLower()), stats);
    }

    /// <summary>
    /// <see cref="StatsDto"/> is a one-way serialization, so writing is not supported
    /// </summary>
    /// <param name="writer">The writer to write to</param>
    /// <param name="value">The value being converted</param>
    /// <param name="options">The options for the serializer </param>
    /// <exception cref="JsonException">Thrown since it is a one-way serialization</exception>
    public override void Write(Utf8JsonWriter writer, StatsDto value, JsonSerializerOptions options)
    {
        throw new JsonException();
    }

    /// <summary>
    /// Parses the stat group
    /// </summary>
    /// <param name="reader">The reader that is parsing the JSON</param>
    /// <returns>The type of stat being parsed</returns>
    /// <exception cref="UnknownStatGroupException">Thrown when an unknown stat is encountered</exception>
    private StatGroup ParseStatGroup(ref Utf8JsonReader reader)
    {
        // Read until group property is found which shows what kind of stats
        ReadUntilProperty(ref reader, "group");
        reader.Read(); // Read StartObject
        reader.Read(); // Read PropertyName = "displayName"
        reader.Read(); // Read the value of the "displayName" property, which is the stat group
        var groupName = reader.GetString();
        return groupName switch
        {
            "hitting" => StatGroup.Hitting,
            "pitching" => StatGroup.Pitching,
            "fielding" => StatGroup.Fielding,
            _ => throw new UnknownStatGroupException($"Unknown stat group: {groupName}")
        };
    }

    /// <summary>
    /// Parses the "splits" stats section
    /// </summary>
    /// <param name="reader">The reader that is parsing the JSON</param>
    /// <param name="statGroup">The type of stat being parsed</param>
    /// <returns>The stats by game</returns>
    private IEnumerable<GameStatSplitDto> ParseStatSplitsByGame(ref Utf8JsonReader reader, StatGroup statGroup)
    {
        // Read until the "splits" property
        ReadUntilProperty(ref reader, "splits");

        // Parse the stat splits by game
        var stats = ParseStatsByGame(ref reader, statGroup);

        reader.Read(); // Read EndArray

        // Leave the reader on EndObject so that we can stop the reader

        return stats;
    }

    /// <summary>
    /// Parses stats by games using the reader
    /// </summary>
    /// <param name="reader">The reader that is parsing the JSON</param>
    /// <param name="statGroup">The type of stat to be parsed</param>
    /// <returns>The stats parsed by the reader</returns>
    private IEnumerable<GameStatSplitDto> ParseStatsByGame(ref Utf8JsonReader reader, StatGroup statGroup)
    {
        return statGroup switch
        {
            StatGroup.Hitting => JsonSerializer.Deserialize<IEnumerable<GameStatSplitHittingDto>>(ref reader) ??
                                 Array.Empty<GameStatSplitHittingDto>(),
            StatGroup.Pitching => JsonSerializer.Deserialize<IEnumerable<GameStatSplitPitchingDto>>(ref reader) ??
                                  Array.Empty<GameStatSplitPitchingDto>(),
            _ => JsonSerializer.Deserialize<IEnumerable<GameStatSplitFieldingDto>>(ref reader) ??
                 Array.Empty<GameStatSplitFieldingDto>(),
        };
    }

    /// <summary>
    /// Advances the reader until the specified JSON property name is encountered
    /// </summary>
    /// <param name="reader">The reader to advance</param>
    /// <param name="propertyName">The property name to look for</param>
    private void ReadUntilProperty(ref Utf8JsonReader reader, string propertyName)
    {
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    if (reader.ValueTextEquals(Encoding.UTF8.GetBytes(propertyName)))
                    {
                        return;
                    }

                    break;
                default:
                    reader.TrySkip();
                    break;
            }
        }
    }

    /// <summary>
    /// The different types of stat groups that can be parsed
    /// </summary>
    private enum StatGroup
    {
        Hitting,
        Pitching,
        Fielding
    }
}