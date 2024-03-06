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
    public override StatsDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
        var isHitting = value.Group.DisplayName == Constants.Parameters.Hitting;
        var isPitching = value.Group.DisplayName == Constants.Parameters.Pitching;

        // Start the whole JSON object
        writer.WriteStartObject();

        // Write the group
        writer.WriteStartObject(PreEncodedText.General.Group);
        if (isHitting)
        {
            writer.WriteString(PreEncodedText.General.DisplayName, PreEncodedText.Hitting.DisplayName);
        }
        else if (isPitching)
        {
            writer.WriteString(PreEncodedText.General.DisplayName, PreEncodedText.Pitching.DisplayName);
        }
        else
        {
            writer.WriteString(PreEncodedText.General.DisplayName, PreEncodedText.Fielding.DisplayName);
        }

        // End the group
        writer.WriteEndObject();

        // Start the stats by game array
        writer.WriteStartArray(PreEncodedText.General.Splits);
        foreach (var split in value.Splits)
        {
            // Start the stats by game object
            writer.WriteStartObject();

            // Write the stats by game object properties
            writer.WriteString(PreEncodedText.General.Season, split.Season);
            writer.WriteString(PreEncodedText.General.Date, split.Date);
            writer.WriteString(PreEncodedText.General.GameType, split.GameType);
            writer.WriteBoolean(PreEncodedText.General.IsHome, split.IsHome);
            writer.WriteBoolean(PreEncodedText.General.IsWin, split.IsWin);

            // Team object
            writer.WriteStartObject(PreEncodedText.General.Team);
            writer.WriteNumber(PreEncodedText.General.Id, split.Team.Id);
            writer.WriteString(PreEncodedText.General.Name, split.Team.Name);
            writer.WriteEndObject();

            // Game object
            writer.WriteStartObject(PreEncodedText.General.Game);
            writer.WriteNumber(PreEncodedText.General.GamePk, split.Game.GamePk);
            writer.WriteEndObject();

            // Start the stat object
            writer.WriteStartObject(PreEncodedText.General.Stat);
            if (isHitting && split is GameHittingStatsDto gameHittingStatsDto)
            {
                WriteHitting(ref writer, gameHittingStatsDto);
            }
            else if (isPitching && split is GamePitchingStatsDto gamePitchingStatsDto)
            {
                WritePitching(ref writer, gamePitchingStatsDto);
            }
            else
            {
                WriteFielding(ref writer, (split as GameFieldingStatsDto)!);
            }

            // End the stat object
            writer.WriteEndObject();

            // End the stats by game object
            writer.WriteEndObject();
        }

        // End the stats by game array
        writer.WriteEndArray();

        // End the whole JSON object
        writer.WriteEndObject();
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
            Constants.Parameters.Hitting => StatGroup.Hitting,
            Constants.Parameters.Pitching => StatGroup.Pitching,
            Constants.Parameters.Fielding => StatGroup.Fielding,
            _ => throw new UnknownStatGroupException($"Unknown stat group: {groupName}")
        };
    }

    /// <summary>
    /// Parses the "splits" stats section
    /// </summary>
    /// <param name="reader">The reader that is parsing the JSON</param>
    /// <param name="statGroup">The type of stat being parsed</param>
    /// <returns>The stats by game</returns>
    private IEnumerable<GameStatsDto> ParseStatSplitsByGame(ref Utf8JsonReader reader, StatGroup statGroup)
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
    private IEnumerable<GameStatsDto> ParseStatsByGame(ref Utf8JsonReader reader, StatGroup statGroup)
    {
        return statGroup switch
        {
            StatGroup.Hitting => JsonSerializer.Deserialize<IEnumerable<GameHittingStatsDto>>(ref reader) ??
                                 Array.Empty<GameHittingStatsDto>(),
            StatGroup.Pitching => JsonSerializer.Deserialize<IEnumerable<GamePitchingStatsDto>>(ref reader) ??
                                  Array.Empty<GamePitchingStatsDto>(),
            _ => JsonSerializer.Deserialize<IEnumerable<GameFieldingStatsDto>>(ref reader) ??
                 Array.Empty<GameFieldingStatsDto>(),
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
    /// Writes a hitting stats game
    /// </summary>
    /// <param name="writer">The writer that is serializing the JSON</param>
    /// <param name="game">The game stats</param>
    private void WriteHitting(ref Utf8JsonWriter writer, GameHittingStatsDto game)
    {
        writer.WriteString(PreEncodedText.Hitting.Summary, game.Stat.Summary);
        writer.WriteNumber(PreEncodedText.Hitting.GamesPlayed, game.Stat.GamesPlayed);
        writer.WriteNumber(PreEncodedText.Hitting.GroundOuts, game.Stat.GroundOuts);
        writer.WriteNumber(PreEncodedText.Hitting.AirOuts, game.Stat.AirOuts);
        writer.WriteNumber(PreEncodedText.Hitting.Runs, game.Stat.Runs);
        writer.WriteNumber(PreEncodedText.Hitting.Doubles, game.Stat.Doubles);
        writer.WriteNumber(PreEncodedText.Hitting.Triples, game.Stat.Triples);
        writer.WriteNumber(PreEncodedText.Hitting.HomeRuns, game.Stat.HomeRuns);
        writer.WriteNumber(PreEncodedText.Hitting.StrikeOuts, game.Stat.StrikeOuts);
        writer.WriteNumber(PreEncodedText.Hitting.BaseOnBalls, game.Stat.BaseOnBalls);
        writer.WriteNumber(PreEncodedText.Hitting.IntentionalWalks, game.Stat.IntentionalWalks);
        writer.WriteNumber(PreEncodedText.Hitting.Hits, game.Stat.Hits);
        writer.WriteNumber(PreEncodedText.Hitting.HitByPitch, game.Stat.HitByPitch);
        writer.WriteString(PreEncodedText.Hitting.Avg, game.Stat.Avg);
        writer.WriteNumber(PreEncodedText.Hitting.AtBats, game.Stat.AtBats);
        writer.WriteString(PreEncodedText.Hitting.Obp, game.Stat.Obp);
        writer.WriteString(PreEncodedText.Hitting.Slg, game.Stat.Slg);
        writer.WriteString(PreEncodedText.Hitting.Ops, game.Stat.Ops);
        writer.WriteNumber(PreEncodedText.Hitting.CaughtStealing, game.Stat.CaughtStealing);
        writer.WriteNumber(PreEncodedText.Hitting.StolenBases, game.Stat.StolenBases);
        writer.WriteString(PreEncodedText.Hitting.StolenBasePercentage, game.Stat.StolenBasePercentage);
        writer.WriteNumber(PreEncodedText.Hitting.GroundIntoDoublePlay, game.Stat.GroundIntoDoublePlay);
        writer.WriteNumber(PreEncodedText.Hitting.GroundIntoTriplePlay, game.Stat.GroundIntoTriplePlay);
        writer.WriteNumber(PreEncodedText.Hitting.NumberOfPitches, game.Stat.NumberOfPitches);
        writer.WriteNumber(PreEncodedText.Hitting.PlateAppearances, game.Stat.PlateAppearances);
        writer.WriteNumber(PreEncodedText.Hitting.TotalBases, game.Stat.TotalBases);
        writer.WriteNumber(PreEncodedText.Hitting.Rbi, game.Stat.Rbi);
        writer.WriteNumber(PreEncodedText.Hitting.LeftOnBase, game.Stat.LeftOnBase);
        writer.WriteNumber(PreEncodedText.Hitting.SacBunts, game.Stat.SacBunts);
        writer.WriteNumber(PreEncodedText.Hitting.SacFlies, game.Stat.SacFlies);
        writer.WriteString(PreEncodedText.Hitting.Babip, game.Stat.Babip);
        writer.WriteString(PreEncodedText.Hitting.GroundOutsToAirouts, game.Stat.GroundOutsToAirOuts);
        writer.WriteNumber(PreEncodedText.Hitting.CatchersInterference, game.Stat.CatchersInterference);
        writer.WriteString(PreEncodedText.Hitting.AtBatsPerHomeRun, game.Stat.AtBatsPerHomeRun);
    }

    /// <summary>
    /// Writes a pitching stats game
    /// </summary>
    /// <param name="writer">The writer that is serializing the JSON</param>
    /// <param name="game">The game stats</param>
    private void WritePitching(ref Utf8JsonWriter writer, GamePitchingStatsDto game)
    {
        writer.WriteString(PreEncodedText.Pitching.Summary, game.Stat.Summary);
        writer.WriteNumber(PreEncodedText.Pitching.GamesPlayed, game.Stat.GamesPlayed);
        writer.WriteNumber(PreEncodedText.Pitching.GamesStarted, game.Stat.GamesStarted);
        writer.WriteNumber(PreEncodedText.Pitching.GroundOuts, game.Stat.GroundOuts);
        writer.WriteNumber(PreEncodedText.Pitching.AirOuts, game.Stat.AirOuts);
        writer.WriteNumber(PreEncodedText.Pitching.Runs, game.Stat.Runs);
        writer.WriteNumber(PreEncodedText.Pitching.Doubles, game.Stat.Doubles);
        writer.WriteNumber(PreEncodedText.Pitching.Triples, game.Stat.Triples);
        writer.WriteNumber(PreEncodedText.Pitching.HomeRuns, game.Stat.HomeRuns);
        writer.WriteNumber(PreEncodedText.Pitching.StrikeOuts, game.Stat.StrikeOuts);
        writer.WriteNumber(PreEncodedText.Pitching.BaseOnBalls, game.Stat.BaseOnBalls);
        writer.WriteNumber(PreEncodedText.Pitching.IntentionalWalks, game.Stat.IntentionalWalks);
        writer.WriteNumber(PreEncodedText.Pitching.Hits, game.Stat.Hits);
        writer.WriteNumber(PreEncodedText.Pitching.HitByPitch, game.Stat.HitByPitch);
        writer.WriteString(PreEncodedText.Pitching.Avg, game.Stat.Avg);
        writer.WriteNumber(PreEncodedText.Pitching.AtBats, game.Stat.AtBats);
        writer.WriteString(PreEncodedText.Pitching.Obp, game.Stat.Obp);
        writer.WriteString(PreEncodedText.Pitching.Slg, game.Stat.Slg);
        writer.WriteString(PreEncodedText.Pitching.Ops, game.Stat.Ops);
        writer.WriteNumber(PreEncodedText.Pitching.CaughtStealing, game.Stat.CaughtStealing);
        writer.WriteNumber(PreEncodedText.Pitching.StolenBases, game.Stat.StolenBases);
        writer.WriteString(PreEncodedText.Pitching.StolenBasePercentage, game.Stat.StolenBasePercentage);
        writer.WriteNumber(PreEncodedText.Pitching.GroundIntoDoublePlay, game.Stat.GroundIntoDoublePlay);
        writer.WriteNumber(PreEncodedText.Pitching.NumberOfPitches, game.Stat.NumberOfPitches);
        writer.WriteString(PreEncodedText.Pitching.Era, game.Stat.Era);
        writer.WriteString(PreEncodedText.Pitching.InningsPitched, game.Stat.InningsPitched);
        writer.WriteNumber(PreEncodedText.Pitching.Wins, game.Stat.Wins);
        writer.WriteNumber(PreEncodedText.Pitching.Losses, game.Stat.Losses);
        writer.WriteNumber(PreEncodedText.Pitching.Saves, game.Stat.Saves);
        writer.WriteNumber(PreEncodedText.Pitching.SaveOpportunities, game.Stat.SaveOpportunities);
        writer.WriteNumber(PreEncodedText.Pitching.Holds, game.Stat.Holds);
        writer.WriteNumber(PreEncodedText.Pitching.BlownSaves, game.Stat.BlownSaves);
        writer.WriteNumber(PreEncodedText.Pitching.EarnedRuns, game.Stat.EarnedRuns);
        writer.WriteString(PreEncodedText.Pitching.Whip, game.Stat.Whip);
        writer.WriteNumber(PreEncodedText.Pitching.BattersFaced, game.Stat.BattersFaced);
        writer.WriteNumber(PreEncodedText.Pitching.Outs, game.Stat.Outs);
        writer.WriteNumber(PreEncodedText.Pitching.GamesPitched, game.Stat.GamesPitched);
        writer.WriteNumber(PreEncodedText.Pitching.CompleteGames, game.Stat.CompleteGames);
        writer.WriteNumber(PreEncodedText.Pitching.Shutouts, game.Stat.Shutouts);
        writer.WriteNumber(PreEncodedText.Pitching.Strikes, game.Stat.Strikes);
        writer.WriteString(PreEncodedText.Pitching.StrikePercentage, game.Stat.StrikePercentage);
        writer.WriteNumber(PreEncodedText.Pitching.HitBatsmen, game.Stat.HitBatsmen);
        writer.WriteNumber(PreEncodedText.Pitching.Balks, game.Stat.Balks);
        writer.WriteNumber(PreEncodedText.Pitching.WildPitches, game.Stat.WildPitches);
        writer.WriteNumber(PreEncodedText.Pitching.Pickoffs, game.Stat.Pickoffs);
        writer.WriteNumber(PreEncodedText.Pitching.TotalBases, game.Stat.TotalBases);
        writer.WriteString(PreEncodedText.Pitching.GroundOutsToAirouts, game.Stat.GroundOutsToAirOuts);
        writer.WriteString(PreEncodedText.Pitching.WinPercentage, game.Stat.WinPercentage);
        writer.WriteString(PreEncodedText.Pitching.PitchesPerInning, game.Stat.PitchesPerInning);
        writer.WriteNumber(PreEncodedText.Pitching.GamesFinished, game.Stat.GamesFinished);
        writer.WriteString(PreEncodedText.Pitching.StrikeoutWalkRatio, game.Stat.StrikeoutWalkRatio);
        writer.WriteString(PreEncodedText.Pitching.StrikeoutsPer9Inn, game.Stat.StrikeoutsPer9Inn);
        writer.WriteString(PreEncodedText.Pitching.WalksPer9Inn, game.Stat.WalksPer9Inn);
        writer.WriteString(PreEncodedText.Pitching.HitsPer9Inn, game.Stat.HitsPer9Inn);
        writer.WriteString(PreEncodedText.Pitching.RunsScoredPer9, game.Stat.RunsScoredPer9);
        writer.WriteString(PreEncodedText.Pitching.HomeRunsPer9, game.Stat.HomeRunsPer9);
        writer.WriteNumber(PreEncodedText.Pitching.InheritedRunners, game.Stat.InheritedRunners);
        writer.WriteNumber(PreEncodedText.Pitching.InheritedRunnersScored, game.Stat.InheritedRunnersScored);
        writer.WriteNumber(PreEncodedText.Pitching.CatchersInterference, game.Stat.CatchersInterference);
        writer.WriteNumber(PreEncodedText.Pitching.SacBunts, game.Stat.SacBunts);
        writer.WriteNumber(PreEncodedText.Pitching.SacFlies, game.Stat.SacFlies);
    }

    /// <summary>
    /// Writes a fielding stats game
    /// </summary>
    /// <param name="writer">The writer that is serializing the JSON</param>
    /// <param name="game">The game stats</param>
    private void WriteFielding(ref Utf8JsonWriter writer, GameFieldingStatsDto game)
    {
        writer.WriteNumber(PreEncodedText.Fielding.GamesPlayed, game.Stat.GamesPlayed);
        writer.WriteNumber(PreEncodedText.Fielding.GamesStarted, game.Stat.GamesStarted);
        writer.WriteNumber(PreEncodedText.Fielding.CaughtStealing, game.Stat.CaughtStealing);
        writer.WriteNumber(PreEncodedText.Fielding.StolenBases, game.Stat.StolenBases);
        writer.WriteString(PreEncodedText.Fielding.StolenBasePercentage, game.Stat.StolenBasePercentage);
        writer.WriteNumber(PreEncodedText.Fielding.Assists, game.Stat.Assists);
        writer.WriteNumber(PreEncodedText.Fielding.PutOuts, game.Stat.PutOuts);
        writer.WriteNumber(PreEncodedText.Fielding.Errors, game.Stat.Errors);
        writer.WriteNumber(PreEncodedText.Fielding.Chances, game.Stat.Chances);
        writer.WriteString(PreEncodedText.Fielding.FieldingPercentage, game.Stat.Fielding);

        // Player's fielding position
        writer.WriteStartObject(PreEncodedText.Fielding.Position);
        writer.WriteString(PreEncodedText.General.Name, game.Stat.Position.Name);
        writer.WriteString(PreEncodedText.General.Abbreviation, game.Stat.Position.Abbreviation);
        writer.WriteEndObject();

        writer.WriteString(PreEncodedText.Fielding.RangeFactorPerGame, game.Stat.RangeFactorPerGame);
        writer.WriteString(PreEncodedText.Fielding.RangeFactorPer9Inn, game.Stat.RangeFactorPer9Inn);
        writer.WriteString(PreEncodedText.Fielding.Innings, game.Stat.Innings);
        writer.WriteNumber(PreEncodedText.Fielding.Games, game.Stat.Games);
        writer.WriteNumber(PreEncodedText.Fielding.PassedBall, game.Stat.PassedBall);
        writer.WriteNumber(PreEncodedText.Fielding.DoublePlays, game.Stat.DoublePlays);
        writer.WriteNumber(PreEncodedText.Fielding.TriplePlays, game.Stat.TriplePlays);
        writer.WriteString(PreEncodedText.Fielding.CatcherEra, game.Stat.CatcherEra);
        writer.WriteNumber(PreEncodedText.Fielding.CatchersInterference, game.Stat.CatchersInterference);
        writer.WriteNumber(PreEncodedText.Fielding.WildPitches, game.Stat.WildPitches);
        writer.WriteNumber(PreEncodedText.Fielding.ThrowingErrors, game.Stat.ThrowingErrors);
        writer.WriteNumber(PreEncodedText.Fielding.Pickoffs, game.Stat.Pickoffs);
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