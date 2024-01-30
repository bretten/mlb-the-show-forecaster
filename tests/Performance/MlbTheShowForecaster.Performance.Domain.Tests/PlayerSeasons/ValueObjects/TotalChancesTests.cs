using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class TotalChancesTests
{
    [Fact]
    public void Value_FieldingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint assists = 276;
        const uint putOuts = 139;
        const uint errors = 8;
        var totalChances = TotalChances.Create(assists, putOuts, errors);

        // Act
        var actual = totalChances.Value;

        // Assert
        Assert.Equal(423, Math.Round(actual, 0, MidpointRounding.AwayFromZero));
    }
}