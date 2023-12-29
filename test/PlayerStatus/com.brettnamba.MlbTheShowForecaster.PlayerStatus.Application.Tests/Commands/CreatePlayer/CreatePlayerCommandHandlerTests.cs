using com.brettnamba.MlbTheShowForecaster.Common.Application.Mapping;
using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.CreatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Commands.CreatePlayer;

public class CreatePlayerCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatePlayerCommand_CreatesPlayer()
    {
        // Arrange
        var fakeTeam = Faker.FakeTeam();
        var fakePlayerStatus = Faker.Fake(teamMlbId: fakeTeam.MlbId.Value);
        var fakePlayer = Faker.FakePlayer(active: false, team: null);

        var mockPlayerRepository = Mock.Of<IPlayerRepository>();
        var mockUnitOfWork = Mock.Of<IUnitOfWork>();

        var stubObjectMapper = Mock.Of<IObjectMapper>(x => x.Map<RosterEntry, Player>(fakePlayerStatus) == fakePlayer);

        var stubTeamProvider = new Mock<ITeamProvider>();
        stubTeamProvider.Setup(x => x.GetBy(fakeTeam.MlbId))
            .Returns(fakeTeam);

        var cToken = CancellationToken.None;
        var command = new CreatePlayerCommand(fakePlayerStatus);
        var handler = new CreatePlayerCommandHandler(mockPlayerRepository, mockUnitOfWork,
            stubObjectMapper, stubTeamProvider.Object);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Assert.True(fakePlayer.Active);
        Assert.NotNull(fakePlayer.Team);
        Assert.Equal(fakeTeam, fakePlayer.Team);

        Mock.Get(mockPlayerRepository).Verify(x => x.Add(fakePlayer), Times.Once);
        Mock.Get(mockUnitOfWork).Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}