using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.Services;

public class PlayerSeasonScorekeeperTests
{
    [Fact]
    public void ScoreSeason_NewStatsByGames_ReturnsUpdatedPlayerStatsBySeason()
    {
        /*
         * Arrange
         */

        // The entity in the system only has games scored on 3/31
        var fakePlayerStatsBySeason = Faker.FakePlayerStatsBySeason(
            battingStatsByGames: new List<PlayerBattingStatsByGame>()
                { Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 3, 31), plateAppearances: 1) },
            pitchingStatsByGames: new List<PlayerPitchingStatsByGame>()
                { Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 3, 31), strikeouts: 1) },
            fieldingStatsByGames: new List<PlayerFieldingStatsByGame>()
                { Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 3, 31), putouts: 1) }
        );

        // The stats by games to date, some of which have not yet been scored yet
        var fakeBattingStatsByGameToDate = new List<PlayerBattingStatsByGame>()
        {
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 3, 31), plateAppearances: 1),
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 1), plateAppearances: 1),
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 4), plateAppearances: 1), // Thursday
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 5), plateAppearances: 1),
        };
        var fakePitchingStatsByGameToDate = new List<PlayerPitchingStatsByGame>()
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 3, 31), strikeouts: 1),
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 1), strikeouts: 1),
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 4), strikeouts: 1), // Thursday
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 5), strikeouts: 1),
        };
        var fakeFieldingStatsByGameToDate = new List<PlayerFieldingStatsByGame>()
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 3, 31), putouts: 1),
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 1), putouts: 1),
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 4), putouts: 1), // Thursday
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 5), putouts: 1),
        };

        // Performance assessment
        var stubPerformanceAssessor = new Mock<IPerformanceAssessor>();
        stubPerformanceAssessor.Setup(x => x.AssessBatting(It.IsAny<BattingStats>()))
            .Returns(PerformanceAssessment.TestClasses.Faker.FakePerformanceScore());
        stubPerformanceAssessor.Setup(x => x.AssessPitching(It.IsAny<PitchingStats>()))
            .Returns(PerformanceAssessment.TestClasses.Faker.FakePerformanceScore());
        stubPerformanceAssessor.Setup(x => x.AssessFielding(It.IsAny<FieldingStats>()))
            .Returns(PerformanceAssessment.TestClasses.Faker.FakePerformanceScore());
        stubPerformanceAssessor.Setup(x => x.Compare(It.IsAny<PerformanceScore>(), It.IsAny<PerformanceScore>()))
            .Returns(PerformanceAssessment.TestClasses.Faker.FakePerformanceScoreComparison());

        // Scorekeeper
        var scorekeeper = new PlayerSeasonScorekeeper(stubPerformanceAssessor.Object);

        /*
         * Act
         */
        var actual = scorekeeper.ScoreSeason(fakePlayerStatsBySeason, fakeBattingStatsByGameToDate,
            fakePitchingStatsByGameToDate, fakeFieldingStatsByGameToDate);

        /*
         * Assert
         */
        // Was the entity updated?
        Assert.Equal(fakeBattingStatsByGameToDate.OrderBy(x => x.GameDate),
            actual.BattingStatsByGamesChronologically);
        Assert.Equal(fakePitchingStatsByGameToDate.OrderBy(x => x.GameDate),
            actual.PitchingStatsByGamesChronologically);
        Assert.Equal(fakeFieldingStatsByGameToDate.OrderBy(x => x.GameDate),
            actual.FieldingStatsByGamesChronologically);

        // Were the new games logged?
        Assert.Equal(9, actual.DomainEvents.Count);
        Assert.Equal(3,
            actual.DomainEvents.Count(x =>
                x is PlayerBattedInGameEvent gameEvent && (gameEvent.Date == new DateOnly(2024, 4, 1) ||
                                                           gameEvent.Date == new DateOnly(2024, 4, 4) ||
                                                           gameEvent.Date == new DateOnly(2024, 4, 5))));
        Assert.Equal(3,
            actual.DomainEvents.Count(x =>
                x is PlayerPitchedInGameEvent gameEvent && (gameEvent.Date == new DateOnly(2024, 4, 1) ||
                                                            gameEvent.Date == new DateOnly(2024, 4, 4) ||
                                                            gameEvent.Date == new DateOnly(2024, 4, 5))));
        Assert.Equal(3,
            actual.DomainEvents.Count(x =>
                x is PlayerFieldedInGameEvent gameEvent && (gameEvent.Date == new DateOnly(2024, 4, 1) ||
                                                            gameEvent.Date == new DateOnly(2024, 4, 4) ||
                                                            gameEvent.Date == new DateOnly(2024, 4, 5))));

        // Was the player's performance assessed?
        stubPerformanceAssessor.Verify(x => x.AssessBatting(It.IsAny<BattingStats>()), Times.Exactly(3));
        stubPerformanceAssessor.Verify(x => x.AssessPitching(It.IsAny<PitchingStats>()), Times.Exactly(3));
        stubPerformanceAssessor.Verify(x => x.AssessFielding(It.IsAny<FieldingStats>()), Times.Exactly(3));
        // Make sure the performance was assessed on Thursday, when the player had 3 of each stat
        stubPerformanceAssessor.Verify(x => x.AssessBatting(It.Is<BattingStats>(y => y.PlateAppearances.Value == 3)),
            Times.Exactly(1));
        stubPerformanceAssessor.Verify(x => x.AssessPitching(It.Is<PitchingStats>(y => y.Strikeouts.Value == 3)),
            Times.Exactly(1));
        stubPerformanceAssessor.Verify(x => x.AssessFielding(It.Is<FieldingStats>(y => y.Putouts.Value == 3)),
            Times.Exactly(1));
    }
}