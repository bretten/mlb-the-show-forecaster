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
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();

        var consumer =
            new FieldingStatsImprovementEventConsumer(mockCommandSender.Object, stubCalendar.Object,
                stubImpactDuration);

        var e = new FieldingStatsImprovementEvent(Year, MlbId, PercentageChange.Create(0.5m, 0.7m));

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact = new FieldingStatsForecastImpact(0.5m, 0.7m,stubCalendar.Object.Today(),
            stubCalendar.Object.Today().AddDays(stubImpactDuration.FieldingStatsChange));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.MlbId == MlbId && y.Impacts[0] == expectedImpact), cToken), Times.Once);
    }
}