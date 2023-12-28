using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Queries.GetPlayerByMlbId;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Queries.GetPlayerByMlbId;

public class GetPlayerByMlbIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_GetByMlbIdQuery_ReturnsPlayer()
    {
        // Arrange
        var mlbId = MlbId.Create(PlayerFaker.DefaultMlbId);
        var fakePlayer = PlayerFaker.Fake(mlbId: mlbId.Value);
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