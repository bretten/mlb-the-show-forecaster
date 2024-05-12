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
}