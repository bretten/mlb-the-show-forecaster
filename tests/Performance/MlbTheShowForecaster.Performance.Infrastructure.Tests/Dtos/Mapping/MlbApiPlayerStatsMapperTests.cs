using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests.Dtos.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests.Dtos.Mapping;

public class MlbApiPlayerStatsMapperTests
{
    [Fact]
    public void Map_PlayerSeasonStatsDto_ReturnsApplicationPlayerSeason()
    {
        // Arrange
        const int playerMlbApi = 1;

        var hittingStats = StatsDto.HittingStats(
            Faker.FakeGameHittingStatsDto(),
            Faker.FakeGameHittingStatsDto(stat: Faker.FakeBattingStatsDto(plateAppearances: 0)) // No PAs, ignored
        );
        var pitchingStats = StatsDto.PitchingStats(Faker.FakeGamePitchingStatsDto());
        var fieldingStats = StatsDto.FieldingStats(Faker.FakeGameFieldingStatsDto());

        var playerSeasonStats = new PlayerSeasonStatsByGameDto(playerMlbApi, "First", "Last", new List<StatsDto>()
        {
            hittingStats, pitchingStats, fieldingStats
        }) { SeasonYear = 2024 };

        var mapper = new MlbApiPlayerStatsMapper();

        // Act
        var actual = mapper.Map(playerSeasonStats);

        // Assert
        Assert.Equal(1, actual.PlayerMlbId.Value);
        Assert.Equal(2024, actual.SeasonYear.Value);

        Assert.Single(actual.GameBattingStats);
        Assert.Equal(1, actual.GameBattingStats[0].PlayerMlbId.Value);
        Assert.Equal(2024, actual.GameBattingStats[0].SeasonYear.Value);
        Assert.Equal(new DateOnly(2024, 4, 1), actual.GameBattingStats[0].GameDate);
        Assert.Equal(100, actual.GameBattingStats[0].GameMlbId.Value);
        Assert.Equal(10, actual.GameBattingStats[0].TeamMlbId.Value);
        Assert.Equal(2, actual.GameBattingStats[0].GroundOuts.Value);
        Assert.Equal(3, actual.GameBattingStats[0].AirOuts.Value);
        Assert.Equal(4, actual.GameBattingStats[0].Runs.Value);
        Assert.Equal(5, actual.GameBattingStats[0].Doubles.Value);
        Assert.Equal(6, actual.GameBattingStats[0].Triples.Value);
        Assert.Equal(7, actual.GameBattingStats[0].HomeRuns.Value);
        Assert.Equal(8, actual.GameBattingStats[0].Strikeouts.Value);
        Assert.Equal(9, actual.GameBattingStats[0].BaseOnBalls.Value);
        Assert.Equal(10, actual.GameBattingStats[0].IntentionalWalks.Value);
        Assert.Equal(11, actual.GameBattingStats[0].Hits.Value);
        Assert.Equal(12, actual.GameBattingStats[0].HitByPitch.Value);
        Assert.Equal(13, actual.GameBattingStats[0].AtBats.Value);
        Assert.Equal(14, actual.GameBattingStats[0].CaughtStealing.Value);
        Assert.Equal(15, actual.GameBattingStats[0].StolenBases.Value);
        Assert.Equal(16, actual.GameBattingStats[0].GroundIntoDoublePlays.Value);
        Assert.Equal(17, actual.GameBattingStats[0].GroundIntoTriplePlays.Value);
        Assert.Equal(18, actual.GameBattingStats[0].NumberOfPitchesSeen.Value);
        Assert.Equal(19, actual.GameBattingStats[0].PlateAppearances.Value);
        Assert.Equal(21, actual.GameBattingStats[0].RunsBattedIn.Value);
        Assert.Equal(22, actual.GameBattingStats[0].LeftOnBase.Value);
        Assert.Equal(23, actual.GameBattingStats[0].SacrificeBunts.Value);
        Assert.Equal(24, actual.GameBattingStats[0].SacrificeFlies.Value);
        Assert.Equal(25, actual.GameBattingStats[0].CatcherInterferences.Value);

        Assert.Single(actual.GamePitchingStats);
        Assert.Equal(1, actual.GamePitchingStats[0].PlayerMlbId.Value);
        Assert.Equal(2024, actual.GamePitchingStats[0].SeasonYear.Value);
        Assert.Equal(new DateOnly(2024, 4, 1), actual.GamePitchingStats[0].GameDate);
        Assert.Equal(100, actual.GamePitchingStats[0].GameMlbId.Value);
        Assert.Equal(10, actual.GamePitchingStats[0].TeamMlbId.Value);
        Assert.True(actual.GamePitchingStats[0].GameStarted);
        Assert.Equal(3, actual.GamePitchingStats[0].GroundOuts.Value);
        Assert.Equal(4, actual.GamePitchingStats[0].AirOuts.Value);
        Assert.Equal(5, actual.GamePitchingStats[0].Runs.Value);
        Assert.Equal(6, actual.GamePitchingStats[0].Doubles.Value);
        Assert.Equal(7, actual.GamePitchingStats[0].Triples.Value);
        Assert.Equal(8, actual.GamePitchingStats[0].HomeRuns.Value);
        Assert.Equal(9, actual.GamePitchingStats[0].Strikeouts.Value);
        Assert.Equal(10, actual.GamePitchingStats[0].BaseOnBalls.Value);
        Assert.Equal(11, actual.GamePitchingStats[0].IntentionalWalks.Value);
        Assert.Equal(12, actual.GamePitchingStats[0].Hits.Value);
        Assert.Equal(14, actual.GamePitchingStats[0].AtBats.Value);
        Assert.Equal(15, actual.GamePitchingStats[0].CaughtStealing.Value);
        Assert.Equal(16, actual.GamePitchingStats[0].StolenBases.Value);
        Assert.Equal(17, actual.GamePitchingStats[0].GroundIntoDoublePlays.Value);
        Assert.Equal(18, actual.GamePitchingStats[0].NumberOfPitches.Value);
        Assert.True(actual.GamePitchingStats[0].Win);
        Assert.True(actual.GamePitchingStats[0].Loss);
        Assert.True(actual.GamePitchingStats[0].Save);
        Assert.True(actual.GamePitchingStats[0].SaveOpportunity);
        Assert.True(actual.GamePitchingStats[0].Hold);
        Assert.True(actual.GamePitchingStats[0].BlownSave);
        Assert.Equal(25, actual.GamePitchingStats[0].EarnedRuns.Value);
        Assert.Equal(26, actual.GamePitchingStats[0].BattersFaced.Value);
        Assert.Equal(27, actual.GamePitchingStats[0].Outs.Value);
        Assert.True(actual.GamePitchingStats[0].CompleteGame);
        Assert.True(actual.GamePitchingStats[0].Shutout);
        Assert.Equal(31, actual.GamePitchingStats[0].Strikes.Value);
        Assert.Equal(32, actual.GamePitchingStats[0].HitBatsmen.Value);
        Assert.Equal(33, actual.GamePitchingStats[0].Balks.Value);
        Assert.Equal(34, actual.GamePitchingStats[0].WildPitches.Value);
        Assert.Equal(35, actual.GamePitchingStats[0].Pickoffs.Value);
        Assert.True(actual.GamePitchingStats[0].GameFinished);
        Assert.Equal(38, actual.GamePitchingStats[0].InheritedRunners.Value);
        Assert.Equal(39, actual.GamePitchingStats[0].InheritedRunnersScored.Value);
        Assert.Equal(40, actual.GamePitchingStats[0].CatcherInterferences.Value);
        Assert.Equal(41, actual.GamePitchingStats[0].SacrificeBunts.Value);
        Assert.Equal(42, actual.GamePitchingStats[0].SacrificeFlies.Value);

        Assert.Single(actual.GameFieldingStats);
        Assert.Equal(1, actual.GameFieldingStats[0].PlayerMlbId.Value);
        Assert.Equal(2024, actual.GameFieldingStats[0].SeasonYear.Value);
        Assert.Equal(new DateOnly(2024, 4, 1), actual.GameFieldingStats[0].GameDate);
        Assert.Equal(100, actual.GameFieldingStats[0].GameMlbId.Value);
        Assert.Equal(10, actual.GameFieldingStats[0].TeamMlbId.Value);
        Assert.True(actual.GameFieldingStats[0].GameStarted);
        Assert.Equal(3, actual.GameFieldingStats[0].CaughtStealing.Value);
        Assert.Equal(4, actual.GameFieldingStats[0].StolenBases.Value);
        Assert.Equal(5, actual.GameFieldingStats[0].Assists.Value);
        Assert.Equal(6, actual.GameFieldingStats[0].Putouts.Value);
        Assert.Equal(7, actual.GameFieldingStats[0].Errors.Value);
        Assert.Equal(10, actual.GameFieldingStats[0].PassedBalls.Value);
        Assert.Equal(11, actual.GameFieldingStats[0].DoublePlays.Value);
        Assert.Equal(12, actual.GameFieldingStats[0].TriplePlays.Value);
        Assert.Equal(13, actual.GameFieldingStats[0].CatcherInterferences.Value);
        Assert.Equal(14, actual.GameFieldingStats[0].WildPitches.Value);
        Assert.Equal(15, actual.GameFieldingStats[0].ThrowingErrors.Value);
        Assert.Equal(16, actual.GameFieldingStats[0].Pickoffs.Value);
    }
}