using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects;

public class RawStatTests
{
    [Fact]
    public void Create_RawValue_Created()
    {
        // Arrange
        const decimal rawValue = 3.14m;

        // Act
        var actual = RawStat.Create(rawValue);

        // Assert
        Assert.Equal(3.14m, actual.Value);
    }
}