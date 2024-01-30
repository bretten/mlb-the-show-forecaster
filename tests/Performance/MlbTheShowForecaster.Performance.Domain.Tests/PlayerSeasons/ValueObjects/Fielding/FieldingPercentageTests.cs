using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Fielding;

public class FieldingPercentageTests
{
    [Fact]
    public void Value_FieldingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint assists = 276;
        const uint putOuts = 139;
        const uint errors = 8;
        var fieldingPercentage = FieldingPercentage.Create(assists, putOuts, errors);

        // Act
        var actual = fieldingPercentage.Value;

        // Assert
        Assert.Equal(0.981m, Math.Round(actual, 3, MidpointRounding.AwayFromZero));
    }
}