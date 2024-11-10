using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.BattingStatsChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.BattingStatsChange;

public class BattingStatsImprovementEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_BattingStatsImprovementEvent_AppliesForecastImpact()
    {
        // Arrange
        var date = new DateOnly(2024, 10, 28);
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var mockTrendReporter = MockTrendReporter();
        var stubImpactDuration = StubImpactDuration();

        var consumer = new BattingStatsImprovementEventConsumer(mockCommandSender.Object, mockTrendReporter.Object,
            stubImpactDuration);

        var e = new BattingStatsImprovementEvent(Year, MlbId, PercentageChange.Create(0.5m, 0.7m), date);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact =
            new BattingStatsForecastImpact(0.5m, 0.7m, date, date.AddDays(stubImpactDuration.BattingStatsChange));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.MlbId == MlbId && y.Impacts[0] == expectedImpact), cToken), Times.Once);
        mockTrendReporter.Verify(x => x.UpdateTrendReport(Year, MlbId, cToken), Times.Once);
    }
}