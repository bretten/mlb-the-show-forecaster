using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.BattingStatsChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.BattingStatsChange;

public class BattingStatsDeclineEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_BattingStatsDeclineEvent_AppliesForecastImpact()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();

        var consumer =
            new BattingStatsDeclineEventConsumer(mockCommandSender.Object, stubCalendar.Object, stubImpactDuration);

        var e = new BattingStatsDeclineEvent(Year, MlbId, PercentageChange.Create(0.7m, 0.5m));

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact = new BattingStatsForecastImpact(0.7m, 0.5m, stubCalendar.Object.Today(),
            stubCalendar.Object.Today().AddDays(stubImpactDuration.BattingStatsChange));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.MlbId == MlbId && y.Impacts[0] == expectedImpact), cToken), Times.Once);
    }
}