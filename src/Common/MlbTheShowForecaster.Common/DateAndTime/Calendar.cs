namespace com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;

/// <summary>
/// Calendar that provides dates
/// </summary>
public sealed class Calendar : ICalendar
{
    /// <inheritdoc />
    public DateOnly Today()
    {
        return DateOnly.FromDateTime(DateTime.Today);
    }

    /// <inheritdoc />
    public DateOnly TodayPst()
    {
        var tzInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");
        var offset = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tzInfo);
        return DateOnly.FromDateTime(offset.DateTime);
    }
}