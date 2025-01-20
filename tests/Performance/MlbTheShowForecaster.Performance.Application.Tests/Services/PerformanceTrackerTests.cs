using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.CreatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Queries.GetAllPlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Services;

public class PerformanceTrackerTests
{
    [Fact]
    public async Task TrackPlayerPerformance_NoPlayerSeasonsInDomain_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        var getAllPlayerStatsBySeasonQuery = new GetAllPlayerStatsBySeasonQuery(seasonYear);
        var stubQuerySender = Mock.Of<IQuerySender>(x =>
            x.Send(getAllPlayerStatsBySeasonQuery, cToken) == Task.FromResult(Enumerable.Empty<PlayerStatsBySeason>()));

        var mockPlayerStats = Mock.Of<IPlayerStats>();
        var mockCommandSender = Mock.Of<ICommandSender>();
        var tracker = new PerformanceTracker(stubQuerySender, mockCommandSender, mockPlayerStats);
        var action = () => tracker.TrackPlayerPerformance(seasonYear, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PerformanceTrackerFoundNoPlayerSeasonsException>(actual);
    }

    [Fact]
    public async Task TrackPlayerPerformance_SeasonYear_UpdatesPlayerSeasonPerformance()
    {
        /*
         * Act
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);
        // There will be a player with and without changes
        var player1Id = MlbId.Create(1); // Player1 has changes
        var player2Id = MlbId.Create(2); // Player2 has NO changes
        var player3Id = MlbId.Create(3); // Player3 is being created

        // Stats by season that exist in the domain currently
        var player1StatsBySeason = Faker.FakePlayerStatsBySeason(playerMlbId: player1Id.Value);
        var player2StatsBySeason = Faker.FakePlayerStatsBySeason(playerMlbId: player2Id.Value);
        var statsBySeasonInDomain = new List<PlayerStatsBySeason>()
        {
            player1StatsBySeason, player2StatsBySeason
        };

        // The query that returns what's currently in the domain
        var getAllPlayerStatsBySeasonQuery = new GetAllPlayerStatsBySeasonQuery(seasonYear);
        var stubQuerySender = Mock.Of<IQuerySender>(x =>
            x.Send(getAllPlayerStatsBySeasonQuery, cToken) == Task.FromResult(statsBySeasonInDomain.AsEnumerable()));

        // Live MLB stats
        var stubPlayerStats = new Mock<IPlayerStats>();
        // Player1 has new stats from the live data source, so it has changed
        var player1Season = Dtos.TestClasses.Faker.FakePlayerSeason(playerMlbId: player1Id.Value,
            playerGameBattingStats: new List<PlayerGameBattingStats>()
                { Dtos.TestClasses.Faker.FakePlayerGameBattingStats() });
        // Player2 has no changes
        var player2Season = Dtos.TestClasses.Faker.FakePlayerSeason(playerMlbId: player2Id.Value);
        // Player3 stats do not yet exist in the domain, so they will be created
        var player3Season = Dtos.TestClasses.Faker.FakePlayerSeason(playerMlbId: player3Id.Value,
            playerGamePitchingStats: new List<PlayerGamePitchingStats>()
                { Dtos.TestClasses.Faker.FakePlayerGamePitchingStats() });
        // Live MLB stats returns the corresponding live stats per each player
        stubPlayerStats.Setup(x => x.GetAllPlayerStatsFor(seasonYear, cToken))
            .Returns(ToAsyncEnumerable(new List<PlayerSeason>() { player1Season, player2Season, player3Season }));

        // Mock command sender
        var mockCommandSender = new Mock<ICommandSender>();
        // Player1 expects an update
        var expectedPlayer1UpdateCommand = new UpdatePlayerStatsBySeasonCommand(player1StatsBySeason, player1Season);
        // Player2 does NOT expect an update
        var notExpectedPlayer2UpdateCommand = new UpdatePlayerStatsBySeasonCommand(player2StatsBySeason, player2Season);
        // Player3 expects a create command
        var expectedPlayer3CreateCommand = new CreatePlayerStatsBySeasonCommand(player3Season);

        // The service under test
        var tracker = new PerformanceTracker(stubQuerySender, mockCommandSender.Object, stubPlayerStats.Object);

        /*
         * Act
         */
        var actual = await tracker.TrackPlayerPerformance(seasonYear, cToken);

        /*
         * Assert
         */
        // There were 2 player seasons in the domain
        Assert.Equal(3, actual.TotalPlayerSeasons);
        // Player3's season stats were created in the domain
        Assert.Equal(1, actual.TotalNewPlayerSeasons);
        // Player1 had a season performance update
        Assert.Equal(1, actual.TotalPlayerSeasonUpdates);
        // Player2 had no changes
        Assert.Equal(1, actual.TotalUpToDatePlayerSeasons);

        // Was the domain queried for player season stats?
        Mock.Get(stubQuerySender).Verify(x => x.Send(getAllPlayerStatsBySeasonQuery, cToken), Times.Once);
        // Was the live MLB data queried?
        stubPlayerStats.Verify(x => x.GetAllPlayerStatsFor(seasonYear, cToken), Times.Once);

        // Was an update command sent for player 1?
        mockCommandSender.Verify(x => x.Send(expectedPlayer1UpdateCommand, cToken), Times.Once);

        // No update command should have been sent for player 2
        mockCommandSender.Verify(x => x.Send(notExpectedPlayer2UpdateCommand, cToken), Times.Never);

        // Was a create command sent for player 3?
        mockCommandSender.Verify(x => x.Send(expectedPlayer3CreateCommand, cToken), Times.Once);
    }

    private static async IAsyncEnumerable<PlayerSeason> ToAsyncEnumerable(List<PlayerSeason> playerSeasons)
    {
        foreach (var playerSeason in playerSeasons)
        {
            yield return playerSeason;
        }

        await Task.CompletedTask;
    }
}