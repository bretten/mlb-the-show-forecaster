using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.Entities;

public class PlayerStatsBySeasonTests
{
    [Fact]
    public void BattingStatsByGamesChronologically_BattingStats_ReturnsStatsOrderedByGameDate()
    {
        // Arrange
        var game1 = Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 3, 31));
        var game2 = Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 2));
        var game3 = Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 5));
        var battingStatsByGames = new List<PlayerBattingStatsByGame>()
            { game3, game2, game1 }; // Most recent to oldest
        var seasonStats = Faker.FakePlayerStatsBySeason(battingStatsByGames: battingStatsByGames);

        // Act
        var actual = seasonStats.BattingStatsByGamesChronologically;

        // Assert
        Assert.Equal(3, actual.Count);
        Assert.Equal(new DateOnly(2024, 3, 31), actual[0].GameDate);
        Assert.Equal(new DateOnly(2024, 4, 2), actual[1].GameDate);
        Assert.Equal(new DateOnly(2024, 4, 5), actual[2].GameDate);
    }

    [Fact]
    public void FieldingStatsByGamesChronologically_FieldingStats_ReturnsStatsOrderedByGameDate()
    {
        // Arrange
        var game1 = Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 3, 31));
        var game2 = Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 2));
        var game3 = Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 5));
        var fieldingStatsByGames = new List<PlayerFieldingStatsByGame>()
            { game3, game2, game1 }; // Most recent to oldest
        var seasonStats = Faker.FakePlayerStatsBySeason(fieldingStatsByGames: fieldingStatsByGames);

        // Act
        var actual = seasonStats.FieldingStatsByGamesChronologically;

        // Assert
        Assert.Equal(3, actual.Count);
        Assert.Equal(new DateOnly(2024, 3, 31), actual[0].GameDate);
        Assert.Equal(new DateOnly(2024, 4, 2), actual[1].GameDate);
        Assert.Equal(new DateOnly(2024, 4, 5), actual[2].GameDate);
    }

    [Fact]
    public void PitchingStatsByGamesChronologically_PitchingStats_ReturnsStatsOrderedByGameDate()
    {
        // Arrange
        var game1 = Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 3, 31));
        var game2 = Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 2));
        var game3 = Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 5));
        var pitchingStatsByGames = new List<PlayerPitchingStatsByGame>()
            { game3, game2, game1 }; // Most recent to oldest
        var seasonStats = Faker.FakePlayerStatsBySeason(pitchingStatsByGames: pitchingStatsByGames);

        // Act
        var actual = seasonStats.PitchingStatsByGamesChronologically;

        // Assert
        Assert.Equal(3, actual.Count);
        Assert.Equal(new DateOnly(2024, 3, 31), actual[0].GameDate);
        Assert.Equal(new DateOnly(2024, 4, 2), actual[1].GameDate);
        Assert.Equal(new DateOnly(2024, 4, 5), actual[2].GameDate);
    }

    [Fact]
    public void SeasonBattingStats_BattingStatsByGameCollection_ReturnsSeasonTotals()
    {
        // Arrange
        var game1 = Faker.FakePlayerBattingStats(scalar: 1);
        var game2 = Faker.FakePlayerBattingStats(scalar: 1000);
        var battingStatsByGames = new List<PlayerBattingStatsByGame>() { game1, game2 };
        var seasonStats = Faker.FakePlayerStatsBySeason(battingStatsByGames: battingStatsByGames);

        // Act
        var actual = seasonStats.SeasonBattingStats;

        // Assert
        Assert.Equal(1001, actual.PlateAppearances.Value);
        Assert.Equal(2002, actual.AtBats.Value);
        Assert.Equal(3003, actual.Runs.Value);
        Assert.Equal(4004, actual.Hits.Value);
        Assert.Equal(5005, actual.Doubles.Value);
        Assert.Equal(6006, actual.Triples.Value);
        Assert.Equal(7007, actual.HomeRuns.Value);
        Assert.Equal(8008, actual.RunsBattedIn.Value);
        Assert.Equal(9009, actual.BaseOnBalls.Value);
        Assert.Equal(10010, actual.IntentionalWalks.Value);
        Assert.Equal(11011, actual.Strikeouts.Value);
        Assert.Equal(12012, actual.StolenBases.Value);
        Assert.Equal(13013, actual.CaughtStealing.Value);
        Assert.Equal(14014, actual.HitByPitches.Value);
        Assert.Equal(15015, actual.SacrificeBunts.Value);
        Assert.Equal(16016, actual.SacrificeFlies.Value);
        Assert.Equal(17017, actual.NumberOfPitchesSeen.Value);
        Assert.Equal(18018, actual.LeftOnBase.Value);
        Assert.Equal(19019, actual.GroundOuts.Value);
        Assert.Equal(20020, actual.GroundIntoDoublePlays.Value);
        Assert.Equal(21021, actual.GroundIntoTriplePlays.Value);
        Assert.Equal(22022, actual.AirOuts.Value);
        Assert.Equal(23023, actual.CatcherInterferences.Value);
    }

    [Fact]
    public void SeasonPitchingStats_PitchingStatsByGameCollection_ReturnsSeasonTotals()
    {
        // Arrange
        var game1 = Faker.FakePlayerPitchingStats(win: true,
            loss: false,
            gameStarted: true,
            gameFinished: false,
            completeGame: true,
            shutout: true,
            hold: false,
            save: false,
            blownSave: false,
            saveOpportunity: false,
            scalar: 1
        );
        var game2 = Faker.FakePlayerPitchingStats(win: false,
            loss: true,
            gameStarted: false,
            gameFinished: true,
            completeGame: false,
            shutout: false,
            hold: false,
            save: false,
            blownSave: true,
            saveOpportunity: true,
            scalar: 1000
        );
        var game3 = Faker.FakePlayerPitchingStats(win: true, hold: true, save: true, scalar: 0);
        var playerPitchingStatsByGames = new List<PlayerPitchingStatsByGame>() { game1, game2, game3 };
        var seasonStats = Faker.FakePlayerStatsBySeason(pitchingStatsByGames: playerPitchingStatsByGames);

        // Act
        var actual = seasonStats.SeasonPitchingStats;

        // Assert
        Assert.Equal(2, actual.Wins.Value); // game1 = win, game2 = loss, game3 = win
        Assert.Equal(1, actual.Losses.Value);
        Assert.Equal(1, actual.GamesStarted.Value);
        Assert.Equal(1, actual.GamesFinished.Value);
        Assert.Equal(1, actual.CompleteGames.Value);
        Assert.Equal(1, actual.Shutouts.Value);
        Assert.Equal(1, actual.Holds.Value);
        Assert.Equal(1, actual.Saves.Value);
        Assert.Equal(1, actual.BlownSaves.Value);
        Assert.Equal(1, actual.SaveOpportunities.Value);
        Assert.Equal(1001, actual.InningsPitched.Value);
        Assert.Equal(2002, actual.Hits.Value);
        Assert.Equal(3003, actual.Doubles.Value);
        Assert.Equal(4004, actual.Triples.Value);
        Assert.Equal(5005, actual.HomeRuns.Value);
        Assert.Equal(6006, actual.Runs.Value);
        Assert.Equal(7007, actual.EarnedRuns.Value);
        Assert.Equal(8008, actual.Strikeouts.Value);
        Assert.Equal(9009, actual.BaseOnBalls.Value);
        Assert.Equal(10010, actual.IntentionalWalks.Value);
        Assert.Equal(11011, actual.HitBatsmen.Value);
        Assert.Equal(12012, actual.Outs.Value);
        Assert.Equal(13013, actual.GroundOuts.Value);
        Assert.Equal(14014, actual.AirOuts.Value);
        Assert.Equal(15015, actual.GroundIntoDoublePlays.Value);
        Assert.Equal(16016, actual.NumberOfPitches.Value);
        Assert.Equal(17017, actual.Strikes.Value);
        Assert.Equal(18018, actual.WildPitches.Value);
        Assert.Equal(19019, actual.Balks.Value);
        Assert.Equal(20020, actual.BattersFaced.Value);
        Assert.Equal(21021, actual.AtBats.Value);
        Assert.Equal(22022, actual.StolenBases.Value);
        Assert.Equal(23023, actual.CaughtStealing.Value);
        Assert.Equal(24024, actual.Pickoffs.Value);
        Assert.Equal(25025, actual.InheritedRunners.Value);
        Assert.Equal(26026, actual.InheritedRunnersScored.Value);
        Assert.Equal(27027, actual.CatcherInterferences.Value);
        Assert.Equal(28028, actual.SacrificeBunts.Value);
        Assert.Equal(29029, actual.SacrificeFlies.Value);
    }

    [Fact]
    public void SeasonFieldingStats_FieldingStatsByGameCollection_ReturnsSeasonTotals()
    {
        // Arrange
        var game1 = Faker.FakePlayerFieldingStats(position: Position.RightField, gameStarted: true, scalar: 1);
        var game2 = Faker.FakePlayerFieldingStats(position: Position.LeftField, gameStarted: false, scalar: 1000);
        var playerFieldingStatsByGames = new List<PlayerFieldingStatsByGame>() { game1, game2 };
        var seasonStats = Faker.FakePlayerStatsBySeason(fieldingStatsByGames: playerFieldingStatsByGames);

        // Act
        var actual = seasonStats.SeasonFieldingStats;

        // Assert
        Assert.Equal(Position.RightField, actual.Position);
        Assert.Equal(1, actual.GamesStarted.Value);
        Assert.Equal(1001, actual.InningsPlayed.Value);
        Assert.Equal(2002, actual.Assists.Value);
        Assert.Equal(3003, actual.Putouts.Value);
        Assert.Equal(4004, actual.Errors.Value);
        Assert.Equal(5005, actual.ThrowingErrors.Value);
        Assert.Equal(6006, actual.DoublePlays.Value);
        Assert.Equal(7007, actual.TriplePlays.Value);
        Assert.Equal(8008, actual.CaughtStealing.Value);
        Assert.Equal(9009, actual.StolenBases.Value);
        Assert.Equal(10010, actual.PassedBalls.Value);
        Assert.Equal(11011, actual.CatcherInterferences.Value);
        Assert.Equal(12012, actual.WildPitches.Value);
        Assert.Equal(13013, actual.Pickoffs.Value);
    }

    [Fact]
    public void SeasonFieldingStatsByPosition_FieldingStatsByGameCollection_ReturnsSeasonTotals()
    {
        // Arrange
        var game1 = Faker.FakePlayerFieldingStats(position: Position.RightField, gameStarted: true, scalar: 1);
        var game2 = Faker.FakePlayerFieldingStats(position: Position.LeftField, gameStarted: false, scalar: 1000);
        var playerFieldingStatsByGames = new List<PlayerFieldingStatsByGame>() { game1, game2 };
        var seasonStats = Faker.FakePlayerStatsBySeason(fieldingStatsByGames: playerFieldingStatsByGames);

        // Act
        var actual = seasonStats.SeasonFieldingStatsByPosition;

        // Assert
        Assert.Equal(Position.RightField, actual[Position.RightField].Position);
        Assert.Equal(1, actual[Position.RightField].GamesStarted.Value);
        Assert.Equal(1, actual[Position.RightField].InningsPlayed.Value);
        Assert.Equal(2, actual[Position.RightField].Assists.Value);
        Assert.Equal(3, actual[Position.RightField].Putouts.Value);
        Assert.Equal(4, actual[Position.RightField].Errors.Value);
        Assert.Equal(5, actual[Position.RightField].ThrowingErrors.Value);
        Assert.Equal(6, actual[Position.RightField].DoublePlays.Value);
        Assert.Equal(7, actual[Position.RightField].TriplePlays.Value);
        Assert.Equal(8, actual[Position.RightField].CaughtStealing.Value);
        Assert.Equal(9, actual[Position.RightField].StolenBases.Value);
        Assert.Equal(10, actual[Position.RightField].PassedBalls.Value);
        Assert.Equal(11, actual[Position.RightField].CatcherInterferences.Value);
        Assert.Equal(12, actual[Position.RightField].WildPitches.Value);
        Assert.Equal(13, actual[Position.RightField].Pickoffs.Value);

        Assert.Equal(Position.LeftField, actual[Position.LeftField].Position);
        Assert.Equal(0, actual[Position.LeftField].GamesStarted.Value);
        Assert.Equal(1000, actual[Position.LeftField].InningsPlayed.Value);
        Assert.Equal(2000, actual[Position.LeftField].Assists.Value);
        Assert.Equal(3000, actual[Position.LeftField].Putouts.Value);
        Assert.Equal(4000, actual[Position.LeftField].Errors.Value);
        Assert.Equal(5000, actual[Position.LeftField].ThrowingErrors.Value);
        Assert.Equal(6000, actual[Position.LeftField].DoublePlays.Value);
        Assert.Equal(7000, actual[Position.LeftField].TriplePlays.Value);
        Assert.Equal(8000, actual[Position.LeftField].CaughtStealing.Value);
        Assert.Equal(9000, actual[Position.LeftField].StolenBases.Value);
        Assert.Equal(10000, actual[Position.LeftField].PassedBalls.Value);
        Assert.Equal(11000, actual[Position.LeftField].CatcherInterferences.Value);
        Assert.Equal(12000, actual[Position.LeftField].WildPitches.Value);
        Assert.Equal(13000, actual[Position.LeftField].Pickoffs.Value);
    }

    [Fact]
    public void BattingStatsFor_DateRange_ReturnsAggregateStatsForDateRange()
    {
        // Arrange
        var game1 = Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 6, 1), scalar: 1);
        var game2 = Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 6, 11), scalar: 1000);
        var game3 = Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 6, 12), scalar: 10000);
        var battingStatsByGames = new List<PlayerBattingStatsByGame>() { game1, game2, game3 };
        var seasonStats = Faker.FakePlayerStatsBySeason(battingStatsByGames: battingStatsByGames);

        // Act
        var actual = seasonStats.BattingStatsFor(new DateOnly(2024, 6, 1), new DateOnly(2024, 6, 11));

        // Assert
        Assert.Equal(1001, actual.PlateAppearances.Value);
        Assert.Equal(2002, actual.AtBats.Value);
        Assert.Equal(3003, actual.Runs.Value);
        Assert.Equal(4004, actual.Hits.Value);
        Assert.Equal(5005, actual.Doubles.Value);
        Assert.Equal(6006, actual.Triples.Value);
        Assert.Equal(7007, actual.HomeRuns.Value);
        Assert.Equal(8008, actual.RunsBattedIn.Value);
        Assert.Equal(9009, actual.BaseOnBalls.Value);
        Assert.Equal(10010, actual.IntentionalWalks.Value);
        Assert.Equal(11011, actual.Strikeouts.Value);
        Assert.Equal(12012, actual.StolenBases.Value);
        Assert.Equal(13013, actual.CaughtStealing.Value);
        Assert.Equal(14014, actual.HitByPitches.Value);
        Assert.Equal(15015, actual.SacrificeBunts.Value);
        Assert.Equal(16016, actual.SacrificeFlies.Value);
        Assert.Equal(17017, actual.NumberOfPitchesSeen.Value);
        Assert.Equal(18018, actual.LeftOnBase.Value);
        Assert.Equal(19019, actual.GroundOuts.Value);
        Assert.Equal(20020, actual.GroundIntoDoublePlays.Value);
        Assert.Equal(21021, actual.GroundIntoTriplePlays.Value);
        Assert.Equal(22022, actual.AirOuts.Value);
        Assert.Equal(23023, actual.CatcherInterferences.Value);
    }

    [Fact]
    public void PitchingStatsFor_DateRange_ReturnsAggregateStatsForDateRange()
    {
        // Arrange
        var game1 = Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 6, 1),
            win: true,
            loss: false,
            gameStarted: true,
            gameFinished: false,
            completeGame: true,
            shutout: true,
            hold: false,
            save: false,
            blownSave: false,
            saveOpportunity: false,
            scalar: 1
        );
        var game2 = Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 6, 11),
            win: false,
            loss: true,
            gameStarted: false,
            gameFinished: true,
            completeGame: false,
            shutout: false,
            hold: false,
            save: false,
            blownSave: true,
            saveOpportunity: true,
            scalar: 1000
        );
        var game3 = Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 6, 12), scalar: 10000);
        var playerPitchingStatsByGames = new List<PlayerPitchingStatsByGame>() { game1, game2, game3 };
        var seasonStats = Faker.FakePlayerStatsBySeason(pitchingStatsByGames: playerPitchingStatsByGames);

        // Act
        var actual = seasonStats.PitchingStatsFor(new DateOnly(2024, 6, 1), new DateOnly(2024, 6, 11));

        // Assert
        Assert.Equal(1, actual.Wins.Value);
        Assert.Equal(1, actual.Losses.Value);
        Assert.Equal(1, actual.GamesStarted.Value);
        Assert.Equal(1, actual.GamesFinished.Value);
        Assert.Equal(1, actual.CompleteGames.Value);
        Assert.Equal(1, actual.Shutouts.Value);
        Assert.Equal(0, actual.Holds.Value);
        Assert.Equal(0, actual.Saves.Value);
        Assert.Equal(1, actual.BlownSaves.Value);
        Assert.Equal(1, actual.SaveOpportunities.Value);
        Assert.Equal(1001, actual.InningsPitched.Value);
        Assert.Equal(2002, actual.Hits.Value);
        Assert.Equal(3003, actual.Doubles.Value);
        Assert.Equal(4004, actual.Triples.Value);
        Assert.Equal(5005, actual.HomeRuns.Value);
        Assert.Equal(6006, actual.Runs.Value);
        Assert.Equal(7007, actual.EarnedRuns.Value);
        Assert.Equal(8008, actual.Strikeouts.Value);
        Assert.Equal(9009, actual.BaseOnBalls.Value);
        Assert.Equal(10010, actual.IntentionalWalks.Value);
        Assert.Equal(11011, actual.HitBatsmen.Value);
        Assert.Equal(12012, actual.Outs.Value);
        Assert.Equal(13013, actual.GroundOuts.Value);
        Assert.Equal(14014, actual.AirOuts.Value);
        Assert.Equal(15015, actual.GroundIntoDoublePlays.Value);
        Assert.Equal(16016, actual.NumberOfPitches.Value);
        Assert.Equal(17017, actual.Strikes.Value);
        Assert.Equal(18018, actual.WildPitches.Value);
        Assert.Equal(19019, actual.Balks.Value);
        Assert.Equal(20020, actual.BattersFaced.Value);
        Assert.Equal(21021, actual.AtBats.Value);
        Assert.Equal(22022, actual.StolenBases.Value);
        Assert.Equal(23023, actual.CaughtStealing.Value);
        Assert.Equal(24024, actual.Pickoffs.Value);
        Assert.Equal(25025, actual.InheritedRunners.Value);
        Assert.Equal(26026, actual.InheritedRunnersScored.Value);
        Assert.Equal(27027, actual.CatcherInterferences.Value);
        Assert.Equal(28028, actual.SacrificeBunts.Value);
        Assert.Equal(29029, actual.SacrificeFlies.Value);
    }

    [Fact]
    public void FieldingStatsFor_DateRange_ReturnsAggregateStatsForDateRange()
    {
        // Arrange
        var game1 = Faker.FakePlayerFieldingStats(
            gameDate: new DateOnly(2024, 6, 1),
            position: Position.RightField,
            gameStarted: true,
            scalar: 1
        );
        var game2 = Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 6, 11),
            position: Position.LeftField,
            gameStarted: false,
            scalar: 1000
        );
        var game3 = Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 6, 12),
            position: Position.CenterField,
            gameStarted: true,
            scalar: 10000
        );
        var playerFieldingStatsByGames = new List<PlayerFieldingStatsByGame>() { game1, game2, game3 };
        var seasonStats = Faker.FakePlayerStatsBySeason(fieldingStatsByGames: playerFieldingStatsByGames);

        // Act
        var actual = seasonStats.FieldingStatsFor(new DateOnly(2024, 6, 1), new DateOnly(2024, 6, 11));

        // Assert
        Assert.Equal(Position.RightField, actual.Position);
        Assert.Equal(1, actual.GamesStarted.Value);
        Assert.Equal(1001, actual.InningsPlayed.Value);
        Assert.Equal(2002, actual.Assists.Value);
        Assert.Equal(3003, actual.Putouts.Value);
        Assert.Equal(4004, actual.Errors.Value);
        Assert.Equal(5005, actual.ThrowingErrors.Value);
        Assert.Equal(6006, actual.DoublePlays.Value);
        Assert.Equal(7007, actual.TriplePlays.Value);
        Assert.Equal(8008, actual.CaughtStealing.Value);
        Assert.Equal(9009, actual.StolenBases.Value);
        Assert.Equal(10010, actual.PassedBalls.Value);
        Assert.Equal(11011, actual.CatcherInterferences.Value);
        Assert.Equal(12012, actual.WildPitches.Value);
        Assert.Equal(13013, actual.Pickoffs.Value);
    }

    [Fact]
    public void FieldingStatsByPositionFor_DateRange_ReturnsAggregateStatsForDateRange()
    {
        // Arrange
        var game1 = Faker.FakePlayerFieldingStats(
            gameDate: new DateOnly(2024, 6, 1),
            position: Position.RightField,
            gameStarted: true,
            scalar: 1
        );
        var game2 = Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 6, 11),
            position: Position.LeftField,
            gameStarted: false,
            scalar: 1000
        );
        var game3 = Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 6, 12),
            position: Position.CenterField,
            gameStarted: true,
            scalar: 10000
        );
        var playerFieldingStatsByGames = new List<PlayerFieldingStatsByGame>() { game1, game2, game3 };
        var seasonStats = Faker.FakePlayerStatsBySeason(fieldingStatsByGames: playerFieldingStatsByGames);

        // Act
        var actual = seasonStats.FieldingStatsByPositionFor(new DateOnly(2024, 6, 1), new DateOnly(2024, 6, 11));

        // Assert
        Assert.Equal(Position.RightField, actual[Position.RightField].Position);
        Assert.Equal(1, actual[Position.RightField].GamesStarted.Value);
        Assert.Equal(1, actual[Position.RightField].InningsPlayed.Value);
        Assert.Equal(2, actual[Position.RightField].Assists.Value);
        Assert.Equal(3, actual[Position.RightField].Putouts.Value);
        Assert.Equal(4, actual[Position.RightField].Errors.Value);
        Assert.Equal(5, actual[Position.RightField].ThrowingErrors.Value);
        Assert.Equal(6, actual[Position.RightField].DoublePlays.Value);
        Assert.Equal(7, actual[Position.RightField].TriplePlays.Value);
        Assert.Equal(8, actual[Position.RightField].CaughtStealing.Value);
        Assert.Equal(9, actual[Position.RightField].StolenBases.Value);
        Assert.Equal(10, actual[Position.RightField].PassedBalls.Value);
        Assert.Equal(11, actual[Position.RightField].CatcherInterferences.Value);
        Assert.Equal(12, actual[Position.RightField].WildPitches.Value);
        Assert.Equal(13, actual[Position.RightField].Pickoffs.Value);

        Assert.Equal(Position.LeftField, actual[Position.LeftField].Position);
        Assert.Equal(0, actual[Position.LeftField].GamesStarted.Value);
        Assert.Equal(1000, actual[Position.LeftField].InningsPlayed.Value);
        Assert.Equal(2000, actual[Position.LeftField].Assists.Value);
        Assert.Equal(3000, actual[Position.LeftField].Putouts.Value);
        Assert.Equal(4000, actual[Position.LeftField].Errors.Value);
        Assert.Equal(5000, actual[Position.LeftField].ThrowingErrors.Value);
        Assert.Equal(6000, actual[Position.LeftField].DoublePlays.Value);
        Assert.Equal(7000, actual[Position.LeftField].TriplePlays.Value);
        Assert.Equal(8000, actual[Position.LeftField].CaughtStealing.Value);
        Assert.Equal(9000, actual[Position.LeftField].StolenBases.Value);
        Assert.Equal(10000, actual[Position.LeftField].PassedBalls.Value);
        Assert.Equal(11000, actual[Position.LeftField].CatcherInterferences.Value);
        Assert.Equal(12000, actual[Position.LeftField].WildPitches.Value);
        Assert.Equal(13000, actual[Position.LeftField].Pickoffs.Value);
    }

    [Fact]
    public void LogBattingGame_BattingGame_LogsGameToSeasonAndRaisesParticipationDomainEvent()
    {
        // Arrange
        var battingGame = Faker.FakePlayerBattingStats(plateAppearances: 3, atBats: 2, hits: 1, hitByPitch: 1);
        var seasonStats = Faker.FakePlayerStatsBySeason();

        // Act
        seasonStats.LogBattingGame(battingGame);

        // Assert
        Assert.Single(seasonStats.BattingStatsByGamesChronologically);
        Assert.Equal(battingGame, seasonStats.BattingStatsByGamesChronologically[0]);
        Assert.Equal(3, seasonStats.SeasonBattingStats.PlateAppearances.Value);
        Assert.Equal(2, seasonStats.SeasonBattingStats.AtBats.Value);
        Assert.Equal(1, seasonStats.SeasonBattingStats.Hits.Value);
        Assert.Equal(1, seasonStats.SeasonBattingStats.HitByPitches.Value);

        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<PlayerBattedInGameEvent>(seasonStats.DomainEvents[0]);
    }

    [Fact]
    public void LogPitchingGame_PitchingGame_LogsGameToSeasonAndRaisesParticipationDomainEvent()
    {
        // Arrange
        var pitchingGame = Faker.FakePlayerPitchingStats(battersFaced: 3, strikeouts: 2, hitBatsmen: 1);
        var seasonStats = Faker.FakePlayerStatsBySeason();

        // Act
        seasonStats.LogPitchingGame(pitchingGame);

        // Assert
        Assert.Single(seasonStats.PitchingStatsByGamesChronologically);
        Assert.Equal(pitchingGame, seasonStats.PitchingStatsByGamesChronologically[0]);
        Assert.Equal(3, seasonStats.SeasonPitchingStats.BattersFaced.Value);
        Assert.Equal(2, seasonStats.SeasonPitchingStats.Strikeouts.Value);
        Assert.Equal(1, seasonStats.SeasonPitchingStats.HitBatsmen.Value);

        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<PlayerPitchedInGameEvent>(seasonStats.DomainEvents[0]);
    }

    [Fact]
    public void LogFieldingGame_FieldingGame_LogsGameToSeasonAndRaisesParticipationDomainEvent()
    {
        // Arrange
        var fieldingGame = Faker.FakePlayerFieldingStats(assists: 3, putouts: 2, errors: 1);
        var seasonStats = Faker.FakePlayerStatsBySeason();

        // Act
        seasonStats.LogFieldingGame(fieldingGame);

        // Assert
        Assert.Single(seasonStats.FieldingStatsByGamesChronologically);
        Assert.Equal(fieldingGame, seasonStats.FieldingStatsByGamesChronologically[0]);
        Assert.Equal(3, seasonStats.SeasonFieldingStats.Assists.Value);
        Assert.Equal(2, seasonStats.SeasonFieldingStats.Putouts.Value);
        Assert.Equal(1, seasonStats.SeasonFieldingStats.Errors.Value);

        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<PlayerFieldedInGameEvent>(seasonStats.DomainEvents[0]);
    }

    [Fact]
    public void AssessBattingPerformance_ImprovedBattingStats_RaisesBattingImprovementDomainEvent()
    {
        // Arrange
        var newScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.8m);
        var firstHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.1m);
        var secondHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.9m);
        var seasonStats = Faker.FakePlayerStatsBySeason(battingStatsByGames: new List<PlayerBattingStatsByGame>()
            { Faker.FakePlayerBattingStats() });

        var stubPerformanceAssessor = new Mock<IPerformanceAssessor>();
        stubPerformanceAssessor.SetupSequence(x => x.AssessBatting(It.IsAny<BattingStats>()))
            .Returns(newScore)
            .Returns(firstHalfScore)
            .Returns(secondHalfScore);
        stubPerformanceAssessor.Setup(x => x.Compare(firstHalfScore, secondHalfScore))
            .Returns(PerformanceAssessment.TestClasses.Faker.FakePerformanceScoreComparisonIncrease());

        // Act
        seasonStats.AssessBattingPerformance(stubPerformanceAssessor.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<BattingImprovementEvent>(seasonStats.DomainEvents[0]);
        Assert.Equal(0.8m, seasonStats.BattingScore.Value);
    }

    [Fact]
    public void AssessBattingPerformance_DecliningBattingStats_RaisesBattingDeclineDomainEvent()
    {
        // Arrange
        var newScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.2m);
        var firstHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.9m);
        var secondHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.1m);
        var seasonStats = Faker.FakePlayerStatsBySeason(battingStatsByGames: new List<PlayerBattingStatsByGame>()
            { Faker.FakePlayerBattingStats() });

        var stubPerformanceAssessor = new Mock<IPerformanceAssessor>();
        stubPerformanceAssessor.SetupSequence(x => x.AssessBatting(It.IsAny<BattingStats>()))
            .Returns(newScore)
            .Returns(firstHalfScore)
            .Returns(secondHalfScore);
        stubPerformanceAssessor.Setup(x => x.Compare(firstHalfScore, secondHalfScore))
            .Returns(PerformanceAssessment.TestClasses.Faker.FakePerformanceScoreComparisonDecrease());

        // Act
        seasonStats.AssessBattingPerformance(stubPerformanceAssessor.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<BattingDeclineEvent>(seasonStats.DomainEvents[0]);
        Assert.Equal(0.2m, seasonStats.BattingScore.Value);
    }

    [Fact]
    public void AssessPitchingPerformance_ImprovedPitchingStats_RaisesPitchingImprovementDomainEvent()
    {
        // Arrange
        var newScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.8m);
        var firstHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.1m);
        var secondHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.9m);
        var seasonStats = Faker.FakePlayerStatsBySeason(pitchingStatsByGames: new List<PlayerPitchingStatsByGame>()
            { Faker.FakePlayerPitchingStats() });

        var stubPerformanceAssessor = new Mock<IPerformanceAssessor>();
        stubPerformanceAssessor.SetupSequence(x => x.AssessPitching(It.IsAny<PitchingStats>()))
            .Returns(newScore)
            .Returns(firstHalfScore)
            .Returns(secondHalfScore);
        stubPerformanceAssessor.Setup(x => x.Compare(firstHalfScore, secondHalfScore))
            .Returns(PerformanceAssessment.TestClasses.Faker.FakePerformanceScoreComparisonIncrease());

        // Act
        seasonStats.AssessPitchingPerformance(stubPerformanceAssessor.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<PitchingImprovementEvent>(seasonStats.DomainEvents[0]);
        Assert.Equal(0.8m, seasonStats.PitchingScore.Value);
    }

    [Fact]
    public void AssessPitchingPerformance_DecliningPitchingStats_RaisesPitchingDeclineDomainEvent()
    {
        // Arrange
        var newScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.2m);
        var firstHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.9m);
        var secondHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.1m);
        var seasonStats = Faker.FakePlayerStatsBySeason(pitchingStatsByGames: new List<PlayerPitchingStatsByGame>()
            { Faker.FakePlayerPitchingStats() });

        var stubPerformanceAssessor = new Mock<IPerformanceAssessor>();
        stubPerformanceAssessor.SetupSequence(x => x.AssessPitching(It.IsAny<PitchingStats>()))
            .Returns(newScore)
            .Returns(firstHalfScore)
            .Returns(secondHalfScore);
        stubPerformanceAssessor.Setup(x => x.Compare(firstHalfScore, secondHalfScore))
            .Returns(PerformanceAssessment.TestClasses.Faker.FakePerformanceScoreComparisonDecrease());

        // Act
        seasonStats.AssessPitchingPerformance(stubPerformanceAssessor.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<PitchingDeclineEvent>(seasonStats.DomainEvents[0]);
        Assert.Equal(0.2m, seasonStats.PitchingScore.Value);
    }

    [Fact]
    public void AssessFieldingPerformance_ImprovedFieldingStats_RaisesFieldingImprovementDomainEvent()
    {
        // Arrange
        var newScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.8m);
        var firstHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.1m);
        var secondHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.9m);
        var seasonStats = Faker.FakePlayerStatsBySeason(fieldingStatsByGames: new List<PlayerFieldingStatsByGame>()
            { Faker.FakePlayerFieldingStats() });

        var stubPerformanceAssessor = new Mock<IPerformanceAssessor>();
        stubPerformanceAssessor.SetupSequence(x => x.AssessFielding(It.IsAny<FieldingStats>()))
            .Returns(newScore)
            .Returns(firstHalfScore)
            .Returns(secondHalfScore);
        stubPerformanceAssessor.Setup(x => x.Compare(firstHalfScore, secondHalfScore))
            .Returns(PerformanceAssessment.TestClasses.Faker.FakePerformanceScoreComparisonIncrease());

        // Act
        seasonStats.AssessFieldingPerformance(stubPerformanceAssessor.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<FieldingImprovementEvent>(seasonStats.DomainEvents[0]);
        Assert.Equal(0.8m, seasonStats.FieldingScore.Value);
    }

    [Fact]
    public void AssessFieldingPerformance_DecliningFieldingStats_RaisesFieldingDeclineDomainEvent()
    {
        // Arrange
        var newScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.2m);
        var firstHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.9m);
        var secondHalfScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.1m);
        var seasonStats = Faker.FakePlayerStatsBySeason(fieldingStatsByGames: new List<PlayerFieldingStatsByGame>()
            { Faker.FakePlayerFieldingStats() });

        var stubPerformanceAssessor = new Mock<IPerformanceAssessor>();
        stubPerformanceAssessor.SetupSequence(x => x.AssessFielding(It.IsAny<FieldingStats>()))
            .Returns(newScore)
            .Returns(firstHalfScore)
            .Returns(secondHalfScore);
        stubPerformanceAssessor.Setup(x => x.Compare(firstHalfScore, secondHalfScore))
            .Returns(PerformanceAssessment.TestClasses.Faker.FakePerformanceScoreComparisonDecrease());

        // Act
        seasonStats.AssessFieldingPerformance(stubPerformanceAssessor.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<FieldingDeclineEvent>(seasonStats.DomainEvents[0]);
        Assert.Equal(0.2m, seasonStats.FieldingScore.Value);
    }

    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var mlbId = MlbId.Create(1);
        var seasonYear = SeasonYear.Create(2024);
        var battingScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.1m);
        var pitchingScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.2m);
        var fieldingScore = PerformanceAssessment.TestClasses.Faker.FakePerformanceScore(0.3m);
        var battingStats = new List<PlayerBattingStatsByGame>() { Faker.FakePlayerBattingStats() };
        var fieldingStats = new List<PlayerFieldingStatsByGame>() { Faker.FakePlayerFieldingStats() };
        var pitchingStats = new List<PlayerPitchingStatsByGame>() { Faker.FakePlayerPitchingStats() };

        // Act
        var actual = PlayerStatsBySeason.Create(mlbId, seasonYear, battingScore, pitchingScore, fieldingScore,
            battingStats, pitchingStats, fieldingStats);

        // Assert
        Assert.Equal(1, actual.PlayerMlbId.Value);
        Assert.Equal(2024, actual.SeasonYear.Value);
        Assert.Equal(0.1m, actual.BattingScore.Value);
        Assert.Equal(0.2m, actual.PitchingScore.Value);
        Assert.Equal(0.3m, actual.FieldingScore.Value);
        Assert.Equal(battingStats, actual.BattingStatsByGamesChronologically);
        Assert.Equal(fieldingStats, actual.FieldingStatsByGamesChronologically);
        Assert.Equal(pitchingStats, actual.PitchingStatsByGamesChronologically);
    }
}