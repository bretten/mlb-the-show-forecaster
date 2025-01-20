using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using Moq;

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

        var mapper = new PlayerSeasonMapper(Mock.Of<IPerformanceAssessor>(), Mock.Of<IParticipationAssessor>());

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

        var mapper = new PlayerSeasonMapper(Mock.Of<IPerformanceAssessor>(), Mock.Of<IParticipationAssessor>());

        // Act
        var actual = mapper.MapBattingGames(battingGames);

        // Assert
        var actualList = actual.ToList();
        Assert.Equal(2, actualList.Count);
        Assert.Equal(Domain.Tests.PlayerSeasons.TestClasses.Faker.FakeTestPlayerBattingStats(scalar: 1,
            gameDate: new DateOnly(2024, 4, 30)), actualList[0]);
        Assert.Equal(Domain.Tests.PlayerSeasons.TestClasses.Faker.FakeTestPlayerBattingStats(scalar: 1000,
            gameDate: new DateOnly(2024, 5, 1)), actualList[1]);
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

        var mapper = new PlayerSeasonMapper(Mock.Of<IPerformanceAssessor>(), Mock.Of<IParticipationAssessor>());

        // Act
        var actual = mapper.MapPitchingGames(pitchingGames);

        // Assert
        var actualList = actual.ToList();
        Assert.Equal(2, actualList.Count);
        Assert.Equal(Domain.Tests.PlayerSeasons.TestClasses.Faker.FakeTestPlayerPitchingStats(scalar: 1,
                gameDate: new DateOnly(2024, 4, 30), win: true, gameStarted: true, shutout: true, completeGame: true),
            actualList[0]);
        Assert.Equal(Domain.Tests.PlayerSeasons.TestClasses.Faker.FakeTestPlayerPitchingStats(scalar: 1000,
                gameDate: new DateOnly(2024, 5, 1), loss: true, gameFinished: true, blownSave: true,
                saveOpportunity: true),
            actualList[1]);
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

        var mapper = new PlayerSeasonMapper(Mock.Of<IPerformanceAssessor>(), Mock.Of<IParticipationAssessor>());

        // Act
        var actual = mapper.MapFieldingGames(fieldingGames);

        // Assert
        var actualList = actual.ToList();
        Assert.Equal(2, actualList.Count);
        Assert.Equal(Domain.Tests.PlayerSeasons.TestClasses.Faker.FakeTestPlayerFieldingStats(scalar: 1,
            gameDate: new DateOnly(2024, 4, 30), gameStarted: true), actualList[0]);
        Assert.Equal(Domain.Tests.PlayerSeasons.TestClasses.Faker.FakeTestPlayerFieldingStats(scalar: 1000,
            gameDate: new DateOnly(2024, 5, 1), gameStarted: false), actualList[1]);
    }

    [Fact]
    public void MapToPlayerSeasonPerformanceMetrics_PlayerStatsBySeason_ReturnsPlayerSeasonPerformanceMetrics()
    {
        // Arrange
        var day1 = new DateOnly(2024, 9, 30);
        var day2 = new DateOnly(2024, 10, 1);
        var playerStatsBySeason = Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerStatsBySeason(100, 2024,
            battingScore: 0.1m, pitchingScore: 0.2m, fieldingScore: 0.3m,
            battingStatsByGames: new List<PlayerBattingStatsByGame>()
            {
                Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerBattingStats(plateAppearances: 4, atBats: 3,
                    hits: 2, homeRuns: 1, baseOnBalls: 1, gameDate: day1),
                Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerBattingStats(plateAppearances: 5, atBats: 4,
                    hits: 1, doubles: 1, baseOnBalls: 1, gameDate: day2)
            },
            pitchingStatsByGames: new List<PlayerPitchingStatsByGame>()
            {
                Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerPitchingStats(inningsPitched: 3,
                    battersFaced: 10, atBats: 9, baseOnBalls: 1, strikeouts: 7, hits: 2, homeRuns: 2, earnedRuns: 2,
                    gameDate: day1),
                Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerPitchingStats(inningsPitched: 2,
                    battersFaced: 10, atBats: 5, baseOnBalls: 5, strikeouts: 4, hits: 1, homeRuns: 1, earnedRuns: 1,
                    gameDate: day2)
            },
            fieldingStatsByGames: new List<PlayerFieldingStatsByGame>()
            {
                Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerFieldingStats(assists: 3, putouts: 2, errors: 1,
                    gameDate: day1),
                Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerFieldingStats(assists: 5, putouts: 4, errors: 1,
                    gameDate: day2),
            });

        var battingStatsDay1 = playerStatsBySeason.BattingStatsFor(day1, day1);
        var battingStatsDay2 = playerStatsBySeason.BattingStatsFor(day1, day2);
        var pitchingStatsDay1 = playerStatsBySeason.PitchingStatsFor(day1, day1);
        var pitchingStatsDay2 = playerStatsBySeason.PitchingStatsFor(day1, day2);
        var fieldingStatsDay1 = playerStatsBySeason.FieldingStatsFor(day1, day1);
        var fieldingStatsDay2 = playerStatsBySeason.FieldingStatsFor(day1, day2);

        var stubPerformanceAssessor = new Mock<IPerformanceAssessor>();
        stubPerformanceAssessor.Setup(x => x.AssessBatting(battingStatsDay1))
            .Returns(Domain.Tests.PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.1m));
        stubPerformanceAssessor.Setup(x => x.AssessBatting(battingStatsDay2))
            .Returns(Domain.Tests.PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.2m));
        stubPerformanceAssessor.Setup(x => x.AssessPitching(pitchingStatsDay1))
            .Returns(Domain.Tests.PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.3m));
        stubPerformanceAssessor.Setup(x => x.AssessPitching(pitchingStatsDay2))
            .Returns(Domain.Tests.PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.4m));
        stubPerformanceAssessor.Setup(x => x.AssessFielding(fieldingStatsDay1))
            .Returns(Domain.Tests.PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.5m));
        stubPerformanceAssessor.Setup(x => x.AssessFielding(fieldingStatsDay2))
            .Returns(Domain.Tests.PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.6m));

        var stubParticipationAssessor = new Mock<IParticipationAssessor>();
        stubParticipationAssessor.Setup(x => x.AssessBatting(day1, day1, battingStatsDay1))
            .Returns(false);
        stubParticipationAssessor.Setup(x => x.AssessBatting(day1, day2, battingStatsDay2))
            .Returns(true);
        stubParticipationAssessor.Setup(x => x.AssessPitching(day1, day1, pitchingStatsDay1))
            .Returns(false);
        stubParticipationAssessor.Setup(x => x.AssessPitching(day1, day2, pitchingStatsDay2))
            .Returns(true);
        stubParticipationAssessor.Setup(x => x.AssessFielding(day1, day1, fieldingStatsDay1))
            .Returns(false);
        stubParticipationAssessor.Setup(x => x.AssessFielding(day1, day2, fieldingStatsDay2))
            .Returns(true);

        // Act
        var mapper = new PlayerSeasonMapper(stubPerformanceAssessor.Object, stubParticipationAssessor.Object);

        // Act
        var actual = mapper.MapToPlayerSeasonPerformanceMetrics(playerStatsBySeason, new DateOnly(2024, 9, 30),
            new DateOnly(2024, 10, 1));

        // Assert
        Assert.Equal(2024, actual.Year.Value);
        Assert.Equal(100, actual.MlbId.Value);

        Assert.Equal(new DateOnly(2024, 9, 30), actual.MetricsByDate[0].Date);
        Assert.Equal(0.1m, actual.MetricsByDate[0].BattingScore);
        Assert.False(actual.MetricsByDate[0].SignificantBattingParticipation);
        Assert.Equal(0.3m, actual.MetricsByDate[0].PitchingScore);
        Assert.False(actual.MetricsByDate[0].SignificantPitchingParticipation);
        Assert.Equal(0.5m, actual.MetricsByDate[0].FieldingScore);
        Assert.False(actual.MetricsByDate[0].SignificantFieldingParticipation);
        Assert.Equal(0.667m, actual.MetricsByDate[0].BattingAverage);
        Assert.Equal(0.750m, actual.MetricsByDate[0].OnBasePercentage);
        Assert.Equal(1.667m, actual.MetricsByDate[0].Slugging);
        Assert.Equal(6m, actual.MetricsByDate[0].EarnedRunAverage);
        Assert.Equal(0.222m, actual.MetricsByDate[0].OpponentsBattingAverage);
        Assert.Equal(21m, actual.MetricsByDate[0].StrikeoutsPer9);
        Assert.Equal(3m, actual.MetricsByDate[0].BaseOnBallsPer9);
        Assert.Equal(6m, actual.MetricsByDate[0].HomeRunsPer9);
        Assert.Equal(0.833m, actual.MetricsByDate[0].FieldingPercentage);

        Assert.Equal(new DateOnly(2024, 10, 1), actual.MetricsByDate[1].Date);
        Assert.Equal(0.2m, actual.MetricsByDate[1].BattingScore);
        Assert.True(actual.MetricsByDate[1].SignificantBattingParticipation);
        Assert.Equal(0.4m, actual.MetricsByDate[1].PitchingScore);
        Assert.True(actual.MetricsByDate[1].SignificantPitchingParticipation);
        Assert.Equal(0.6m, actual.MetricsByDate[1].FieldingScore);
        Assert.True(actual.MetricsByDate[1].SignificantFieldingParticipation);
        Assert.Equal(0.429m, actual.MetricsByDate[1].BattingAverage);
        Assert.Equal(0.556m, actual.MetricsByDate[1].OnBasePercentage);
        Assert.Equal(1m, actual.MetricsByDate[1].Slugging);
        Assert.Equal(5.4m, actual.MetricsByDate[1].EarnedRunAverage);
        Assert.Equal(0.214m, actual.MetricsByDate[1].OpponentsBattingAverage);
        Assert.Equal(19.8m, actual.MetricsByDate[1].StrikeoutsPer9);
        Assert.Equal(10.8m, actual.MetricsByDate[1].BaseOnBallsPer9);
        Assert.Equal(5.4m, actual.MetricsByDate[1].HomeRunsPer9);
        Assert.Equal(0.875m, actual.MetricsByDate[1].FieldingPercentage);
    }
}