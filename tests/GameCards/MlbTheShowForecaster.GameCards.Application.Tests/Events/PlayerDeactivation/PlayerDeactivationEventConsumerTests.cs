﻿using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerDeactivation;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Events.PlayerDeactivation;

public class PlayerDeactivationEventConsumerTests : BaseForecastImpactEventConsumerTests
{
    [Fact]
    public async Task Handle_PlayerDeactivationEvent_AppliesForecastImpact()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var mockCommandSender = MockCommandSender();
        var stubCalendar = StubCalendar();
        var stubImpactDuration = StubImpactDuration();

        var consumer =
            new PlayerDeactivationEventConsumer(mockCommandSender.Object, stubCalendar.Object, stubImpactDuration);

        var e = new PlayerDeactivationEvent(Year, MlbId);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedImpact = new PlayerDeactivationForecastImpact(stubCalendar.Object.Today(),
            stubCalendar.Object.Today().AddDays(stubImpactDuration.PlayerDeactivation));
        mockCommandSender.Verify(
            x => x.Send(
                It.Is<UpdatePlayerCardForecastImpactsCommand>(y =>
                    y.Year == Year && y.MlbId == MlbId && y.Impacts[0] == expectedImpact), cToken), Times.Once);
    }
}