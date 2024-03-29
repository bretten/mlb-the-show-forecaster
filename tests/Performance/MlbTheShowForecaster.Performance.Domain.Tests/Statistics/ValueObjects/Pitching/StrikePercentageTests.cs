﻿using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class StrikePercentageTests
{
    [Fact]
    public void Value_StrikesPitches_ReturnsValue()
    {
        // Arrange
        const int strikes = 1337;
        const int numberOfPitches = 2094;
        var strikePercentage = StrikePercentage.Create(strikes, numberOfPitches);

        // Act
        var actual = strikePercentage.Value;

        // Assert
        Assert.Equal(0.638m, actual);
        Assert.Equal(1337, strikePercentage.Strikes.Value);
        Assert.Equal(2094, strikePercentage.NumberOfPitches.Value);
    }
}