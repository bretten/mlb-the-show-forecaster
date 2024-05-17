using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.UpdatePlayerCard;

public class UpdatePlayerCardCommandHandlerTests
{
    [Fact]
    public async Task Handle_UpdatePlayerCardCommand_UpdatesPlayerCard()
    {
        // Arrange
        var playerCard = Faker.FakePlayerCard(overallRating: 80, position: Position.RightField);
        var ratingChange = Dtos.TestClasses.Faker.FakePlayerRatingChange(newOverallRating: 90);
        var positionChange = Dtos.TestClasses.Faker.FakePlayerPositionChange(newPosition: Position.FirstBase);

        var mockPlayerCardRepository = Mock.Of<IPlayerCardRepository>();
        var stubUnitOfWork = new Mock<IUnitOfWork<ICardWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerCardRepository>())
            .Returns(mockPlayerCardRepository);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardCommand(playerCard, ratingChange, positionChange);
        var handler = new UpdatePlayerCardCommandHandler(stubUnitOfWork.Object);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockPlayerCardRepository).Verify(x => x.Update(playerCard), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
        Assert.Equal(90, playerCard.OverallRating.Value);
        Assert.Equal(Position.FirstBase, playerCard.Position);
    }
}