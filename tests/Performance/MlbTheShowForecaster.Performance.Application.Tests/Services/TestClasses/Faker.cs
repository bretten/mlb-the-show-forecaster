using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services.Results;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Services.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PerformanceTrackerResult FakePerformanceTrackerResult(int totalPlayerSeasons = 0,
        int totalNewPlayerSeasons = 0, int totalPlayerSeasonUpdates = 0)
    {
        return new PerformanceTrackerResult(totalPlayerSeasons, totalNewPlayerSeasons, totalPlayerSeasonUpdates);
    }
}