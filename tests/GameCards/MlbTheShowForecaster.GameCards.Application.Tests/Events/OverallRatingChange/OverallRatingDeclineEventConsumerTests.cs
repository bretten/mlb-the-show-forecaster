using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.OverallRatingChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.OverallRatingChange;

public sealed class OverallRatingDeclineEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_OverallRatingDeclineEvent_AppliesForecastImpact()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();
        var mockPublisher = MockForecastReportPublisher();

        var consumer = new OverallRatingDeclineEventConsumer(mockCommandSender.Object, stubCalendar.Object,
            stubImpactDuration, mockPublisher.Object);

        var oldRating = Faker.FakeOverallRating(70);
        var newRating = Faker.FakeOverallRating(90);
        var e = new OverallRatingDeclineEvent(Year, CardExternalId, NewOverallRating: newRating, oldRating);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact = new OverallRatingChangeForecastImpact(oldRating, newRating,
            stubCalendar.Object.Today().AddDays(stubImpactDuration.OverallRatingChange));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.CardExternalId == CardExternalId && y.Impacts[0] == expectedImpact), cToken),
            Times.Once);
        mockPublisher.Verify(x => x.Publish(Year, stubCalendar.Object.Today()), Times.Once);
    }
}