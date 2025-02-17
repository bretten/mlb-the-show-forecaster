﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.CreatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Queries.GetPlayerByMlbId;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Services;

public class PlayerStatusTrackerTests
{
    [Fact]
    public async Task TrackPlayers_NoRosterEntries_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        var mockPlayerRoster = new Mock<IPlayerRoster>();
        mockPlayerRoster.Setup(x => x.GetRosterEntries(seasonYear, cToken))
            .ReturnsAsync(Enumerable.Empty<RosterEntry>);

        var mockQuerySender = Mock.Of<IQuerySender>();
        var mockCommandSender = Mock.Of<ICommandSender>();
        var mockPlayerChangeDetector = Mock.Of<IPlayerStatusChangeDetector>();
        var mockTeamProvider = Mock.Of<ITeamProvider>();
        var mockCalendar = Mock.Of<ICalendar>();

        var tracker = new PlayerStatusTracker(mockPlayerRoster.Object, mockQuerySender, mockCommandSender,
            mockPlayerChangeDetector, mockTeamProvider, mockCalendar);

        var action = () => tracker.TrackPlayers(seasonYear, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerStatusTrackerFoundNoRosterEntriesException>(actual);
    }

    [Fact]
    public async Task TrackPlayers_NewAndExistingPlayers_CreatesAndUpdates()
    {
        // Arrange
        var scenario = new TestScenario();
        var stubPlayerRoster = scenario.StubPlayerRoster;
        var stubPlayerStatusDetector = scenario.StubPlayerStatusChangeDetector;
        var mockQuerySender = scenario.MockQuerySender;
        var mockCommandSender = scenario.MockCommandSender;
        var stubCalendar = scenario.StubCalendar;

        var tracker = new PlayerStatusTracker(stubPlayerRoster, mockQuerySender.Object,
            mockCommandSender.Object, stubPlayerStatusDetector.Object, scenario.StubTeamProvider, stubCalendar.Object);

        // Act
        var actual = await tracker.TrackPlayers(TestScenario.SeasonYear, scenario.CancellationToken);

        /*
         * Assert
         */
        // There were 3 roster entries
        Assert.Equal(3, actual.TotalRosterEntries);
        // Player2 was created
        Assert.Equal(1, actual.TotalNewPlayers);
        // Player3 was updated
        Assert.Equal(1, actual.TotalUpdatedPlayers);
        // Player1 was unchanged
        Assert.Equal(1, actual.TotalUnchangedPlayers);

        // Check existing Players
        mockQuerySender.Verify(x => x.Send(scenario.Query1, scenario.CancellationToken), Times.Once);
        mockQuerySender.Verify(x => x.Send(scenario.Query2, scenario.CancellationToken), Times.Once);
        mockQuerySender.Verify(x => x.Send(scenario.Query3, scenario.CancellationToken), Times.Once);

        // No create command for Player1
        mockCommandSender.Verify(
            x => x.Send(It.Is<CreatePlayerCommand>(c => c.RosterEntry.MlbId == TestScenario.Player1MlbId),
                scenario.CancellationToken), Times.Never);
        // One create command for Player2
        mockCommandSender.Verify(x => x.Send(scenario.CreatePlayer2Command, scenario.CancellationToken), Times.Once);
        // No create command for Player3
        mockCommandSender.Verify(
            x => x.Send(It.Is<CreatePlayerCommand>(c => c.RosterEntry.MlbId == TestScenario.Player3MlbId),
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
        public static readonly SeasonYear SeasonYear = SeasonYear.Create(2023);

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
        public Mock<IPlayerStatusChangeDetector> StubPlayerStatusChangeDetector { get; private set; } = null!;

        /// <summary>
        /// Sends queries
        /// Assert will ensure it does a query for each player returned from <see cref="IPlayerRoster"/>
        /// </summary>
        public Mock<IQuerySender> MockQuerySender { get; private set; } = null!;

        /// <summary>
        /// Calendar for getting date
        /// </summary>
        public Mock<ICalendar> StubCalendar { get; private set; } = null!;

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
            // Calendar
            StubCalendar = new Mock<ICalendar>();
            StubCalendar.Setup(x => x.Today())
                .Returns(new DateOnly(2024, 10, 28));

            // Fake Team
            var team = Faker.FakeTeam(Tests.TestClasses.Faker.DefaultTeamMlbId);
            var otherTeam = Faker.FakeTeam(999);

            // Team Provider
            StubTeamProvider = Mock.Of<ITeamProvider>(x => x.GetBy(team.MlbId) == team);

            // Fake Players in the system
            // Player has no status updates
            Player player1 =
                Domain.Tests.Players.TestClasses.Faker.FakePlayer(Player1MlbId.Value, active: true, team: team);
            // Player doesn't exist, so will be created
            Player? player2 = null;
            // Player will be updated
            Player player3 =
                Domain.Tests.Players.TestClasses.Faker.FakePlayer(Player3MlbId.Value, active: false, team: otherTeam);

            // Roster
            var (rosterEntry1, rosterEntry2, rosterEntry3) = SetupRosterEntries(team);

            // Queries
            SetupQuerySenderAndExpectedQueries(player1, player2, player3);

            // Player status changes
            var (player1Changes, player3Changes) = SetupPlayerChanges(team);

            // Status change detections
            SetupStubPlayerStatusDetector(player1, player3, team, rosterEntry1, rosterEntry3, player1Changes,
                player3Changes);

            // Commands
            SetupCommandSenderAndExpectedCommands(rosterEntry2, player3, player3Changes);
        }

        /// <summary>
        /// Sets up the roster entries that are reported from the <see cref="IPlayerRoster"/>
        /// </summary>
        private (RosterEntry rosterEntry1, RosterEntry rosterEntry2, RosterEntry rosterEntry3)
            SetupRosterEntries(Team team)
        {
            var rosterEntry1 =
                Tests.TestClasses.Faker.FakeRosterEntry(Player1MlbId.Value, active: true, teamMlbId: team.MlbId.Value);
            var rosterEntry2 =
                Tests.TestClasses.Faker.FakeRosterEntry(Player2MlbId.Value, active: true, teamMlbId: team.MlbId.Value);
            var rosterEntry3 =
                Tests.TestClasses.Faker.FakeRosterEntry(Player3MlbId.Value, active: true, teamMlbId: team.MlbId.Value);
            var rosterEntries = new List<RosterEntry>
            {
                rosterEntry1,
                rosterEntry2,
                rosterEntry3,
            };
            StubPlayerRoster = Mock.Of<IPlayerRoster>(x =>
                x.GetRosterEntries(SeasonYear, CancellationToken) == Task.FromResult(rosterEntries.AsEnumerable()));
            return (rosterEntry1, rosterEntry2, rosterEntry3);
        }

        /// <summary>
        /// Sets up the changes that occurred since our system last checked the MLB <see cref="IPlayerRoster"/>
        /// </summary>
        private (PlayerStatusChanges player1Changes, PlayerStatusChanges player3Changes) SetupPlayerChanges(Team team)
        {
            var player1Changes = new PlayerStatusChanges(new List<PlayerStatusChangeType>(), Faker.NoTeam);
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
        private void SetupStubPlayerStatusDetector(Player player1, Player player3, Team team, RosterEntry rosterEntry1,
            RosterEntry rosterEntry3, PlayerStatusChanges player1Changes, PlayerStatusChanges player3Changes)
        {
            StubPlayerStatusChangeDetector = new Mock<IPlayerStatusChangeDetector>();
            StubPlayerStatusChangeDetector.Setup(x => x.DetectChanges(player1, rosterEntry1.Active, team))
                .Returns(player1Changes);
            StubPlayerStatusChangeDetector.Setup(x => x.DetectChanges(player3, rosterEntry3.Active, team))
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
        private void SetupCommandSenderAndExpectedCommands(RosterEntry rosterEntry2, Player player3,
            PlayerStatusChanges player3Changes)
        {
            MockCommandSender = new Mock<ICommandSender>();
            CreatePlayer2Command = new CreatePlayerCommand(rosterEntry2);
            UpdatePlayer3Command =
                new UpdatePlayerCommand(SeasonYear, player3, player3Changes, StubCalendar.Object.Today());
        }
    }
}