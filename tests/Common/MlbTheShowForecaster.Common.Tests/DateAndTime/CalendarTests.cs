using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;

namespace com.brettnamba.MlbTheShowForecaster.Common.Tests.DateAndTime;

public class CalendarTests
{
    [Fact]
    public void Today_NoParameters_ReturnsToday()
    {
        // Arrange
        var calendar = new Calendar();

        // Act
        var actual = calendar.Today();

        // Assert
        Assert.Equal(DateOnly.FromDateTime(DateTime.Today), actual);
    }

    [Fact]
    public void TodayPst_NoParameters_ReturnsToday()
    {
        // Arrange
        var calendar = new Calendar();

        // Act
        var actual = calendar.TodayPst();

        // Assert
        Assert.Equal(DateOnly.FromDateTime(DateTime.Today), actual);
    }
}