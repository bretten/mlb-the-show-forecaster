using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.CreatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Queries.GetPlayerByMlbId;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Services;

public class PlayerStatusUpdaterTests
{
    [Fact]
    public async Task UpdatePlayerStatuses_NewAndExistingPlayers_CreatesAndUpdates()
    {
        // Arrange
        var scenario = new TestScenario();
        var stubPlayerRoster = scenario.StubPlayerRoster;
        var stubPlayerStatusDetector = scenario.StubPlayerStatusDetector;
        var mockQuerySender = scenario.MockQuerySender;
        var mockCommandSender = scenario.MockCommandSender;

        var playerStatusUpdater = new PlayerStatusUpdater(stubPlayerRoster, mockQuerySender.Object,
            mockCommandSender.Object, stubPlayerStatusDetector.Object, scenario.StubTeamProvider);

        // Act
        await playerStatusUpdater.UpdatePlayerStatuses(scenario.CancellationToken);

        /*
         * Assert
         */
        // Check existing Players
        mockQuerySender.Verify(x => x.Send(scenario.Query1, scenario.CancellationToken), Times.Once);
        mockQuerySender.Verify(x => x.Send(scenario.Query2, scenario.CancellationToken), Times.Once);
        mockQuerySender.Verify(x => x.Send(scenario.Query3, scenario.CancellationToken), Times.Once);

        // No create command for Player1
        mockCommandSender.Verify(
            x => x.Send(It.Is<CreatePlayerCommand>(c => c.PlayerStatus.MlbId == TestScenario.Player1MlbId),
                scenario.CancellationToken), Times.Never);
        // One create command for Player2
        mockCommandSender.Verify(x => x.Send(scenario.CreatePlayer2Command, scenario.CancellationToken), Times.Once);
        // No create command for Player3
        mockCommandSender.Verify(
            x => x.Send(It.Is<CreatePlayerCommand>(c => c.PlayerStatus.MlbId == TestScenario.Player3MlbId),
                scenario.CancellationToken), Times.Never);

        // No update command for Player1
        mockCommandSender.Verify(
            x => x.Send(It.Is<UpdatePlayerCommand>(c => c.Player.MlbId == TestScenario.Player1MlbId),
                scenario.CancellationToken), Times.Never);
        // No update command for Player2
        mockCommandSender.Verify(
            x => x.Send(It.Is<UpdatePlayerCommand>(c => c.Player.MlbId == TestScenario.Player2MlbId),
                scenario.CancellationToken), Times.Never);
        // One update command for Player3
        mockCommandSender.Verify(x => x.Send(scenario.UpdatePlayer3Command, scenario.CancellationToken), Times.Once);
    }

    private class TestScenario
    {
        public readonly CancellationToken CancellationToken = CancellationToken.None;
        public static readonly MlbId Player1MlbId = MlbId.Create(1);
        public static readonly MlbId Player2MlbId = MlbId.Create(2);
        public static readonly MlbId Player3MlbId = MlbId.Create(3);

        /// <summary>
        /// Provides a team when invoked
        /// </summary>
        public ITeamProvider StubTeamProvider { get; private set; } = null!;

        /// <summary>
        /// Returns the player states from the MLB roster
        /// </summary>
        public IPlayerRoster StubPlayerRoster { get; private set; } = null!;

        /// <summary>
        /// Detects the changes in player state between the system and the MLB
        /// </summary>
        public Mock<IPlayerStatusChangeDetector> StubPlayerStatusDetector { get; private set; } = null!;

        /// <summary>
        /// Sends queries
        /// Assert will ensure it does a query for each player returned from <see cref="IPlayerRoster"/>
        /// </summary>
        public Mock<IQuerySender> MockQuerySender { get; private set; } = null!;

        /// <summary>
        /// Query for Player1
        /// </summary>
        public GetPlayerByMlbIdQuery Query1 { get; private set; }

        /// <summary>
        /// Query for Player2
        /// </summary>
        public GetPlayerByMlbIdQuery Query2 { get; private set; }

        /// <summary>
        /// Query for Player3
        /// </summary>
        public GetPlayerByMlbIdQuery Query3 { get; private set; }

        /// <summary>
        /// Sends commands
        /// Assert will ensure it sends:
        /// - No commands for Player1
        /// - Create command for Player2
        /// - Update command for Player3
        /// </summary>
        public Mock<ICommandSender> MockCommandSender { get; private set; } = null!;

        /// <summary>
        /// The command for creating Player2
        /// </summary>
        public CreatePlayerCommand CreatePlayer2Command { get; private set; }

        /// <summary>
        /// The command for updating Player3
        /// </summary>
        public UpdatePlayerCommand UpdatePlayer3Command { get; private set; }


        public TestScenario()
        {
            Setup();
        }

