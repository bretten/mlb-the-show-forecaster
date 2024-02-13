using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Fielding;

public class FieldingPercentageTests
{
    [Fact]
    public void Value_AssistsPutOutsErrors_ReturnsCalculatedValue()
    {
        // Arrange
        const int assists = 276;
        const int putOuts = 139;
        const int errors = 8;
        var fieldingPercentage = FieldingPercentage.Create(assists, putOuts, errors);

        // Act
        var actual = fieldingPercentage.Value;

        // Assert
        Assert.Equal(0.981m, actual);
        Assert.Equal(276, fieldingPercentage.Assists.Value);
        Assert.Equal(139, fieldingPercentage.PutOuts.Value);
        Assert.Equal(8, fieldingPercentage.Errors.Value);
    }
}