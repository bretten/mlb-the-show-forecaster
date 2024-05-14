namespace com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;

/// <summary>
/// Defines an interface for date and time
/// </summary>
public interface IClock
{
    /// <summary>
    /// Current UTC date and time
    /// </summary>
    /// <returns>Current UTC date and time</returns>
    DateTimeOffset UtcNow();

    /// <summary>
    /// Current PST date and time
    /// </summary>
    /// <returns>Current PST date and time</returns>
    DateTimeOffset PstNow();
}