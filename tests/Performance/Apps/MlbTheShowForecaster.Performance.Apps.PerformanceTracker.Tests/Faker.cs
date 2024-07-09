using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Tests;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerStatsBySeason FakePlayerStatsBySeason(int playerMlbId = 1, ushort seasonYear = 2024,
        decimal battingScore = 0.5m, decimal pitchingScore = 0.5m, decimal fieldingScore = 0.5m,
        List<PlayerBattingStatsByGame>? battingStatsByGames = null,
        List<PlayerPitchingStatsByGame>? pitchingStatsByGames = null,
        List<PlayerFieldingStatsByGame>? fieldingStatsByGames = null)
    {
        return PlayerStatsBySeason.Create(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            battingScore: FakePerformanceScore(battingScore),
            pitchingScore: FakePerformanceScore(pitchingScore),
            fieldingScore: FakePerformanceScore(fieldingScore),
            battingStatsByGames ?? new List<PlayerBattingStatsByGame>(),
            pitchingStatsByGames ?? new List<PlayerPitchingStatsByGame>(),
            fieldingStatsByGames ?? new List<PlayerFieldingStatsByGame>()
        );
    }

    public static PerformanceScore FakePerformanceScore(decimal score = 0.5m)
    {
        return PerformanceScore.Create(score);
    }
}