using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Queries.GetPlayerByMlbId;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Queries.GetPlayerByMlbId;

public class GetPlayerByMlbIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_GetByMlbIdQuery_ReturnsPlayer()
    {
        // Arrange
        var mlbId = MlbId.Create(Faker.DefaultMlbId);
        var fakePlayer = Domain.Tests.Players.TestClasses.Faker.FakePlayer(mlbId: mlbId.Value);
        var mockPlayerRepository = new Mock<IPlayerRepository>();
        mockPlayerRepository.Setup(x => x.GetByMlbId(mlbId))
            .ReturnsAsync(fakePlayer);

        var cToken = CancellationToken.None;
        var command = new GetPlayerByMlbIdQuery(mlbId);
        var handler = new GetPlayerByMlbIdQueryHandler(mockPlayerRepository.Object);

        // Act
        var actual = await handler.Handle(command, cToken);

        // Assert
        mockPlayerRepository.Verify(x => x.GetByMlbId(mlbId), Times.Once);
        Assert.Equal(fakePlayer, actual);
    }
}