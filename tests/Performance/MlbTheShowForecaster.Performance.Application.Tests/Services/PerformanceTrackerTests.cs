using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Queries.GetAllPlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
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
        var player1Id = MlbId.Create(1); // Player 1 has changes
        var player2Id = MlbId.Create(2); // Player 2 has NO changes

        // Stats by season that exist in the system currently
        var player1StatsBySeason = Faker.FakePlayerStatsBySeason(playerMlbId: player1Id.Value);
        var player2StatsBySeason = Faker.FakePlayerStatsBySeason(playerMlbId: player2Id.Value);
        var statsBySeasonInSystem = new List<PlayerStatsBySeason>()
        {
            player1StatsBySeason, player2StatsBySeason
        };

        // The query that returns what's currently in the system
        var getAllPlayerStatsBySeasonQuery = new GetAllPlayerStatsBySeasonQuery(seasonYear);
        var stubQuerySender = Mock.Of<IQuerySender>(x =>
            x.Send(getAllPlayerStatsBySeasonQuery, cToken) == Task.FromResult(statsBySeasonInSystem.AsEnumerable()));

        // Live MLB stats
        var stubPlayerStats = new Mock<IPlayerStats>();
        // Player 1 has new stats from the live data source, so it has changed
        var player1Season = Dtos.TestClasses.Faker.FakePlayerSeason(playerMlbId: player1Id.Value,
            playerGameBattingStats: new List<PlayerGameBattingStats>()
                { Dtos.TestClasses.Faker.FakePlayerGameBattingStats() });
        // Player 2 has no changes
        var player2Season = Dtos.TestClasses.Faker.FakePlayerSeason(playerMlbId: player2Id.Value);
        // Live MLB stats returns the corresponding live stats per each player
        stubPlayerStats.Setup(x => x.GetPlayerSeason(player1Id, seasonYear))
            .ReturnsAsync(player1Season);
        stubPlayerStats.Setup(x => x.GetPlayerSeason(player2Id, seasonYear))
            .ReturnsAsync(player2Season);

        // Mock command sender
        var mockCommandSender = Mock.Of<ICommandSender>();
        // Player 1 expects an update
        var expectedPlayer1UpdateCommand = new UpdatePlayerStatsBySeasonCommand(player1StatsBySeason, player1Season);
        // Player 2 does NOT expect an update
        var notExpectedPlayer2UpdateCommand = new UpdatePlayerStatsBySeasonCommand(player2StatsBySeason, player2Season);

        // The service under test
        var tracker = new PerformanceTracker(stubQuerySender, mockCommandSender, stubPlayerStats.Object);

        /*
         * Act
         */
        await tracker.TrackPlayerPerformance(seasonYear, cToken);

        /*
         * Assert
         */
        // Was the system queried for player season stats?
        Mock.Get(stubQuerySender).Verify(x => x.Send(getAllPlayerStatsBySeasonQuery, cToken), Times.Once);
        // Was the live MLB data queried for each player?
        stubPlayerStats.Verify(x => x.GetPlayerSeason(player1Id, seasonYear), Times.Once);
        stubPlayerStats.Verify(x => x.GetPlayerSeason(player2Id, seasonYear), Times.Once);

        // Was an update command sent for player 1?
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedPlayer1UpdateCommand, cToken), Times.Once);

        // No update command should have been sent for player 2
        Mock.Get(mockCommandSender).Verify(x => x.Send(notExpectedPlayer2UpdateCommand, cToken), Times.Never);
    }
}