        private void Setup()
        {
            // Fake Team
            var team = TeamFaker.Fake(PlayerStatusFaker.DefaultTeamMlbId);
            var otherTeam = TeamFaker.Fake(999);

            // Team Provider
            StubTeamProvider = Mock.Of<ITeamProvider>(x => x.GetBy(team.MlbId) == team);

            // Fake Players in the system
            Player player1 =
                PlayerFaker.Fake(Player1MlbId.Value, active: true, team: team); // Player has no status updates
            Player? player2 = null; // Player doesn't exist, so will be created
            Player player3 =
                PlayerFaker.Fake(Player3MlbId.Value, active: false, team: otherTeam); // Player will be updated

            // Roster
            var (playerStatus1, playerStatus2, playerStatus3) = SetupPlayerStatuses(team);

            // Queries
            SetupQuerySenderAndExpectedQueries(player1, player2, player3);

            // Player status changes
            var (player1Changes, player3Changes) = SetupPlayerChanges(team);

            // Status change detections
            SetupStubPlayerStatusDetector(player1, player3, team, playerStatus1, playerStatus3, player1Changes,
                player3Changes);

            // Commands
            SetupCommandSenderAndExpectedCommands(playerStatus2, player3, player3Changes);
        }

        /// <summary>
        /// Sets up the statuses that are reported from the MLB, or the <see cref="IPlayerRoster"/>
        /// </summary>
        private (Application.Dtos.PlayerStatus playerStatus1, Application.Dtos.PlayerStatus playerStatus2,
            Application.Dtos.PlayerStatus playerStatus3) SetupPlayerStatuses(Team team)
        {
            var playerStatus1 = PlayerStatusFaker.Fake(Player1MlbId.Value, active: true, teamMlbId: team.MlbId.Value);
            var playerStatus2 = PlayerStatusFaker.Fake(Player2MlbId.Value, active: true, teamMlbId: team.MlbId.Value);
            var playerStatus3 = PlayerStatusFaker.Fake(Player3MlbId.Value, active: true, teamMlbId: team.MlbId.Value);
            var playerStatuses = new List<Application.Dtos.PlayerStatus>
            {
                playerStatus1,
                playerStatus2,
                playerStatus3,
            };
            StubPlayerRoster = Mock.Of<IPlayerRoster>(x =>
                x.GetPlayerStatuses(CancellationToken) == Task.FromResult(playerStatuses.AsEnumerable()));
            return (playerStatus1, playerStatus2, playerStatus3);
        }

        /// <summary>
        /// Sets up the changes that occurred since our system last checked the MLB <see cref="IPlayerRoster"/>
        /// </summary>
        private (PlayerStatusChanges player1Changes, PlayerStatusChanges player3Changes) SetupPlayerChanges(Team team)
        {
            var player1Changes = new PlayerStatusChanges(new List<PlayerStatusChangeType>(), TeamFaker.NoTeam);
            var player3Changes = new PlayerStatusChanges(new List<PlayerStatusChangeType>
            {
                PlayerStatusChangeType.Activated,
                PlayerStatusChangeType.SignedContractWithTeam
            }, team);
            return (player1Changes, player3Changes);
        }

        /// <summary>
        /// Sets up how the Stub <see cref="IPlayerStatusChangeDetector"/> will behave
        /// </summary>
        private void SetupStubPlayerStatusDetector(Player player1, Player player3, Team team,
            Application.Dtos.PlayerStatus playerStatus1, Application.Dtos.PlayerStatus playerStatus3,
            PlayerStatusChanges player1Changes, PlayerStatusChanges player3Changes)
        {
            StubPlayerStatusDetector = new Mock<IPlayerStatusChangeDetector>();
            StubPlayerStatusDetector.Setup(x => x.DetectChanges(player1, playerStatus1.Active, team))
                .Returns(player1Changes);
            StubPlayerStatusDetector.Setup(x => x.DetectChanges(player3, playerStatus3.Active, team))
                .Returns(player3Changes);
        }

        /// <summary>
        /// Sets up how the <see cref="IQuerySender"/> will behave
        /// </summary>
        private void SetupQuerySenderAndExpectedQueries(Player player1, Player? player2, Player player3)
        {
            MockQuerySender = new Mock<IQuerySender>();
            Query1 = new GetPlayerByMlbIdQuery(Player1MlbId);
            Query2 = new GetPlayerByMlbIdQuery(Player2MlbId);
            Query3 = new GetPlayerByMlbIdQuery(Player3MlbId);
            MockQuerySender.Setup(x => x.Send(Query1, CancellationToken)).ReturnsAsync(player1);
            MockQuerySender.Setup(x => x.Send(Query2, CancellationToken)).ReturnsAsync(player2);
            MockQuerySender.Setup(x => x.Send(Query3, CancellationToken)).ReturnsAsync(player3);
        }

        /// <summary>
        /// Sets up how the <see cref="ICommandSender"/> will behave
        /// </summary>
        private void SetupCommandSenderAndExpectedCommands(Application.Dtos.PlayerStatus playerStatus2, Player player3,
            PlayerStatusChanges player3Changes)
        {
            MockCommandSender = new Mock<ICommandSender>();
            CreatePlayer2Command = new CreatePlayerCommand(playerStatus2);
            UpdatePlayer3Command = new UpdatePlayerCommand(player3, player3Changes);
        }
    }
}