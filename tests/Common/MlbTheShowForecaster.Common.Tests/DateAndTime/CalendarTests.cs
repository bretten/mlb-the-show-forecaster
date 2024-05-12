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
}