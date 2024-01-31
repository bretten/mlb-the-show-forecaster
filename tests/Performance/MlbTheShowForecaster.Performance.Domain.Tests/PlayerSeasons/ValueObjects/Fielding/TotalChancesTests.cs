using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Fielding;

public class TotalChancesTests
{
    [Fact]
    public void Value_AssistsPutOutsErrors_ReturnsCalculatedValue()
    {
        // Arrange
        const uint assists = 276;
        const uint putOuts = 139;
        const uint errors = 8;
        var totalChances = TotalChances.Create(assists, putOuts, errors);

        // Act
        var actual = totalChances.Value;

        // Assert
        Assert.Equal(423, actual);
        Assert.Equal(276U, totalChances.Assists.Value);
        Assert.Equal(139U, totalChances.PutOuts.Value);
        Assert.Equal(8U, totalChances.Errors.Value);
    }
}