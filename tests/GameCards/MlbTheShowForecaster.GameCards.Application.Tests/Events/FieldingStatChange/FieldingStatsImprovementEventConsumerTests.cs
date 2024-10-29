using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.FieldingStatsChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.FieldingStatChange;

public class FieldingStatsImprovementEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_FieldingStatsImprovementEvent_AppliesForecastImpact()
    {
        // Arrange
        var date = new DateOnly(2024, 10, 28);
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();

        var consumer =
            new FieldingStatsImprovementEventConsumer(mockCommandSender.Object, stubCalendar.Object,
                stubImpactDuration);

        var e = new FieldingStatsImprovementEvent(Year, MlbId, PercentageChange.Create(0.5m, 0.7m), date);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact =
            new FieldingStatsForecastImpact(0.5m, 0.7m, date, date.AddDays(stubImpactDuration.FieldingStatsChange));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.MlbId == MlbId && y.Impacts[0] == expectedImpact), cToken), Times.Once);
    }
}