using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PitchingStatsChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.PitchingStatsChange;

public class PitchingStatsDeclineEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_PitchingStatsDeclineEvent_AppliesForecastImpact()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();

        var consumer =
            new PitchingStatsDeclineEventConsumer(mockCommandSender.Object, stubCalendar.Object, stubImpactDuration);

        var e = new PitchingStatsDeclineEvent(Year, MlbId, PercentageChange.Create(0.7m, 0.5m));

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact = new PitchingStatsForecastImpact(0.7m, 0.5m,
            stubCalendar.Object.Today().AddDays(stubImpactDuration.PitchingStatsChange));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.MlbId == MlbId && y.Impacts[0] == expectedImpact), cToken), Times.Once);
    }
}