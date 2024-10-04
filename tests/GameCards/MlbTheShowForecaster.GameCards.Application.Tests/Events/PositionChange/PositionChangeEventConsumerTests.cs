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
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();

        var consumer =
            new PositionChangeEventConsumer(mockCommandSender.Object, stubCalendar.Object, stubImpactDuration);

        const Position oldPosition = Position.CenterField;
        const Position newPosition = Position.LeftField;
        var e = new PositionChangeEvent(Year, CardExternalId, NewPosition: newPosition, oldPosition);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact = new PositionChangeForecastImpact(oldPosition: oldPosition, newPosition,
            stubCalendar.Object.Today(), stubCalendar.Object.Today().AddDays(stubImpactDuration.PlayerTeamSigning));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.CardExternalId == CardExternalId && y.Impacts[0] == expectedImpact), cToken),
            Times.Once);
    }
}