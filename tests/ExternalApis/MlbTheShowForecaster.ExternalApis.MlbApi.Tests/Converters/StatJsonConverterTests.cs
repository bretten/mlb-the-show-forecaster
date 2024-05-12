using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters.Exceptions;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Tests.TestFiles;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Tests.Converters;

public class StatJsonConverterTests
{
    [Fact]
    public void Read_BattingStatsJson_ParsesToBattingStatsDto()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.BattingStatsJson);

        // Act
        var actual = JsonSerializer.Deserialize<StatsDto>(json);

        // Assert
        Assert.Equal("hitting", actual.Group.DisplayName);

        var actualGame1 = actual.Splits.ElementAt(0) as GameHittingStatsDto;
        Assert.NotNull(actualGame1);
        Assert.Equal("2023", actualGame1.Season);
        Assert.Equal(new DateOnly(2023, 3, 30), actualGame1.Date);
        Assert.Equal("R", actualGame1.GameType);
        Assert.False(actualGame1.IsHome);
        Assert.False(actualGame1.IsWin);
        Assert.Equal(108, actualGame1.Team.Id);
        Assert.Equal("Los Angeles Angels", actualGame1.Team.Name);
        Assert.Equal(718769, actualGame1.Game.GamePk);

        Assert.Equal("1-3 | BB, 2 K", actualGame1.Stat.Summary);
        Assert.Equal(1, actualGame1.Stat.GamesPlayed);
        Assert.Equal(2, actualGame1.Stat.GroundOuts);
        Assert.Equal(3, actualGame1.Stat.AirOuts);
        Assert.Equal(4, actualGame1.Stat.Runs);
        Assert.Equal(5, actualGame1.Stat.Doubles);
        Assert.Equal(6, actualGame1.Stat.Triples);
        Assert.Equal(7, actualGame1.Stat.HomeRuns);
        Assert.Equal(8, actualGame1.Stat.StrikeOuts);
        Assert.Equal(9, actualGame1.Stat.BaseOnBalls);
        Assert.Equal(10, actualGame1.Stat.IntentionalWalks);
        Assert.Equal(11, actualGame1.Stat.Hits);
        Assert.Equal(12, actualGame1.Stat.HitByPitch);
        Assert.Equal(".111", actualGame1.Stat.Avg);
        Assert.Equal(13, actualGame1.Stat.AtBats);
        Assert.Equal(".222", actualGame1.Stat.Obp);
        Assert.Equal(".333", actualGame1.Stat.Slg);
        Assert.Equal(".444", actualGame1.Stat.Ops);
        Assert.Equal(14, actualGame1.Stat.CaughtStealing);
        Assert.Equal(15, actualGame1.Stat.StolenBases);
        Assert.Equal(".555", actualGame1.Stat.StolenBasePercentage);
        Assert.Equal(16, actualGame1.Stat.GroundIntoDoublePlay);
        Assert.Equal(17, actualGame1.Stat.GroundIntoTriplePlay);
        Assert.Equal(18, actualGame1.Stat.NumberOfPitches);
        Assert.Equal(19, actualGame1.Stat.PlateAppearances);
        Assert.Equal(20, actualGame1.Stat.TotalBases);
        Assert.Equal(21, actualGame1.Stat.Rbi);
        Assert.Equal(22, actualGame1.Stat.LeftOnBase);
        Assert.Equal(23, actualGame1.Stat.SacBunts);
        Assert.Equal(24, actualGame1.Stat.SacFlies);
        Assert.Equal(".666", actualGame1.Stat.Babip);
        Assert.Equal(".777", actualGame1.Stat.GroundOutsToAirOuts);
        Assert.Equal(25, actualGame1.Stat.CatcherInterferences);
        Assert.Equal(".888", actualGame1.Stat.AtBatsPerHomeRun);

        var actualGame2 = actual.Splits.ElementAt(1) as GameHittingStatsDto;
        Assert.NotNull(actualGame2);
        Assert.Equal("2023", actualGame2.Season);
        Assert.Equal(new DateOnly(2023, 4, 1), actualGame2.Date);
        Assert.Equal("R", actualGame2.GameType);
        Assert.True(actualGame2.IsHome);
        Assert.True(actualGame2.IsWin);
        Assert.Equal(108, actualGame2.Team.Id);
        Assert.Equal("Los Angeles Angels", actualGame2.Team.Name);
        Assert.Equal(718757, actualGame2.Game.GamePk);

        Assert.Equal("2-5 | K, 2 RBI, R", actualGame2.Stat.Summary);
        Assert.Equal(26, actualGame2.Stat.GamesPlayed);
        Assert.Equal(27, actualGame2.Stat.GroundOuts);
        Assert.Equal(28, actualGame2.Stat.AirOuts);
        Assert.Equal(29, actualGame2.Stat.Runs);
        Assert.Equal(30, actualGame2.Stat.Doubles);
        Assert.Equal(31, actualGame2.Stat.Triples);
        Assert.Equal(32, actualGame2.Stat.HomeRuns);
        Assert.Equal(33, actualGame2.Stat.StrikeOuts);
        Assert.Equal(34, actualGame2.Stat.BaseOnBalls);
        Assert.Equal(35, actualGame2.Stat.IntentionalWalks);
        Assert.Equal(36, actualGame2.Stat.Hits);
        Assert.Equal(37, actualGame2.Stat.HitByPitch);
        Assert.Equal(".999", actualGame2.Stat.Avg);
        Assert.Equal(38, actualGame2.Stat.AtBats);
        Assert.Equal("1.111", actualGame2.Stat.Obp);
        Assert.Equal("1.222", actualGame2.Stat.Slg);
        Assert.Equal("1.333", actualGame2.Stat.Ops);
        Assert.Equal(39, actualGame2.Stat.CaughtStealing);
        Assert.Equal(40, actualGame2.Stat.StolenBases);
        Assert.Equal("1.444", actualGame2.Stat.StolenBasePercentage);
        Assert.Equal(41, actualGame2.Stat.GroundIntoDoublePlay);
        Assert.Equal(42, actualGame2.Stat.GroundIntoTriplePlay);
        Assert.Equal(43, actualGame2.Stat.NumberOfPitches);
        Assert.Equal(44, actualGame2.Stat.PlateAppearances);
        Assert.Equal(45, actualGame2.Stat.TotalBases);
        Assert.Equal(46, actualGame2.Stat.Rbi);
        Assert.Equal(47, actualGame2.Stat.LeftOnBase);
        Assert.Equal(48, actualGame2.Stat.SacBunts);
        Assert.Equal(49, actualGame2.Stat.SacFlies);
        Assert.Equal("1.555", actualGame2.Stat.Babip);
        Assert.Equal("1.666", actualGame2.Stat.GroundOutsToAirOuts);
        Assert.Equal(50, actualGame2.Stat.CatcherInterferences);
        Assert.Equal("1.777", actualGame2.Stat.AtBatsPerHomeRun);
    }

    [Fact]
    public void Read_PitchingStatsJson_ParsesToPitchingStatsDto()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.PitchingStatsJson);

        // Act
        var actual = JsonSerializer.Deserialize<StatsDto>(json);

        // Assert
        Assert.Equal("pitching", actual.Group.DisplayName);

        var actualGame1 = actual.Splits.ElementAt(0) as GamePitchingStatsDto;
        Assert.NotNull(actualGame1);
        Assert.Equal("2023", actualGame1.Season);
        Assert.Equal(new DateOnly(2023, 3, 30), actualGame1.Date);
        Assert.Equal("R", actualGame1.GameType);
        Assert.False(actualGame1.IsHome);
        Assert.False(actualGame1.IsWin);
        Assert.Equal(108, actualGame1.Team.Id);
        Assert.Equal("Los Angeles Angels", actualGame1.Team.Name);
        Assert.Equal(718769, actualGame1.Game.GamePk);

        Assert.Equal("6.0 IP, 0 ER, 10 K, 3 BB", actualGame1.Stat.Summary);
        Assert.Equal(1, actualGame1.Stat.GamesPlayed);
        Assert.Equal(2, actualGame1.Stat.GamesStarted);
        Assert.Equal(3, actualGame1.Stat.GroundOuts);
        Assert.Equal(4, actualGame1.Stat.AirOuts);
        Assert.Equal(5, actualGame1.Stat.Runs);
        Assert.Equal(6, actualGame1.Stat.Doubles);
        Assert.Equal(7, actualGame1.Stat.Triples);
        Assert.Equal(8, actualGame1.Stat.HomeRuns);
        Assert.Equal(9, actualGame1.Stat.StrikeOuts);
        Assert.Equal(10, actualGame1.Stat.BaseOnBalls);
        Assert.Equal(11, actualGame1.Stat.IntentionalWalks);
        Assert.Equal(12, actualGame1.Stat.Hits);
        Assert.Equal(13, actualGame1.Stat.HitByPitch);
        Assert.Equal(".111", actualGame1.Stat.Avg);
        Assert.Equal(14, actualGame1.Stat.AtBats);
        Assert.Equal(".222", actualGame1.Stat.Obp);
        Assert.Equal(".333", actualGame1.Stat.Slg);
        Assert.Equal(".444", actualGame1.Stat.Ops);
        Assert.Equal(15, actualGame1.Stat.CaughtStealing);
        Assert.Equal(16, actualGame1.Stat.StolenBases);
        Assert.Equal(".555", actualGame1.Stat.StolenBasePercentage);
        Assert.Equal(17, actualGame1.Stat.GroundIntoDoublePlay);
        Assert.Equal(18, actualGame1.Stat.NumberOfPitches);
        Assert.Equal(".666", actualGame1.Stat.Era);
        Assert.Equal("777.0", actualGame1.Stat.InningsPitched);
        Assert.Equal(19, actualGame1.Stat.Wins);
        Assert.Equal(20, actualGame1.Stat.Losses);
        Assert.Equal(21, actualGame1.Stat.Saves);
        Assert.Equal(22, actualGame1.Stat.SaveOpportunities);
        Assert.Equal(23, actualGame1.Stat.Holds);
        Assert.Equal(24, actualGame1.Stat.BlownSaves);
        Assert.Equal(25, actualGame1.Stat.EarnedRuns);
        Assert.Equal(".888", actualGame1.Stat.Whip);
        Assert.Equal(26, actualGame1.Stat.BattersFaced);
        Assert.Equal(27, actualGame1.Stat.Outs);
        Assert.Equal(28, actualGame1.Stat.GamesPitched);
        Assert.Equal(29, actualGame1.Stat.CompleteGames);
        Assert.Equal(30, actualGame1.Stat.Shutouts);
        Assert.Equal(31, actualGame1.Stat.Strikes);
        Assert.Equal(".999", actualGame1.Stat.StrikePercentage);
        Assert.Equal(32, actualGame1.Stat.HitBatsmen);
        Assert.Equal(33, actualGame1.Stat.Balks);
        Assert.Equal(34, actualGame1.Stat.WildPitches);
        Assert.Equal(35, actualGame1.Stat.Pickoffs);
        Assert.Equal(36, actualGame1.Stat.TotalBases);
        Assert.Equal("1.111", actualGame1.Stat.GroundOutsToAirOuts);
        Assert.Equal("1.222", actualGame1.Stat.WinPercentage);
        Assert.Equal("1.333", actualGame1.Stat.PitchesPerInning);
        Assert.Equal(37, actualGame1.Stat.GamesFinished);
        Assert.Equal("1.444", actualGame1.Stat.StrikeoutWalkRatio);
        Assert.Equal("1.555", actualGame1.Stat.StrikeoutsPer9Inn);
        Assert.Equal("1.666", actualGame1.Stat.WalksPer9Inn);
        Assert.Equal("1.777", actualGame1.Stat.HitsPer9Inn);
        Assert.Equal("1.888", actualGame1.Stat.RunsScoredPer9);
        Assert.Equal("1.999", actualGame1.Stat.HomeRunsPer9);
        Assert.Equal(38, actualGame1.Stat.InheritedRunners);
        Assert.Equal(39, actualGame1.Stat.InheritedRunnersScored);
        Assert.Equal(40, actualGame1.Stat.CatcherInterferences);
        Assert.Equal(41, actualGame1.Stat.SacBunts);
        Assert.Equal(42, actualGame1.Stat.SacFlies);

        var actualGame2 = actual.Splits.ElementAt(1) as GamePitchingStatsDto;
        Assert.NotNull(actualGame2);
        Assert.Equal("2023", actualGame2.Season);
        Assert.Equal(new DateOnly(2023, 4, 1), actualGame2.Date);
        Assert.Equal("R", actualGame2.GameType);
        Assert.True(actualGame2.IsHome);
        Assert.True(actualGame2.IsWin);
        Assert.Equal(108, actualGame2.Team.Id);
        Assert.Equal("Los Angeles Angels", actualGame2.Team.Name);
        Assert.Equal(718691, actualGame2.Game.GamePk);

        Assert.Equal("6.0 IP, ER, 8 K, 4 BB", actualGame2.Stat.Summary);
        Assert.Equal(10, actualGame2.Stat.GamesPlayed);
        Assert.Equal(20, actualGame2.Stat.GamesStarted);
        Assert.Equal(30, actualGame2.Stat.GroundOuts);
        Assert.Equal(40, actualGame2.Stat.AirOuts);
        Assert.Equal(50, actualGame2.Stat.Runs);
        Assert.Equal(60, actualGame2.Stat.Doubles);
        Assert.Equal(70, actualGame2.Stat.Triples);
        Assert.Equal(80, actualGame2.Stat.HomeRuns);
        Assert.Equal(90, actualGame2.Stat.StrikeOuts);
        Assert.Equal(100, actualGame2.Stat.BaseOnBalls);
        Assert.Equal(110, actualGame2.Stat.IntentionalWalks);
        Assert.Equal(120, actualGame2.Stat.Hits);
        Assert.Equal(130, actualGame2.Stat.HitByPitch);
        Assert.Equal("10.111", actualGame2.Stat.Avg);
        Assert.Equal(140, actualGame2.Stat.AtBats);
        Assert.Equal("10.222", actualGame2.Stat.Obp);
        Assert.Equal("10.333", actualGame2.Stat.Slg);
        Assert.Equal("10.444", actualGame2.Stat.Ops);
        Assert.Equal(150, actualGame2.Stat.CaughtStealing);
        Assert.Equal(160, actualGame2.Stat.StolenBases);
        Assert.Equal("10.555", actualGame2.Stat.StolenBasePercentage);
        Assert.Equal(170, actualGame2.Stat.GroundIntoDoublePlay);
        Assert.Equal(180, actualGame2.Stat.NumberOfPitches);
        Assert.Equal("10.666", actualGame2.Stat.Era);
        Assert.Equal("10777.0", actualGame2.Stat.InningsPitched);
        Assert.Equal(190, actualGame2.Stat.Wins);
        Assert.Equal(200, actualGame2.Stat.Losses);
        Assert.Equal(210, actualGame2.Stat.Saves);
        Assert.Equal(220, actualGame2.Stat.SaveOpportunities);
        Assert.Equal(230, actualGame2.Stat.Holds);
        Assert.Equal(240, actualGame2.Stat.BlownSaves);
        Assert.Equal(250, actualGame2.Stat.EarnedRuns);
        Assert.Equal("10.888", actualGame2.Stat.Whip);
        Assert.Equal(260, actualGame2.Stat.BattersFaced);
        Assert.Equal(270, actualGame2.Stat.Outs);
        Assert.Equal(280, actualGame2.Stat.GamesPitched);
        Assert.Equal(290, actualGame2.Stat.CompleteGames);
        Assert.Equal(300, actualGame2.Stat.Shutouts);
        Assert.Equal(310, actualGame2.Stat.Strikes);
        Assert.Equal("10.999", actualGame2.Stat.StrikePercentage);
        Assert.Equal(320, actualGame2.Stat.HitBatsmen);
        Assert.Equal(330, actualGame2.Stat.Balks);
        Assert.Equal(340, actualGame2.Stat.WildPitches);
        Assert.Equal(350, actualGame2.Stat.Pickoffs);
        Assert.Equal(360, actualGame2.Stat.TotalBases);
        Assert.Equal("11.111", actualGame2.Stat.GroundOutsToAirOuts);
        Assert.Equal("11.222", actualGame2.Stat.WinPercentage);
        Assert.Equal("11.333", actualGame2.Stat.PitchesPerInning);
        Assert.Equal(370, actualGame2.Stat.GamesFinished);
        Assert.Equal("11.444", actualGame2.Stat.StrikeoutWalkRatio);
        Assert.Equal("11.555", actualGame2.Stat.StrikeoutsPer9Inn);
        Assert.Equal("11.666", actualGame2.Stat.WalksPer9Inn);
        Assert.Equal("11.777", actualGame2.Stat.HitsPer9Inn);
        Assert.Equal("11.888", actualGame2.Stat.RunsScoredPer9);
        Assert.Equal("11.999", actualGame2.Stat.HomeRunsPer9);
        Assert.Equal(380, actualGame2.Stat.InheritedRunners);
        Assert.Equal(390, actualGame2.Stat.InheritedRunnersScored);
        Assert.Equal(400, actualGame2.Stat.CatcherInterferences);
        Assert.Equal(410, actualGame2.Stat.SacBunts);
        Assert.Equal(420, actualGame2.Stat.SacFlies);
    }

    [Fact]
    public void Read_FieldingStatsJson_ParsesToFieldingStatsDto()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.FieldingStatsJson);

        // Act
        var actual = JsonSerializer.Deserialize<StatsDto>(json);

        // Assert
        Assert.Equal("fielding", actual.Group.DisplayName);

        var actualGame1 = actual.Splits.ElementAt(0) as GameFieldingStatsDto;
        Assert.NotNull(actualGame1);
        Assert.Equal("2023", actualGame1.Season);
        Assert.Equal(new DateOnly(2023, 3, 30), actualGame1.Date);
        Assert.Equal("R", actualGame1.GameType);
        Assert.False(actualGame1.IsHome);
        Assert.False(actualGame1.IsWin);
        Assert.Equal(144, actualGame1.Team.Id);
        Assert.Equal("Atlanta Braves", actualGame1.Team.Name);
        Assert.Equal(718780, actualGame1.Game.GamePk);

        Assert.Equal(1, actualGame1.Stat.GamesPlayed);
        Assert.Equal(2, actualGame1.Stat.GamesStarted);
        Assert.Equal(3, actualGame1.Stat.CaughtStealing);
        Assert.Equal(4, actualGame1.Stat.StolenBases);
        Assert.Equal(".111", actualGame1.Stat.StolenBasePercentage);
        Assert.Equal(5, actualGame1.Stat.Assists);
        Assert.Equal(6, actualGame1.Stat.Putouts);
        Assert.Equal(7, actualGame1.Stat.Errors);
        Assert.Equal(8, actualGame1.Stat.Chances);
        Assert.Equal(".222", actualGame1.Stat.Fielding);
        Assert.Equal("C", actualGame1.Stat.Position.Abbreviation);
        Assert.Equal("Catcher", actualGame1.Stat.Position.Name);
        Assert.Equal(".333", actualGame1.Stat.RangeFactorPerGame);
        Assert.Equal(".444", actualGame1.Stat.RangeFactorPer9Inn);
        Assert.Equal("5.0", actualGame1.Stat.Innings);
        Assert.Equal(9, actualGame1.Stat.Games);
        Assert.Equal(10, actualGame1.Stat.PassedBall);
        Assert.Equal(11, actualGame1.Stat.DoublePlays);
        Assert.Equal(12, actualGame1.Stat.TriplePlays);
        Assert.Equal(".666", actualGame1.Stat.CatcherEra);
        Assert.Equal(13, actualGame1.Stat.CatcherInterferences);
        Assert.Equal(14, actualGame1.Stat.WildPitches);
        Assert.Equal(15, actualGame1.Stat.ThrowingErrors);
        Assert.Equal(16, actualGame1.Stat.Pickoffs);

        var actualGame2 = actual.Splits.ElementAt(1) as GameFieldingStatsDto;
        Assert.NotNull(actualGame2);
        Assert.Equal("2023", actualGame2.Season);
        Assert.Equal(new DateOnly(2023, 4, 1), actualGame2.Date);
        Assert.Equal("R", actualGame2.GameType);
        Assert.True(actualGame2.IsHome);
        Assert.True(actualGame2.IsWin);
        Assert.Equal(144, actualGame2.Team.Id);
        Assert.Equal("Atlanta Braves", actualGame2.Team.Name);
        Assert.Equal(718781, actualGame2.Game.GamePk);

        Assert.Equal(10, actualGame2.Stat.GamesPlayed);
        Assert.Equal(20, actualGame2.Stat.GamesStarted);
        Assert.Equal(30, actualGame2.Stat.CaughtStealing);
        Assert.Equal(40, actualGame2.Stat.StolenBases);
        Assert.Equal("1.111", actualGame2.Stat.StolenBasePercentage);
        Assert.Equal(50, actualGame2.Stat.Assists);
        Assert.Equal(60, actualGame2.Stat.Putouts);
        Assert.Equal(70, actualGame2.Stat.Errors);
        Assert.Equal(80, actualGame2.Stat.Chances);
        Assert.Equal("1.222", actualGame2.Stat.Fielding);
        Assert.Equal("C", actualGame2.Stat.Position.Abbreviation);
        Assert.Equal("Catcher", actualGame2.Stat.Position.Name);
        Assert.Equal("1.333", actualGame2.Stat.RangeFactorPerGame);
        Assert.Equal("1.444", actualGame2.Stat.RangeFactorPer9Inn);
        Assert.Equal("15.0", actualGame2.Stat.Innings);
        Assert.Equal(90, actualGame2.Stat.Games);
        Assert.Equal(100, actualGame2.Stat.PassedBall);
        Assert.Equal(110, actualGame2.Stat.DoublePlays);
        Assert.Equal(120, actualGame2.Stat.TriplePlays);
        Assert.Equal("1.666", actualGame2.Stat.CatcherEra);
        Assert.Equal(130, actualGame2.Stat.CatcherInterferences);
        Assert.Equal(140, actualGame2.Stat.WildPitches);
        Assert.Equal(150, actualGame2.Stat.ThrowingErrors);
        Assert.Equal(160, actualGame2.Stat.Pickoffs);
    }

    [Fact]
    public void Read_UnknownStatsJson_ThrowsException()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.UnknownStatsJson);
        Func<object> action = () => JsonSerializer.Deserialize<StatsDto>(json);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<UnknownStatGroupException>(actual);
    }

    [Fact]
    public void Write_BattingStatsDto_SerializesMatchingJson()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.BattingStatsJson);
        var dto = JsonSerializer.Deserialize<StatsDto>(json);
        var expected = File.ReadAllText(TestFilesConstants.ExpectedJson.BattingStats);

        // Act
        var actual = JsonSerializer.Serialize(dto);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Write_PitchingStatsDto_SerializesMatchingJson()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.PitchingStatsJson);
        var dto = JsonSerializer.Deserialize<StatsDto>(json);
        var expected = File.ReadAllText(TestFilesConstants.ExpectedJson.PitchingStats);

        // Act
        var actual = JsonSerializer.Serialize(dto);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Write_FieldingStatsDto_SerializesMatchingJson()
    {
        // Arrange
        var json = File.ReadAllText(TestFilesConstants.Objects.FieldingStatsJson);
        var dto = JsonSerializer.Deserialize<StatsDto>(json);
        var expected = File.ReadAllText(TestFilesConstants.ExpectedJson.FieldingStats);

        // Act
        var actual = JsonSerializer.Serialize(dto);

        // Assert
        Assert.Equal(expected, actual);
    }
}