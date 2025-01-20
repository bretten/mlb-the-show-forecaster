using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Commands.UpdatePlayer;

public class UpdatePlayerCommandHandlerTests
{
    [Fact]
    public async Task Handle_InactiveFreeAgentUpdatePlayerCommand_UpdatesPlayer()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var fakeTeam = Faker.FakeTeam();
        var fakePlayer = Domain.Tests.Players.TestClasses.Faker.FakePlayer(active: true,
            team: fakeTeam); // Player's initial state
        var fakePlayerStatusChange = new PlayerStatusChanges(new List<PlayerStatusChangeType>() // Player's next state
        {
            PlayerStatusChangeType.Inactivated,
            PlayerStatusChangeType.EnteredFreeAgency
        }, Faker.NoTeam);

        var mockPlayerRepository = Mock.Of<IPlayerRepository>();
        var stubUnitOfWork = new Mock<IUnitOfWork<IPlayerWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerRepository>())
            .Returns(mockPlayerRepository);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCommand(year, fakePlayer, fakePlayerStatusChange, new DateOnly(2024, 10, 28));
        var handler = new UpdatePlayerCommandHandler(stubUnitOfWork.Object);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Assert.False(fakePlayer.Active);
        Assert.Null(fakePlayer.Team);

        Mock.Get(mockPlayerRepository).Verify(x => x.Update(fakePlayer), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ActiveNewTeamUpdatePlayerCommand_UpdatesPlayer()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var fakeTeam = Faker.FakeTeam();
        var fakePlayer = Domain.Tests.Players.TestClasses.Faker.FakePlayer(active: false,
            team: Faker.NoTeam); // Player's initial state
        var fakePlayerStatusChange = new PlayerStatusChanges(new List<PlayerStatusChangeType>() // Player's next state
        {
            PlayerStatusChangeType.Activated,
            PlayerStatusChangeType.SignedContractWithTeam
        }, fakeTeam);

        var mockPlayerRepository = Mock.Of<IPlayerRepository>();
        var stubUnitOfWork = new Mock<IUnitOfWork<IPlayerWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerRepository>())
            .Returns(mockPlayerRepository);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCommand(year, fakePlayer, fakePlayerStatusChange, new DateOnly(2024, 10, 28));
        var handler = new UpdatePlayerCommandHandler(stubUnitOfWork.Object);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Assert.True(fakePlayer.Active);
        Assert.NotNull(fakePlayer.Team);
        Assert.Equal(fakeTeam, fakePlayer.Team);

        Mock.Get(mockPlayerRepository).Verify(x => x.Update(fakePlayer), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }

    [Fact]
    public async Task Handle_NoTeamForSigningUpdatePlayerCommand_ThrowsException()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var fakePlayer = Domain.Tests.Players.TestClasses.Faker.FakePlayer(active: false,
            team: Faker.NoTeam); // Player's initial state
        var fakePlayerStatusChange = new PlayerStatusChanges(new List<PlayerStatusChangeType>() // Player's next state
        {
            PlayerStatusChangeType.Activated,
            PlayerStatusChangeType.SignedContractWithTeam
        }, Faker.NoTeam); // <-- Cause of exception = No team specified in the status change

        var mockPlayerRepository = Mock.Of<IPlayerRepository>();
        var stubUnitOfWork = new Mock<IUnitOfWork<IPlayerWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerRepository>())
            .Returns(mockPlayerRepository);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCommand(year, fakePlayer, fakePlayerStatusChange, new DateOnly(2024, 10, 28));
        var handler = new UpdatePlayerCommandHandler(stubUnitOfWork.Object);
        var action = async () => await handler.Handle(command, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<MissingTeamContractSigningException>(actual);

        Mock.Get(mockPlayerRepository).Verify(x => x.Update(fakePlayer), Times.Never);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Never);
    }
}