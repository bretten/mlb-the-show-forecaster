using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Commands.UpdatePlayer;

public class UpdatePlayerCommandHandlerTests
{
    [Fact]
    public async Task Handle_InactiveFreeAgentUpdatePlayerCommand_UpdatesPlayer()
    {
        // Arrange
        var fakeTeam = TeamFaker.Fake();
        var fakePlayer = PlayerFaker.Fake(active: true, team: fakeTeam); // Player's initial state
        var fakePlayerStatusChange = new PlayerStatusChanges(new List<PlayerStatusChangeType>() // Player's next state
        {
            PlayerStatusChangeType.Inactivated,
            PlayerStatusChangeType.EnteredFreeAgency
        }, TeamFaker.NoTeam);

        var mockPlayerRepository = Mock.Of<IPlayerRepository>();
        var mockUnitOfWork = Mock.Of<IUnitOfWork>();

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCommand(fakePlayer, fakePlayerStatusChange);
        var handler = new UpdatePlayerCommandHandler(mockPlayerRepository, mockUnitOfWork);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Assert.False(fakePlayer.Active);
        Assert.Null(fakePlayer.Team);

        Mock.Get(mockPlayerRepository).Verify(x => x.Update(fakePlayer), Times.Once);
        Mock.Get(mockUnitOfWork).Verify(x => x.CommitAsync(cToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ActiveNewTeamUpdatePlayerCommand_UpdatesPlayer()
    {
        // Arrange
        var fakeTeam = TeamFaker.Fake();
        var fakePlayer = PlayerFaker.Fake(active: false, team: TeamFaker.NoTeam); // Player's initial state
        var fakePlayerStatusChange = new PlayerStatusChanges(new List<PlayerStatusChangeType>() // Player's next state
        {
            PlayerStatusChangeType.Activated,
            PlayerStatusChangeType.SignedContractWithTeam
        }, fakeTeam);

        var mockPlayerRepository = Mock.Of<IPlayerRepository>();
        var mockUnitOfWork = Mock.Of<IUnitOfWork>();

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCommand(fakePlayer, fakePlayerStatusChange);
        var handler = new UpdatePlayerCommandHandler(mockPlayerRepository, mockUnitOfWork);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Assert.True(fakePlayer.Active);
        Assert.NotNull(fakePlayer.Team);
        Assert.Equal(fakeTeam, fakePlayer.Team);

        Mock.Get(mockPlayerRepository).Verify(x => x.Update(fakePlayer), Times.Once);
        Mock.Get(mockUnitOfWork).Verify(x => x.CommitAsync(cToken), Times.Once);
    }

    [Fact]
    public async Task Handle_NoTeamForSigningUpdatePlayerCommand_ThrowsException()
    {
        // Arrange
        var fakePlayer = PlayerFaker.Fake(active: false, team: TeamFaker.NoTeam); // Player's initial state
        var fakePlayerStatusChange = new PlayerStatusChanges(new List<PlayerStatusChangeType>() // Player's next state
        {
            PlayerStatusChangeType.Activated,
            PlayerStatusChangeType.SignedContractWithTeam
        }, TeamFaker.NoTeam); // <-- Cause of exception = No team specified in the status change

        var mockPlayerRepository = Mock.Of<IPlayerRepository>();
        var mockUnitOfWork = Mock.Of<IUnitOfWork>();

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCommand(fakePlayer, fakePlayerStatusChange);
        var handler = new UpdatePlayerCommandHandler(mockPlayerRepository, mockUnitOfWork);
        var action = async () => await handler.Handle(command, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<MissingTeamContractSigningException>(actual);

        Mock.Get(mockPlayerRepository).Verify(x => x.Update(fakePlayer), Times.Never);
        Mock.Get(mockUnitOfWork).Verify(x => x.CommitAsync(cToken), Times.Never);
    }
}