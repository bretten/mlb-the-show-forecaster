using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class HitsPerNineTests
{
    [Fact]
    public void Value_HitsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const uint hitsAllowed = 85;
        const decimal inningsPitched = 132;
        var hitsPerNine = HitsPerNine.Create(hitsAllowed, inningsPitched);

        // Act
        var actual = hitsPerNine.Value;

        // Assert
        Assert.Equal(5.795m, actual);
        Assert.Equal(85U, hitsPerNine.HitsAllowed.Value);
        Assert.Equal(132, hitsPerNine.InningsPitched.Value);
    }
}