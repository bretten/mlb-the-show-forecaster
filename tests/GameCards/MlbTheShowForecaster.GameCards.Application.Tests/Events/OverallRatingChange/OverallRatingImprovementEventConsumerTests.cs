using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.OverallRatingChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.OverallRatingChange;

public class OverallRatingImprovementEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_OverallRatingImprovementEvent_AppliesForecastImpact()
    {
        // Arrange
        var date = new DateOnly(2024, 10, 28);
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();

        var consumer =
            new OverallRatingImprovementEventConsumer(mockCommandSender.Object, stubCalendar.Object,
                stubImpactDuration);

        var oldRating = Faker.FakeOverallRating(70);
        var newRating = Faker.FakeOverallRating(90);
        var e = new OverallRatingImprovementEvent(Year, CardExternalId, NewOverallRating: newRating, oldRating, date);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact = new OverallRatingChangeForecastImpact(oldRating, newRating, date,
            date.AddDays(stubImpactDuration.OverallRatingChange));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.CardExternalId == CardExternalId && y.Impacts[0] == expectedImpact), cToken),
            Times.Once);
    }
}