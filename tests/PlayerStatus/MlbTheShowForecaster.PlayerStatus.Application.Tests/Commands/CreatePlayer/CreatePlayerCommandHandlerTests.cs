using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.CreatePlayer;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Commands.CreatePlayer;

public class CreatePlayerCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatePlayerCommand_CreatesPlayer()
    {
        // Arrange
        var fakeTeam = Faker.FakeTeam();
        var fakeRosterEntry = TestClasses.Faker.FakeRosterEntry(teamMlbId: fakeTeam.MlbId.Value);
        var fakePlayer = Domain.Tests.Players.TestClasses.Faker.FakePlayer(active: true, team: fakeTeam);

        var mockPlayerRepository = Mock.Of<IPlayerRepository>();
        var stubUnitOfWork = new Mock<IUnitOfWork<IPlayerWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerRepository>())
            .Returns(mockPlayerRepository);

        var stubPlayerMapper = Mock.Of<IPlayerMapper>(x => x.Map(fakeRosterEntry) == fakePlayer);

        var cToken = CancellationToken.None;
        var command = new CreatePlayerCommand(fakeRosterEntry);
        var handler = new CreatePlayerCommandHandler(stubUnitOfWork.Object, stubPlayerMapper);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Assert.True(fakePlayer.Active);
        Assert.NotNull(fakePlayer.Team);
        Assert.Equal(fakeTeam, fakePlayer.Team);

        Mock.Get(mockPlayerRepository).Verify(x => x.Add(fakePlayer), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}