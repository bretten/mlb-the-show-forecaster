namespace com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;

/// <summary>
/// Clock that provides the date and time
/// </summary>
public sealed class Clock : IClock
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow()
    {
        return DateTimeOffset.UtcNow;
    }

    /// <inheritdoc />
    public DateTimeOffset PstNow()
    {
        var tzInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzInfo);
    }
}