using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.Mapping;

public class PlayerSeasonMapperTests
{
    [Fact]
    public void Map_PlayerSeason_ReturnsPlayerStatsBySeason()
    {
        // Arrange
        const int playerMlbId = 1;
        const int seasonYear = 2024;
        var battingGame1 = Faker.FakePlayerGameBattingStats(scalar: 1);
        var battingGame2 = Faker.FakePlayerGameBattingStats(scalar: 1000, gameDate: new DateOnly(2024, 5, 1));
        var pitchingGame1 = Faker.FakePlayerGamePitchingStats(scalar: 1, win: true, gameStarted: true, shutout: true,
            completeGame: true);
        var pitchingGame2 = Faker.FakePlayerGamePitchingStats(scalar: 1000, gameDate: new DateOnly(2024, 5, 1),
            loss: true, gameFinished: true, blownSave: true, saveOpportunity: true);
        var fieldingGame1 = Faker.FakePlayerGameFieldingStats(scalar: 1, gameStarted: true);
        var fieldingGame2 = Faker.FakePlayerGameFieldingStats(scalar: 1000, gameDate: new DateOnly(2024, 5, 1));
        var playerSeason = new PlayerSeason(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            new List<PlayerGameBattingStats>() { battingGame1, battingGame2 },
            new List<PlayerGamePitchingStats>() { pitchingGame1, pitchingGame2 },
            new List<PlayerGameFieldingStats>() { fieldingGame1, fieldingGame2 }
        );

        var stubPerformanceAssessor = new Mock<IPerformanceAssessor>();
        stubPerformanceAssessor.Setup(x => x.AssessBatting(It.IsAny<BattingStats>()))
            .Returns(Tests.TestClasses.Faker.FakePerformanceScore(0.1m));
        stubPerformanceAssessor.Setup(x => x.AssessPitching(It.IsAny<PitchingStats>()))
            .Returns(Tests.TestClasses.Faker.FakePerformanceScore(0.2m));
        stubPerformanceAssessor.Setup(x => x.AssessFielding(It.IsAny<FieldingStats>()))
            .Returns(Tests.TestClasses.Faker.FakePerformanceScore(0.3m));

        var mapper = new PlayerSeasonMapper(stubPerformanceAssessor.Object);

        // Act
        var actual = mapper.Map(playerSeason);

        // Assert
        Assert.Equal(1, actual.PlayerMlbId.Value);
        Assert.Equal(2024, actual.SeasonYear.Value);
        Assert.Equal(0.1m, actual.BattingScore.Value);
        Assert.Equal(0.2m, actual.PitchingScore.Value);
        Assert.Equal(0.3m, actual.FieldingScore.Value);
        Assert.Equal(2, actual.BattingStatsByGamesChronologically.Count);
        Assert.Equal(2, actual.PitchingStatsByGamesChronologically.Count);
        Assert.Equal(2, actual.FieldingStatsByGamesChronologically.Count);

        Assert.Equal(1001, actual.SeasonBattingStats.PlateAppearances.Value);
        Assert.Equal(2002, actual.SeasonBattingStats.AtBats.Value);
        Assert.Equal(3003, actual.SeasonBattingStats.Runs.Value);
        Assert.Equal(4004, actual.SeasonBattingStats.Hits.Value);
        Assert.Equal(5005, actual.SeasonBattingStats.Doubles.Value);
        Assert.Equal(6006, actual.SeasonBattingStats.Triples.Value);
        Assert.Equal(7007, actual.SeasonBattingStats.HomeRuns.Value);
        Assert.Equal(8008, actual.SeasonBattingStats.RunsBattedIn.Value);
        Assert.Equal(9009, actual.SeasonBattingStats.BaseOnBalls.Value);
        Assert.Equal(10010, actual.SeasonBattingStats.IntentionalWalks.Value);
        Assert.Equal(11011, actual.SeasonBattingStats.Strikeouts.Value);
        Assert.Equal(12012, actual.SeasonBattingStats.StolenBases.Value);
        Assert.Equal(13013, actual.SeasonBattingStats.CaughtStealing.Value);
        Assert.Equal(14014, actual.SeasonBattingStats.HitByPitches.Value);
        Assert.Equal(15015, actual.SeasonBattingStats.SacrificeBunts.Value);
        Assert.Equal(16016, actual.SeasonBattingStats.SacrificeFlies.Value);
        Assert.Equal(17017, actual.SeasonBattingStats.NumberOfPitchesSeen.Value);
        Assert.Equal(18018, actual.SeasonBattingStats.LeftOnBase.Value);
        Assert.Equal(19019, actual.SeasonBattingStats.GroundOuts.Value);
        Assert.Equal(20020, actual.SeasonBattingStats.GroundIntoDoublePlays.Value);
        Assert.Equal(21021, actual.SeasonBattingStats.GroundIntoTriplePlays.Value);
        Assert.Equal(22022, actual.SeasonBattingStats.AirOuts.Value);
        Assert.Equal(23023, actual.SeasonBattingStats.CatcherInterferences.Value);

        Assert.Equal(1, actual.SeasonPitchingStats.Wins.Value);
        Assert.Equal(1, actual.SeasonPitchingStats.Losses.Value);
        Assert.Equal(1, actual.SeasonPitchingStats.GamesStarted.Value);
        Assert.Equal(1, actual.SeasonPitchingStats.GamesFinished.Value);
        Assert.Equal(1, actual.SeasonPitchingStats.CompleteGames.Value);
        Assert.Equal(1, actual.SeasonPitchingStats.Shutouts.Value);
        Assert.Equal(0, actual.SeasonPitchingStats.Holds.Value);
        Assert.Equal(0, actual.SeasonPitchingStats.Saves.Value);
        Assert.Equal(1, actual.SeasonPitchingStats.BlownSaves.Value);
        Assert.Equal(1, actual.SeasonPitchingStats.SaveOpportunities.Value);
        Assert.Equal(1001, actual.SeasonPitchingStats.InningsPitched.Value);
        Assert.Equal(2002, actual.SeasonPitchingStats.Hits.Value);
        Assert.Equal(3003, actual.SeasonPitchingStats.Doubles.Value);
        Assert.Equal(4004, actual.SeasonPitchingStats.Triples.Value);
        Assert.Equal(5005, actual.SeasonPitchingStats.HomeRuns.Value);
        Assert.Equal(6006, actual.SeasonPitchingStats.Runs.Value);
        Assert.Equal(7007, actual.SeasonPitchingStats.EarnedRuns.Value);
        Assert.Equal(8008, actual.SeasonPitchingStats.Strikeouts.Value);
        Assert.Equal(9009, actual.SeasonPitchingStats.BaseOnBalls.Value);
        Assert.Equal(10010, actual.SeasonPitchingStats.IntentionalWalks.Value);
        Assert.Equal(11011, actual.SeasonPitchingStats.HitBatsmen.Value);
        Assert.Equal(12012, actual.SeasonPitchingStats.Outs.Value);
        Assert.Equal(13013, actual.SeasonPitchingStats.GroundOuts.Value);
        Assert.Equal(14014, actual.SeasonPitchingStats.AirOuts.Value);
        Assert.Equal(15015, actual.SeasonPitchingStats.GroundIntoDoublePlays.Value);
        Assert.Equal(16016, actual.SeasonPitchingStats.NumberOfPitches.Value);
        Assert.Equal(17017, actual.SeasonPitchingStats.Strikes.Value);
        Assert.Equal(18018, actual.SeasonPitchingStats.WildPitches.Value);
        Assert.Equal(19019, actual.SeasonPitchingStats.Balks.Value);
        Assert.Equal(20020, actual.SeasonPitchingStats.BattersFaced.Value);
        Assert.Equal(21021, actual.SeasonPitchingStats.AtBats.Value);
        Assert.Equal(22022, actual.SeasonPitchingStats.StolenBases.Value);
        Assert.Equal(23023, actual.SeasonPitchingStats.CaughtStealing.Value);
        Assert.Equal(24024, actual.SeasonPitchingStats.Pickoffs.Value);
        Assert.Equal(25025, actual.SeasonPitchingStats.InheritedRunners.Value);
        Assert.Equal(26026, actual.SeasonPitchingStats.InheritedRunnersScored.Value);
        Assert.Equal(27027, actual.SeasonPitchingStats.CatcherInterferences.Value);
        Assert.Equal(28028, actual.SeasonPitchingStats.SacrificeBunts.Value);
        Assert.Equal(29029, actual.SeasonPitchingStats.SacrificeFlies.Value);

        Assert.Equal(Position.Catcher, actual.SeasonFieldingStats.Position);
        Assert.Equal(1, actual.SeasonFieldingStats.GamesStarted.Value);
        Assert.Equal(1001, actual.SeasonFieldingStats.InningsPlayed.Value);
        Assert.Equal(2002, actual.SeasonFieldingStats.Assists.Value);
        Assert.Equal(3003, actual.SeasonFieldingStats.Putouts.Value);
        Assert.Equal(4004, actual.SeasonFieldingStats.Errors.Value);
        Assert.Equal(5005, actual.SeasonFieldingStats.ThrowingErrors.Value);
        Assert.Equal(6006, actual.SeasonFieldingStats.DoublePlays.Value);
        Assert.Equal(7007, actual.SeasonFieldingStats.TriplePlays.Value);
        Assert.Equal(8008, actual.SeasonFieldingStats.CaughtStealing.Value);
        Assert.Equal(9009, actual.SeasonFieldingStats.StolenBases.Value);
        Assert.Equal(10010, actual.SeasonFieldingStats.PassedBalls.Value);
        Assert.Equal(11011, actual.SeasonFieldingStats.CatcherInterferences.Value);
        Assert.Equal(12012, actual.SeasonFieldingStats.WildPitches.Value);
        Assert.Equal(13013, actual.SeasonFieldingStats.Pickoffs.Value);
    }
}