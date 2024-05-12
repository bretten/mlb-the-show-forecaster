using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Games;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Tests.Dtos.Stats;

public class PlayerSeasonStatsByGameDtoTests
{
    [Fact]
    public void GetHittingStats_SeasonStatsWithHitting_ReturnsHittingStats()
    {
        // Arrange
        var gameStats = new GameHittingStatsDto("2024", new DateOnly(2024, 4, 1), "R", true, true, new TeamDto(10, "A"),
            new GameDto(100), new BattingStatsDto());
        var stats = StatsDto.HittingStats(gameStats);
        var playerSeasonStats = new PlayerSeasonStatsByGameDto(1, "First", "Last", new List<StatsDto>() { stats });

        // Act
        var actual = playerSeasonStats.GetHittingStats().ToArray();

        // Assert
        Assert.Single(actual);
        Assert.Equal("2024", actual[0].Season);
        Assert.Equal(new DateOnly(2024, 4, 1), actual[0].Date);
        Assert.Equal("R", actual[0].GameType);
        Assert.True(actual[0].IsHome);
        Assert.True(actual[0].IsWin);
        Assert.Equal(new TeamDto(10, "A"), actual[0].Team);
        Assert.Equal(new GameDto(100), actual[0].Game);
        Assert.Equal(new BattingStatsDto(), actual[0].Stat);
    }

    [Fact]
    public void GetPitchingStats_SeasonStatsWithPitching_ReturnsPitchingStats()
    {
        // Arrange
        var gameStats = new GamePitchingStatsDto("2024", new DateOnly(2024, 4, 1), "R", true, true,
            new TeamDto(10, "A"), new GameDto(100), new PitchingStatsDto());
        var stats = StatsDto.PitchingStats(gameStats);
        var playerSeasonStats = new PlayerSeasonStatsByGameDto(1, "First", "Last", new List<StatsDto>() { stats });

        // Act
        var actual = playerSeasonStats.GetPitchingStats().ToArray();

        // Assert
        Assert.Single(actual);
        Assert.Equal("2024", actual[0].Season);
        Assert.Equal(new DateOnly(2024, 4, 1), actual[0].Date);
        Assert.Equal("R", actual[0].GameType);
        Assert.True(actual[0].IsHome);
        Assert.True(actual[0].IsWin);
        Assert.Equal(new TeamDto(10, "A"), actual[0].Team);
        Assert.Equal(new GameDto(100), actual[0].Game);
        Assert.Equal(new PitchingStatsDto(), actual[0].Stat);
    }

    [Fact]
    public void GetFieldingStats_SeasonStatsWithFielding_ReturnsFieldingStats()
    {
        // Arrange
        var gameStats = new GameFieldingStatsDto("2024", new DateOnly(2024, 4, 1), "R", true, true,
            new TeamDto(10, "A"), new GameDto(100), new FieldingStatsDto());
        var stats = StatsDto.FieldingStats(gameStats);
        var playerSeasonStats = new PlayerSeasonStatsByGameDto(1, "First", "Last", new List<StatsDto>() { stats });

        // Act
        var actual = playerSeasonStats.GetFieldingStats().ToArray();

        // Assert
        Assert.Single(actual);
        Assert.Equal("2024", actual[0].Season);
        Assert.Equal(new DateOnly(2024, 4, 1), actual[0].Date);
        Assert.Equal("R", actual[0].GameType);
        Assert.True(actual[0].IsHome);
        Assert.True(actual[0].IsWin);
        Assert.Equal(new TeamDto(10, "A"), actual[0].Team);
        Assert.Equal(new GameDto(100), actual[0].Game);
        Assert.Equal(new FieldingStatsDto(), actual[0].Stat);
    }
}