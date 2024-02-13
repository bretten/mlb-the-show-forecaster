using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.Entities;

public class PlayerStatsBySeasonTests
{
    [Fact]
    public void BattingStatsByGamesChronologically_BattingStats_ReturnsStatsOrderedByGameDate()
    {
        // Arrange
        var game1 = Faker.FakeBattingStats(gameDate: new DateTime(2024, 3, 31));
        var game2 = Faker.FakeBattingStats(gameDate: new DateTime(2024, 4, 2));
        var game3 = Faker.FakeBattingStats(gameDate: new DateTime(2024, 4, 5));
        var battingStatsByGames = new List<PlayerBattingStatsByGame>()
            { game3, game2, game1 }; // Most recent to oldest
        var seasonStats = Faker.FakeSeasonStats(battingStatsByGames: battingStatsByGames);

        // Act
        var actual = seasonStats.BattingStatsByGamesChronologically;

        // Assert
        Assert.Equal(3, actual.Count);
        Assert.Equal(new DateTime(2024, 3, 31), actual[0].GameDate);
        Assert.Equal(new DateTime(2024, 4, 2), actual[1].GameDate);
        Assert.Equal(new DateTime(2024, 4, 5), actual[2].GameDate);
    }

    [Fact]
    public void FieldingStatsByGamesChronologically_FieldingStats_ReturnsStatsOrderedByGameDate()
    {
        // Arrange
        var game1 = Faker.FakeFieldingStats(gameDate: new DateTime(2024, 3, 31));
        var game2 = Faker.FakeFieldingStats(gameDate: new DateTime(2024, 4, 2));
        var game3 = Faker.FakeFieldingStats(gameDate: new DateTime(2024, 4, 5));
        var fieldingStatsByGames = new List<PlayerFieldingStatsByGame>()
            { game3, game2, game1 }; // Most recent to oldest
        var seasonStats = Faker.FakeSeasonStats(fieldingStatsByGames: fieldingStatsByGames);

        // Act
        var actual = seasonStats.FieldingStatsByGamesChronologically;

        // Assert
        Assert.Equal(3, actual.Count);
        Assert.Equal(new DateTime(2024, 3, 31), actual[0].GameDate);
        Assert.Equal(new DateTime(2024, 4, 2), actual[1].GameDate);
        Assert.Equal(new DateTime(2024, 4, 5), actual[2].GameDate);
    }

    [Fact]
    public void PitchingStatsByGamesChronologically_PitchingStats_ReturnsStatsOrderedByGameDate()
    {
        // Arrange
        var game1 = Faker.FakePitchingStats(gameDate: new DateTime(2024, 3, 31));
        var game2 = Faker.FakePitchingStats(gameDate: new DateTime(2024, 4, 2));
        var game3 = Faker.FakePitchingStats(gameDate: new DateTime(2024, 4, 5));
        var pitchingStatsByGames = new List<PlayerPitchingStatsByGame>()
            { game3, game2, game1 }; // Most recent to oldest
        var seasonStats = Faker.FakeSeasonStats(pitchingStatsByGames: pitchingStatsByGames);

        // Act
        var actual = seasonStats.PitchingStatsByGamesChronologically;

        // Assert
        Assert.Equal(3, actual.Count);
        Assert.Equal(new DateTime(2024, 3, 31), actual[0].GameDate);
        Assert.Equal(new DateTime(2024, 4, 2), actual[1].GameDate);
        Assert.Equal(new DateTime(2024, 4, 5), actual[2].GameDate);
    }

    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var mlbId = MlbId.Create(1);
        var seasonYear = SeasonYear.Create(2024);
        var battingStats = new List<PlayerBattingStatsByGame>() { Faker.FakeBattingStats() };
        var fieldingStats = new List<PlayerFieldingStatsByGame>() { Faker.FakeFieldingStats() };
        var pitchingStats = new List<PlayerPitchingStatsByGame>() { Faker.FakePitchingStats() };

        // Act
        var actual = PlayerStatsBySeason.Create(mlbId, seasonYear, battingStats, fieldingStats, pitchingStats);
        
        // Assert
        Assert.Equal(mlbId, actual.PlayerId);
        Assert.Equal(seasonYear, actual.SeasonYear);
        Assert.Equal(battingStats, actual.BattingStatsByGamesChronologically);
        Assert.Equal(fieldingStats, actual.FieldingStatsByGamesChronologically);
        Assert.Equal(pitchingStats, actual.PitchingStatsByGamesChronologically);
    }
}