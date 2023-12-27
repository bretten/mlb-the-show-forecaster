using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.CreatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Queries.GetPlayerByMlbId;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Services;

public class PlayerStatusUpdaterTests
{
    [Fact]
    public async Task UpdatePlayerStatuses_NewAndExistingPlayers_CreatesAndUpdates()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        // Fake Team
        var team = TeamFaker.Fake(PlayerStatusFaker.DefaultTeamMlbId);
        var otherTeam = TeamFaker.Fake(999);

        // Fake Players in the system
        Player player1 = PlayerFaker.Fake(1, active: true, team: team); // Player has no status updates
        Player? player2 = null; // Player doesn't exist, so will be created
        Player player3 = PlayerFaker.Fake(3, active: false, team: otherTeam); // Player will be updated

        // Player status changes
        var player1Changes = new PlayerStatusChanges(new List<PlayerStatusChangeType>(), TeamFaker.NoTeam);
        var player2Changes = new PlayerStatusChanges(new List<PlayerStatusChangeType>()
        {
            PlayerStatusChangeType.Activated,
            PlayerStatusChangeType.SignedContractWithTeam
        }, team);
        var player3Changes = new PlayerStatusChanges(new List<PlayerStatusChangeType>()
        {
            PlayerStatusChangeType.Activated,
            PlayerStatusChangeType.SignedContractWithTeam
        }, team);

        // Team Provider
        var stubTeamProvider = Mock.Of<ITeamProvider>(x => x.GetBy(team.MlbId) == team);

        // Roster
        var playerStatus1 = PlayerStatusFaker.Fake(1, active: true, teamMlbId: team.MlbId.Value);
        var playerStatus2 = PlayerStatusFaker.Fake(2, active: true, teamMlbId: team.MlbId.Value);
        var playerStatus3 = PlayerStatusFaker.Fake(3, active: true, teamMlbId: team.MlbId.Value);
        var playerStatuses = new List<Application.Dtos.PlayerStatus>()
        {
            playerStatus1,
            playerStatus2,
            playerStatus3,
        };
        var stubRoster = Mock.Of<IPlayerRoster>(x =>
            x.GetPlayerStatuses(cToken) == Task.FromResult(playerStatuses.AsEnumerable()));

        // Query
        var mockQuerySender = new Mock<IQuerySender>();
        var query1 = new GetPlayerByMlbIdQuery(playerStatus1.MlbId);
        var query2 = new GetPlayerByMlbIdQuery(playerStatus2.MlbId);
        var query3 = new GetPlayerByMlbIdQuery(playerStatus3.MlbId);
        mockQuerySender.Setup(x => x.Send(query1, cToken)).ReturnsAsync(player1);
        mockQuerySender.Setup(x => x.Send(query2, cToken)).ReturnsAsync(player2);
        mockQuerySender.Setup(x => x.Send(query3, cToken)).ReturnsAsync(player3);

        // Status change detections
        var stubPlayerStatusDetector = new Mock<IPlayerStatusChangeDetector>();
        stubPlayerStatusDetector.Setup(x => x.DetectChanges(player1, playerStatus1.Active, team))
            .Returns(player1Changes);
        stubPlayerStatusDetector.Setup(x => x.DetectChanges(player3, playerStatus3.Active, team))
            .Returns(player3Changes);

        // Command
        var mockCommandSender = new Mock<ICommandSender>();
        var createPlayerCommand1 = new CreatePlayerCommand(playerStatus1);
        var createPlayerCommand2 = new CreatePlayerCommand(playerStatus2);
        var createPlayerCommand3 = new CreatePlayerCommand(playerStatus3);
        var updatePlayerCommand1 = new UpdatePlayerCommand(player1, player1Changes);
        var updatePlayerCommand3 = new UpdatePlayerCommand(player3, player3Changes);

        // Service
        var playerStatusUpdater = new PlayerStatusUpdater(stubRoster, mockQuerySender.Object, mockCommandSender.Object,
            stubPlayerStatusDetector.Object, stubTeamProvider);

        /*
         * Act
         */
        await playerStatusUpdater.UpdatePlayerStatuses(cToken);

        /*
         * Assert
         */
        // Check existing Players
        mockQuerySender.Verify(x => x.Send(query1, cToken), Times.Once);
        mockQuerySender.Verify(x => x.Send(query2, cToken), Times.Once);
        mockQuerySender.Verify(x => x.Send(query3, cToken), Times.Once);

        // Create players
        mockCommandSender.Verify(x => x.Send(createPlayerCommand1, cToken), Times.Never);
        mockCommandSender.Verify(x => x.Send(createPlayerCommand2, cToken), Times.Once);
        mockCommandSender.Verify(x => x.Send(createPlayerCommand3, cToken), Times.Never);

        // Update players
        mockCommandSender.Verify(x => x.Send(updatePlayerCommand1, cToken), Times.Never);
        mockCommandSender.Verify(
            x => x.Send(It.Is<UpdatePlayerCommand>(c => c.Player.MlbId == playerStatus2.MlbId), cToken), Times.Never);
        mockCommandSender.Verify(x => x.Send(updatePlayerCommand3, cToken), Times.Once);
    }
}