using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerFreeAgency;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.PlayerFreeAgency;

public class PlayerFreeAgencyEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_PlayerFreeAgencyEvent_AppliesForecastImpact()
    {
        // Arrange
        var date = new DateOnly(2024, 10, 28);
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var mockTrendReporter = MockTrendReporter();
        var stubImpactDuration = StubImpactDuration();

        var consumer = new PlayerFreeAgencyEventConsumer(mockCommandSender.Object, mockTrendReporter.Object,
            stubImpactDuration);

        var e = new PlayerFreeAgencyEvent(Year, MlbId, date);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact =
            new PlayerFreeAgencyForecastImpact(date, date.AddDays(stubImpactDuration.PlayerFreeAgency));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.MlbId == MlbId && y.Impacts[0] == expectedImpact), cToken), Times.Once);
        mockTrendReporter.Verify(x => x.UpdateTrendReport(Year, MlbId, cToken), Times.Once);
    }
}