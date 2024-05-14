using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;

namespace com.brettnamba.MlbTheShowForecaster.Common.Tests.DateAndTime;

public class ClockTests
{
    [Fact]
    public void UtcNow_NoParameters_ReturnsUtcNow()
    {
        // Arrange
        var clock = new Clock();

        // Act
        var actual = clock.UtcNow();

        // Assert
        var timeDifference = DateTimeOffset.UtcNow - actual;
        Assert.InRange(timeDifference.TotalMilliseconds, 0, 50);
    }

    [Fact]
    public void PstNow_NoParameters_ReturnsPstNow()
    {
        // Arrange
        var clock = new Clock();

        // Act
        var actual = clock.PstNow();

        // Assert
        Console.WriteLine("PSTNOW " + actual.ToString());
        var pstNow = DateTime.UtcNow.AddHours(-7);
        Console.WriteLine("PSTNOW " + pstNow.ToString());
        var pstNowOffset = new DateTimeOffset(DateOnly.FromDateTime(pstNow), TimeOnly.FromDateTime(pstNow),
            TimeSpan.FromHours(-7));
        Console.WriteLine("PSTNOW " + pstNowOffset.ToString());
        var timeDifference = pstNowOffset - actual;
        Assert.InRange(timeDifference.TotalMilliseconds, 0, 50);
    }
}