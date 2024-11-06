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
        var tzInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");

        // Act
        var actual = clock.PstNow();

        // Assert
        var pstNowOffset = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tzInfo);
        var timeDifference = pstNowOffset - actual;
        Assert.InRange(timeDifference.TotalMilliseconds, 0, 50);
    }
}