using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerTeamSigning;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.PlayerTeamSigning;

public class PlayerTeamSigningEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_PlayerTeamSigningEvent_AppliesForecastImpact()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();
        var mockPublisher = MockForecastReportPublisher();

        var consumer = new PlayerTeamSigningEventConsumer(mockCommandSender.Object, stubCalendar.Object,
            stubImpactDuration, mockPublisher.Object);

        var e = new PlayerTeamSigningEvent(Year, MlbId);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact =
            new PlayerTeamSigningForecastImpact(stubCalendar.Object.Today()
                .AddDays(stubImpactDuration.PlayerTeamSigning));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.MlbId == MlbId && y.Impacts[0] == expectedImpact), cToken), Times.Once);
        mockPublisher.Verify(x => x.Publish(Year, stubCalendar.Object.Today()), Times.Once);
    }
}