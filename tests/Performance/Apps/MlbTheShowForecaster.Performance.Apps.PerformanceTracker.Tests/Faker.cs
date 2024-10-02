using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
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

    public static PlayerSeasonPerformanceMetrics FakePlayerSeasonPerformanceMetrics(ushort seasonYear = 2024,
        int mlbId = 100, List<PerformanceMetricsByDate>? metricsByDate = null)
    {
        return new PlayerSeasonPerformanceMetrics(
            Year: SeasonYear.Create(seasonYear),
            MlbId: MlbId.Create(mlbId),
            MetricsByDate: metricsByDate ?? new List<PerformanceMetricsByDate>());
    }

    public static PerformanceMetricsByDate FakePerformanceMetricByDate(DateOnly? date = null,
        decimal battingScore = 0.1m, bool significantBattingParticipation = false, decimal pitchingScore = 0.2m,
        bool significantPitchingParticipation = false, decimal fieldingScore = 0.3m,
        bool significantFieldingParticipation = false, decimal battingAverage = 1.1m, decimal onBasePercentage = 1.2m,
        decimal slugging = 1.3m, decimal earnedRunAverage = 1.4m, decimal opponentsBattingAverage = 1.5m,
        decimal strikeoutsPer9 = 1.6m, decimal baseOnBallsPer9 = 1.7m, decimal homeRunsPer9 = 1.8m,
        decimal fieldingPercentage = 1.9m)
    {
        return new PerformanceMetricsByDate(Date: date ?? new DateOnly(2024, 10, 2),
            BattingScore: battingScore,
            SignificantBattingParticipation: significantBattingParticipation,
            PitchingScore: pitchingScore,
            SignificantPitchingParticipation: significantPitchingParticipation,
            FieldingScore: fieldingScore,
            SignificantFieldingParticipation: significantFieldingParticipation,
            BattingAverage: battingAverage,
            OnBasePercentage: onBasePercentage,
            Slugging: slugging,
            EarnedRunAverage: earnedRunAverage,
            OpponentsBattingAverage: opponentsBattingAverage,
            StrikeoutsPer9: strikeoutsPer9,
            BaseOnBallsPer9: baseOnBallsPer9,
            HomeRunsPer9: homeRunsPer9,
            FieldingPercentage: fieldingPercentage
        );
    }

    public static PerformanceScore FakePerformanceScore(decimal score = 0.5m)
    {
        return PerformanceScore.Create(score);
    }
}