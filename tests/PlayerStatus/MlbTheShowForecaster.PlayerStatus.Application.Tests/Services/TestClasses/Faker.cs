using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services.Results;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Services.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerStatusTrackerResult FakePlayerStatusTrackerResult(int totalRosterEntries = 0,
        int totalNewPlayers = 0, int totalUpdatedPlayers = 0)
    {
        return new PlayerStatusTrackerResult(totalRosterEntries, totalNewPlayers, totalUpdatedPlayers);
    }
}