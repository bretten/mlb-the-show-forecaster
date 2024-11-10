using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PositionChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.PositionChange;

public class PositionChangeEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_PositionChangeEvent_AppliesForecastImpact()
    {
        // Arrange
        var date = new DateOnly(2024, 10, 28);
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var mockTrendReporter = MockTrendReporter();
        var stubImpactDuration = StubImpactDuration();

        var consumer = new PositionChangeEventConsumer(mockCommandSender.Object, mockTrendReporter.Object,
            stubImpactDuration);

        const Position oldPosition = Position.CenterField;
        const Position newPosition = Position.LeftField;
        var e = new PositionChangeEvent(Year, CardExternalId, NewPosition: newPosition, oldPosition, date);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact = new PositionChangeForecastImpact(oldPosition: oldPosition, newPosition, date,
            date.AddDays(stubImpactDuration.PlayerTeamSigning));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.CardExternalId == CardExternalId && y.Impacts[0] == expectedImpact), cToken),
            Times.Once);
        mockTrendReporter.Verify(x => x.UpdateTrendReport(Year, CardExternalId, cToken), Times.Once);
    }
}