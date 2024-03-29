﻿using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class PitchesPerInningTests
{
    [Fact]
    public void Value_PitchesInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int numberOfPitches = 1993;
        const decimal inningsPitched = 117 + (decimal)2 / 3;
        var pitchesPerInning = PitchesPerInning.Create(numberOfPitches, inningsPitched);

        // Act
        var actual = pitchesPerInning.Value;

        // Assert
        Assert.Equal(16.938m, actual);
        Assert.Equal(1993, pitchesPerInning.NumberOfPitches.Value);
        Assert.Equal(117.667m, pitchesPerInning.InningsPitched.Value);
    }
}