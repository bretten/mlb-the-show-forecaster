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
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();

        var consumer =
            new PlayerFreeAgencyEventConsumer(mockCommandSender.Object, stubCalendar.Object, stubImpactDuration);

        var e = new PlayerFreeAgencyEvent(Year, MlbId);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact = new PlayerFreeAgencyForecastImpact(stubCalendar.Object.Today(),
            stubCalendar.Object.Today().AddDays(stubImpactDuration.PlayerFreeAgency));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.MlbId == MlbId && y.Impacts[0] == expectedImpact), cToken), Times.Once);
    }
}