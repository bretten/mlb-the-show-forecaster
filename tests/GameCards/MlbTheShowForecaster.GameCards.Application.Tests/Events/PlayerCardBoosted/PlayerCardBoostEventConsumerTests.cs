using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerCardBoosted;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.PlayerCardBoosted;

public class PlayerCardBoostEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_PlayerCardBoostedEvent_AppliesForecastImpact()
    {
        // Arrange
        var date = new DateOnly(2024, 10, 28);
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();

        var consumer =
            new PlayerCardBoostEventConsumer(mockCommandSender.Object, stubCalendar.Object, stubImpactDuration);

        var newRating = Faker.FakeOverallRating(90);
        const string boostReason = "Hit 5 HRs";
        var boostEndDate = date.AddDays(stubImpactDuration.Boost);
        var e = new PlayerCardBoostEvent(Year, CardExternalId, newRating, boostReason, boostEndDate, date);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact = new BoostForecastImpact(boostReason, date, boostEndDate);
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.CardExternalId == CardExternalId && y.Impacts[0] == expectedImpact), cToken),
            Times.Once);
    }
}