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
}