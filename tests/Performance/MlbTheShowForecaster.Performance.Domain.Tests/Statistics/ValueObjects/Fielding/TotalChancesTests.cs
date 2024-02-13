using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Fielding;

public class TotalChancesTests
{
    [Fact]
    public void Value_AssistsPutOutsErrors_ReturnsCalculatedValue()
    {
        // Arrange
        const int assists = 276;
        const int putOuts = 139;
        const int errors = 8;
        var totalChances = TotalChances.Create(assists, putOuts, errors);

        // Act
        var actual = totalChances.Value;

        // Assert
        Assert.Equal(423, actual);
        Assert.Equal(276, totalChances.Assists.Value);
        Assert.Equal(139, totalChances.PutOuts.Value);
        Assert.Equal(8, totalChances.Errors.Value);
    }
}