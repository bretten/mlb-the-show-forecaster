using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;
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
        var fakePlayerStatsBySeason = Faker.FakePlayerSeasonStats(
            battingStatsByGames: new List<PlayerBattingStatsByGame>()
                { Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 3, 31)) },
            pitchingStatsByGames: new List<PlayerPitchingStatsByGame>()
                { Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 3, 31)) },
            fieldingStatsByGames: new List<PlayerFieldingStatsByGame>()
                { Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 3, 31)) }
        );

        // The stats by games to date, some of which have not yet been scored yet
        var fakeBattingStatsByGameToDate = new List<PlayerBattingStatsByGame>()
        {
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 3, 31)),
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 4, 1))
        };
        var fakePitchingStatsByGameToDate = new List<PlayerPitchingStatsByGame>()
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 3, 31)),
            Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 1))
        };
        var fakeFieldingStatsByGameToDate = new List<PlayerFieldingStatsByGame>()
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 3, 31)),
            Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 1))
        };

        // Performance assessment
        var mockPerformanceAssessmentRequirements = Mock.Of<IPerformanceAssessmentRequirements>();
        var comparisonDate = new DateTime(2024, 4, 1);

        // Scorekeeper
        var scorekeeper = new PlayerSeasonScorekeeper(mockPerformanceAssessmentRequirements);

        /*
         * Act
         */
        var actual = scorekeeper.ScoreSeason(fakePlayerStatsBySeason, comparisonDate,
            fakeBattingStatsByGameToDate, fakePitchingStatsByGameToDate, fakeFieldingStatsByGameToDate);

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
        Assert.Equal(3, actual.DomainEvents.Count);
        Assert.Equal(1,
            actual.DomainEvents.Count(x =>
                x is PlayerBattedInGameEvent gameEvent && gameEvent.Date == new DateTime(2024, 4, 1)));
        Assert.Equal(1,
            actual.DomainEvents.Count(x =>
                x is PlayerPitchedInGameEvent gameEvent && gameEvent.Date == new DateTime(2024, 4, 1)));
        Assert.Equal(1,
            actual.DomainEvents.Count(x =>
                x is PlayerFieldedInGameEvent gameEvent && gameEvent.Date == new DateTime(2024, 4, 1)));

        // Was the player's performance assessed?
        Mock.Get(mockPerformanceAssessmentRequirements)
            .Verify(x => x.AreBattingAssessmentRequirementsMet(It.IsAny<NaturalNumber>()), Times.Once);
        Mock.Get(mockPerformanceAssessmentRequirements).Verify(
            x => x.ArePitchingAssessmentRequirementsMet(It.IsAny<InningsCount>(), It.IsAny<NaturalNumber>()),
            Times.Once);
        Mock.Get(mockPerformanceAssessmentRequirements)
            .Verify(x => x.AreFieldingAssessmentRequirementsMet(It.IsAny<NaturalNumber>()), Times.Once);
    }
}