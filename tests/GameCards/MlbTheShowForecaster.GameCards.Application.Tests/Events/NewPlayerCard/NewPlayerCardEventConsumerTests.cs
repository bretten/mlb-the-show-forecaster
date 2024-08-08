using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCardForecast;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.NewPlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.NewPlayerCard;

public class NewPlayerCardEventConsumerTests
{
    [Fact]
    public async Task Handle_NewPlayerCardEvent_CreatesPlayerCardForecast()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var mlbId = MlbId.Create(1);
        var fakePlayerCard = Faker.FakePlayerCard();
        var mockCommandSender = new Mock<ICommandSender>();
        var stubPlayerMatcher = new Mock<IPlayerMatcher>();
        stubPlayerMatcher.Setup(x => x.GetPlayerByName(fakePlayerCard.Name))
            .ReturnsAsync(mlbId);

        var consumer = new NewPlayerCardEventConsumer(mockCommandSender.Object, stubPlayerMatcher.Object);

        var e = new NewPlayerCardEvent(fakePlayerCard);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        mockCommandSender.Verify(x =>
            x.Send(
                new CreatePlayerCardForecastCommand(fakePlayerCard.Year, fakePlayerCard.ExternalId,
                    fakePlayerCard.Position, fakePlayerCard.OverallRating, mlbId), cToken));
    }
}