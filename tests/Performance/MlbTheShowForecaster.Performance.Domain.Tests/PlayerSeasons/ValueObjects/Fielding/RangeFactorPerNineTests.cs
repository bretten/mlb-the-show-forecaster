using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Fielding;

public class RangeFactorPerNineTests
{
    [Fact]
    public void Value_FieldingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint assists = 276;
        const uint putOuts = 139;
        const decimal innings = 951.2m;
        var rangeFactorPerNine = RangeFactorPerNine.Create(assists, putOuts, innings);

        // Act
        var actual = rangeFactorPerNine.Value;

        // Assert
        Assert.Equal(3.92m, Math.Round(actual, 2, MidpointRounding.AwayFromZero));
    }
}