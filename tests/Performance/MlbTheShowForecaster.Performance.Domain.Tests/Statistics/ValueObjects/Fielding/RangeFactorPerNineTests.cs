﻿using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Fielding;

public class RangeFactorPerNineTests
{
    [Fact]
    public void Value_AssistsPutOutsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const uint assists = 276;
        const uint putOuts = 139;
        const decimal innings = 951.2m;
        var rangeFactorPerNine = RangeFactorPerNine.Create(assists, putOuts, innings);

        // Act
        var actual = rangeFactorPerNine.Value;

        // Assert
        Assert.Equal(3.925m, actual);
        Assert.Equal(276U, rangeFactorPerNine.Assists.Value);
        Assert.Equal(139U, rangeFactorPerNine.PutOuts.Value);
        Assert.Equal(951.667m, rangeFactorPerNine.Innings.Value);
    }
}