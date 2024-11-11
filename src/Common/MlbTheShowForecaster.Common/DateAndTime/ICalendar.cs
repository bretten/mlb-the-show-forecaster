namespace com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;

/// <summary>
/// Defines an interface for dates
/// </summary>
public interface ICalendar
{
    /// <summary>
    /// Today's date
    /// </summary>
    /// <returns>Today's date</returns>
    DateOnly Today();

    /// <summary>
    /// Today's date from the perspective of PST
    /// </summary>
    /// <returns>Today's date in the PST timezone</returns>
    DateOnly TodayPst();
}