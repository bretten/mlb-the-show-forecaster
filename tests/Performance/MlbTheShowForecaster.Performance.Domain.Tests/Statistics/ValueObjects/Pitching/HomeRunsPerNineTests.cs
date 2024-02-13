using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class HomeRunsPerNineTests
{
    [Fact]
    public void Value_HomeRunsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int homeRunsAllowed = 18;
        const decimal inningsPitched = 132;
        var homeRunsPerNine = HomeRunsPerNine.Create(homeRunsAllowed, inningsPitched);

        // Act
        var actual = homeRunsPerNine.Value;

        // Assert
        Assert.Equal(1.227m, actual);
        Assert.Equal(18, homeRunsPerNine.HomeRunsAllowed.Value);
        Assert.Equal(132, homeRunsPerNine.InningsPitched.Value);
    }
}