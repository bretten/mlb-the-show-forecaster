using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.Mapping;

public class PlayerSeasonMapperTests
{
    [Fact]
    public void Map_PlayerSeason_ReturnsPlayerStatsBySeason()
    {
        // Arrange
        const int playerMlbId = 1;
        const int seasonYear = 2024;
        var playerSeason = new PlayerSeason(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            new List<PlayerGameBattingStats>(), new List<PlayerGamePitchingStats>(),
            new List<PlayerGameFieldingStats>());

        var mapper = new PlayerSeasonMapper();

        // Act
        var actual = mapper.Map(playerSeason);

        // Assert
        Assert.Equal(1, actual.PlayerMlbId.Value);
        Assert.Equal(2024, actual.SeasonYear.Value);
        Assert.Equal(0m, actual.BattingScore.Value);
        Assert.Equal(0m, actual.PitchingScore.Value);
        Assert.Equal(0m, actual.FieldingScore.Value);
        Assert.Empty(actual.BattingStatsByGamesChronologically);
        Assert.Empty(actual.PitchingStatsByGamesChronologically);
        Assert.Empty(actual.FieldingStatsByGamesChronologically);
    }

    [Fact]
    public void MapBattingGames_BattingStats_ReturnsPlayerBattingStatsByGames()
    {
        // Arrange
        var battingGame1 = Faker.FakePlayerGameBattingStats(scalar: 1, gameDate: new DateOnly(2024, 4, 30));
        var battingGame2 = Faker.FakePlayerGameBattingStats(scalar: 1000, gameDate: new DateOnly(2024, 5, 1));
        var battingGames = new List<PlayerGameBattingStats>()
        {
            battingGame1, battingGame2
        };

        var mapper = new PlayerSeasonMapper();

        // Act
        var actual = mapper.MapBattingGames(battingGames);

        // Assert
        var actualList = actual.ToList();
        Assert.Equal(2, actualList.Count);
        Assert.Equal(Tests.TestClasses.Faker.FakePlayerBattingStats(scalar: 1, gameDate: new DateOnly(2024, 4, 30)),
            actualList[0]);
        Assert.Equal(Tests.TestClasses.Faker.FakePlayerBattingStats(scalar: 1000, gameDate: new DateOnly(2024, 5, 1)),
            actualList[1]);
    }

    [Fact]
    public void MapPitchingGames_PitchingStats_ReturnsPlayerPitchingStatsByGames()
    {
        // Arrange
        var pitchingGame1 = Faker.FakePlayerGamePitchingStats(scalar: 1, gameDate: new DateOnly(2024, 4, 30), win: true,
            gameStarted: true, shutout: true, completeGame: true);
        var pitchingGame2 = Faker.FakePlayerGamePitchingStats(scalar: 1000, gameDate: new DateOnly(2024, 5, 1),
            loss: true, gameFinished: true, blownSave: true, saveOpportunity: true);
        var pitchingGames = new List<PlayerGamePitchingStats>()
        {
            pitchingGame1, pitchingGame2
        };

        var mapper = new PlayerSeasonMapper();

        // Act
        var actual = mapper.MapPitchingGames(pitchingGames);

        // Assert
        var actualList = actual.ToList();
        Assert.Equal(2, actualList.Count);
        Assert.Equal(
            Tests.TestClasses.Faker.FakePlayerPitchingStats(scalar: 1, gameDate: new DateOnly(2024, 4, 30), win: true,
                gameStarted: true, shutout: true, completeGame: true), actualList[0]);
        Assert.Equal(
            Tests.TestClasses.Faker.FakePlayerPitchingStats(scalar: 1000, gameDate: new DateOnly(2024, 5, 1),
                loss: true, gameFinished: true, blownSave: true, saveOpportunity: true), actualList[1]);
    }

    [Fact]
    public void MapFieldingGames_FieldingStats_ReturnsPlayerFieldingStatsByGames()
    {
        // Arrange
        var fieldingGame1 =
            Faker.FakePlayerGameFieldingStats(scalar: 1, gameDate: new DateOnly(2024, 4, 30), gameStarted: true);
        var fieldingGame2 =
            Faker.FakePlayerGameFieldingStats(scalar: 1000, gameDate: new DateOnly(2024, 5, 1), gameStarted: false);
        var fieldingGames = new List<PlayerGameFieldingStats>()
        {
            fieldingGame1, fieldingGame2
        };

        var mapper = new PlayerSeasonMapper();

        // Act
        var actual = mapper.MapFieldingGames(fieldingGames);

        // Assert
        var actualList = actual.ToList();
        Assert.Equal(2, actualList.Count);
        Assert.Equal(
            Tests.TestClasses.Faker.FakePlayerFieldingStats(scalar: 1, gameDate: new DateOnly(2024, 4, 30),
                gameStarted: true), actualList[0]);
        Assert.Equal(
            Tests.TestClasses.Faker.FakePlayerFieldingStats(scalar: 1000, gameDate: new DateOnly(2024, 5, 1),
                gameStarted: false), actualList[1]);
    }
}