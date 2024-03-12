using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.Entities;

public class PlayerStatsBySeasonTests
{
    [Fact]
    public void BattingStatsByGamesChronologically_BattingStats_ReturnsStatsOrderedByGameDate()
    {
        // Arrange
        var game1 = Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 3, 31));
        var game2 = Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 4, 2));
        var game3 = Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 4, 5));
        var battingStatsByGames = new List<PlayerBattingStatsByGame>()
            { game3, game2, game1 }; // Most recent to oldest
        var seasonStats = Faker.FakePlayerSeasonStats(battingStatsByGames: battingStatsByGames);

        // Act
        var actual = seasonStats.BattingStatsByGamesChronologically;

        // Assert
        Assert.Equal(3, actual.Count);
        Assert.Equal(new DateTime(2024, 3, 31), actual[0].GameDate);
        Assert.Equal(new DateTime(2024, 4, 2), actual[1].GameDate);
        Assert.Equal(new DateTime(2024, 4, 5), actual[2].GameDate);
    }

    [Fact]
    public void FieldingStatsByGamesChronologically_FieldingStats_ReturnsStatsOrderedByGameDate()
    {
        // Arrange
        var game1 = Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 3, 31));
        var game2 = Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 2));
        var game3 = Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 5));
        var fieldingStatsByGames = new List<PlayerFieldingStatsByGame>()
            { game3, game2, game1 }; // Most recent to oldest
        var seasonStats = Faker.FakePlayerSeasonStats(fieldingStatsByGames: fieldingStatsByGames);

        // Act
        var actual = seasonStats.FieldingStatsByGamesChronologically;

        // Assert
        Assert.Equal(3, actual.Count);
        Assert.Equal(new DateTime(2024, 3, 31), actual[0].GameDate);
        Assert.Equal(new DateTime(2024, 4, 2), actual[1].GameDate);
        Assert.Equal(new DateTime(2024, 4, 5), actual[2].GameDate);
    }

    [Fact]
    public void PitchingStatsByGamesChronologically_PitchingStats_ReturnsStatsOrderedByGameDate()
    {
        // Arrange
        var game1 = Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 3, 31));
        var game2 = Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 2));
        var game3 = Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 5));
        var pitchingStatsByGames = new List<PlayerPitchingStatsByGame>()
            { game3, game2, game1 }; // Most recent to oldest
        var seasonStats = Faker.FakePlayerSeasonStats(pitchingStatsByGames: pitchingStatsByGames);

        // Act
        var actual = seasonStats.PitchingStatsByGamesChronologically;

        // Assert
        Assert.Equal(3, actual.Count);
        Assert.Equal(new DateTime(2024, 3, 31), actual[0].GameDate);
        Assert.Equal(new DateTime(2024, 4, 2), actual[1].GameDate);
        Assert.Equal(new DateTime(2024, 4, 5), actual[2].GameDate);
    }

    [Fact]
    public void SeasonBattingStats_BattingStatsByGameCollection_ReturnsSeasonTotals()
    {
        // Arrange
        var game1 = Faker.FakePlayerBattingStats(
            plateAppearances: 1, // Every number is different to ensure no crossed variable assignment
            atBats: 2,
            runs: 3,
            hits: 4,
            doubles: 5,
            triples: 6,
            homeRuns: 7,
            runsBattedIn: 8,
            baseOnBalls: 9,
            intentionalWalks: 10,
            strikeouts: 11,
            stolenBases: 12,
            caughtStealing: 13,
            hitByPitch: 14,
            sacrificeBunts: 15,
            sacrificeFlies: 16,
            numberOfPitchesSeen: 17,
            leftOnBase: 18,
            groundOuts: 19,
            groundIntoDoublePlays: 20,
            groundIntoTriplePlays: 21,
            airOuts: 22,
            catcherInterferences: 23
        );
        var game2 = Faker.FakePlayerBattingStats(
            plateAppearances: 1000, // Game2 is simply the previous one multiplied by 1000 to make expected values easy to calculate
            atBats: 2000,
            runs: 3000,
            hits: 4000,
            doubles: 5000,
            triples: 6000,
            homeRuns: 7000,
            runsBattedIn: 8000,
            baseOnBalls: 9000,
            intentionalWalks: 10000,
            strikeouts: 11000,
            stolenBases: 12000,
            caughtStealing: 13000,
            hitByPitch: 14000,
            sacrificeBunts: 15000,
            sacrificeFlies: 16000,
            numberOfPitchesSeen: 17000,
            leftOnBase: 18000,
            groundOuts: 19000,
            groundIntoDoublePlays: 20000,
            groundIntoTriplePlays: 21000,
            airOuts: 22000,
            catcherInterferences: 23000
        );
        var battingStatsByGames = new List<PlayerBattingStatsByGame>() { game1, game2 };
        var seasonStats = Faker.FakePlayerSeasonStats(battingStatsByGames: battingStatsByGames);

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
            inningsPitched: 1,
            hits: 2,
            doubles: 3,
            triples: 4,
            homeRuns: 5,
            runs: 6,
            earnedRuns: 7,
            strikeouts: 8,
            baseOnBalls: 9,
            intentionalWalks: 10,
            hitBatsmen: 11,
            outs: 12,
            groundOuts: 13,
            airOuts: 14,
            groundIntoDoublePlays: 15,
            numberOfPitches: 16,
            strikes: 17,
            wildPitches: 18,
            balks: 19,
            battersFaced: 20,
            atBats: 21,
            stolenBases: 22,
            caughtStealing: 23,
            pickOffs: 24,
            inheritedRunners: 25,
            inheritedRunnersScored: 26,
            catcherInterferences: 27,
            sacrificeBunts: 28,
            sacrificeFlies: 29
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
            inningsPitched: 1000,
            hits: 2000,
            doubles: 3000,
            triples: 4000,
            homeRuns: 5000,
            runs: 6000,
            earnedRuns: 7000,
            strikeouts: 8000,
            baseOnBalls: 9000,
            intentionalWalks: 10000,
            hitBatsmen: 11000,
            outs: 12000,
            groundOuts: 13000,
            airOuts: 14000,
            groundIntoDoublePlays: 15000,
            numberOfPitches: 16000,
            strikes: 17000,
            wildPitches: 18000,
            balks: 19000,
            battersFaced: 20000,
            atBats: 21000,
            stolenBases: 22000,
            caughtStealing: 23000,
            pickOffs: 24000,
            inheritedRunners: 25000,
            inheritedRunnersScored: 26000,
            catcherInterferences: 27000,
            sacrificeBunts: 28000,
            sacrificeFlies: 29000
        );
        var game3 = Faker.FakePlayerPitchingStats(hold: true,
            save: true);
        var playerPitchingStatsByGames = new List<PlayerPitchingStatsByGame>() { game1, game2, game3 };
        var seasonStats = Faker.FakePlayerSeasonStats(pitchingStatsByGames: playerPitchingStatsByGames);

        // Act
        var actual = seasonStats.SeasonPitchingStats;

        // Assert
        Assert.Equal(1, actual.Wins.Value);
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
        Assert.Equal(24024, actual.PickOffs.Value);
        Assert.Equal(25025, actual.InheritedRunners.Value);
        Assert.Equal(26026, actual.InheritedRunnersScored.Value);
        Assert.Equal(27027, actual.CatcherInterferences.Value);
        Assert.Equal(28028, actual.SacrificeBunts.Value);
        Assert.Equal(29029, actual.SacrificeFlies.Value);
    }

    [Fact]
    public void SeasonAggregateFieldingStats_FieldingStatsByGameCollection_ReturnsSeasonTotals()
    {
        // Arrange
        var game1 = Faker.FakePlayerFieldingStats(position: Position.RightField,
            gameStarted: true,
            inningsPlayed: 1,
            assists: 2,
            putouts: 3,
            errors: 4,
            throwingErrors: 5,
            doublePlays: 6,
            triplePlays: 7,
            caughtStealing: 8,
            stolenBases: 9,
            passedBalls: 10,
            catcherInterferences: 11,
            wildPitches: 12,
            pickOffs: 13
        );
        var game2 = Faker.FakePlayerFieldingStats(position: Position.LeftField,
            gameStarted: false,
            inningsPlayed: 1000,
            assists: 2000,
            putouts: 3000,
            errors: 4000,
            throwingErrors: 5000,
            doublePlays: 6000,
            triplePlays: 7000,
            caughtStealing: 8000,
            stolenBases: 9000,
            passedBalls: 10000,
            catcherInterferences: 11000,
            wildPitches: 12000,
            pickOffs: 13000
        );
        var playerFieldingStatsByGames = new List<PlayerFieldingStatsByGame>() { game1, game2 };
        var seasonStats = Faker.FakePlayerSeasonStats(fieldingStatsByGames: playerFieldingStatsByGames);

        // Act
        var actual = seasonStats.SeasonAggregateFieldingStats;

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
        Assert.Equal(13013, actual.PickOffs.Value);
    }

    [Fact]
    public void SeasonFieldingStatsByPosition_FieldingStatsByGameCollection_ReturnsSeasonTotals()
    {
        // Arrange
        var game1 = Faker.FakePlayerFieldingStats(position: Position.RightField,
            gameStarted: true,
            inningsPlayed: 1,
            assists: 2,
            putouts: 3,
            errors: 4,
            throwingErrors: 5,
            doublePlays: 6,
            triplePlays: 7,
            caughtStealing: 8,
            stolenBases: 9,
            passedBalls: 10,
            catcherInterferences: 11,
            wildPitches: 12,
            pickOffs: 13
        );
        var game2 = Faker.FakePlayerFieldingStats(position: Position.LeftField,
            gameStarted: false,
            inningsPlayed: 1000,
            assists: 2000,
            putouts: 3000,
            errors: 4000,
            throwingErrors: 5000,
            doublePlays: 6000,
            triplePlays: 7000,
            caughtStealing: 8000,
            stolenBases: 9000,
            passedBalls: 10000,
            catcherInterferences: 11000,
            wildPitches: 12000,
            pickOffs: 13000
        );
        var playerFieldingStatsByGames = new List<PlayerFieldingStatsByGame>() { game1, game2 };
        var seasonStats = Faker.FakePlayerSeasonStats(fieldingStatsByGames: playerFieldingStatsByGames);

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
        Assert.Equal(13, actual[Position.RightField].PickOffs.Value);

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
        Assert.Equal(13000, actual[Position.LeftField].PickOffs.Value);
    }

    [Fact]
    public void LogBattingGame_BattingGame_LogsGameToSeasonAndRaisesParticipationDomainEvent()
    {
        // Arrange
        var battingGame = Faker.FakePlayerBattingStats(plateAppearances: 3, atBats: 2, hits: 1, hitByPitch: 1);
        var seasonStats = Faker.FakePlayerSeasonStats();

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
        var seasonStats = Faker.FakePlayerSeasonStats();

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
        var seasonStats = Faker.FakePlayerSeasonStats();

        // Act
        seasonStats.LogFieldingGame(fieldingGame);

        // Assert
        Assert.Single(seasonStats.FieldingStatsByGamesChronologically);
        Assert.Equal(fieldingGame, seasonStats.FieldingStatsByGamesChronologically[0]);
        Assert.Equal(3, seasonStats.SeasonAggregateFieldingStats.Assists.Value);
        Assert.Equal(2, seasonStats.SeasonAggregateFieldingStats.Putouts.Value);
        Assert.Equal(1, seasonStats.SeasonAggregateFieldingStats.Errors.Value);

        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<PlayerFieldedInGameEvent>(seasonStats.DomainEvents[0]);
    }

    [Fact]
    public void AssessBattingPerformance_NegligibleBattingStats_NoEventsRaised()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 20m;
        var gameBeforeComparisonDate = Faker.FakePlayerBattingStats(gameDate: comparisonDate.AddDays(-1),
            plateAppearances: 4, atBats: 4, hits: 1, triples: 1); // OBP = 0.250, SLG = 0.750, OPS = 1.000
        var gameSinceComparisonDate = Faker.FakePlayerBattingStats(gameDate: comparisonDate.AddDays(1),
            plateAppearances: 4, atBats: 4, hits: 1, homeRuns: 1); // OBP = 0.250, SLG = 1.000, OPS = 1.250
        var seasonStats = Faker.FakePlayerSeasonStats(battingStatsByGames: new List<PlayerBattingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForBatting(percentChangeThreshold,
            gameBeforeComparisonDate.PlateAppearances, false, gameSinceComparisonDate.PlateAppearances, false
        );

        // Act
        seasonStats.AssessBattingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(0, seasonStats.DomainEvents.Count);
    }

    [Fact]
    public void AssessBattingPerformance_ImprovedBattingStats_RaisesBattingImprovementDomainEvent()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 20m;
        var gameBeforeComparisonDate = Faker.FakePlayerBattingStats(gameDate: comparisonDate.AddDays(-1),
            plateAppearances: 4, atBats: 4, hits: 1, triples: 1); // OBP = 0.250, SLG = 0.750, OPS = 1.000
        var gameSinceComparisonDate = Faker.FakePlayerBattingStats(gameDate: comparisonDate.AddDays(1),
            plateAppearances: 4, atBats: 4, hits: 1, homeRuns: 1); // OBP = 0.250, SLG = 1.000, OPS = 1.250
        var seasonStats = Faker.FakePlayerSeasonStats(battingStatsByGames: new List<PlayerBattingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForBatting(percentChangeThreshold,
            gameBeforeComparisonDate.PlateAppearances, true, gameSinceComparisonDate.PlateAppearances, true
        );

        // Act
        seasonStats.AssessBattingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<BattingImprovementEvent>(seasonStats.DomainEvents[0]);
    }

    [Fact]
    public void AssessBattingPerformance_DecliningBattingStats_RaisesBattingDeclineDomainEvent()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 20m;
        var gameBeforeComparisonDate = Faker.FakePlayerBattingStats(gameDate: comparisonDate.AddDays(-1),
            plateAppearances: 4, atBats: 4, hits: 1, homeRuns: 1); // OBP = 0.250, SLG = 1.000, OPS = 1.250
        var gameSinceComparisonDate = Faker.FakePlayerBattingStats(gameDate: comparisonDate.AddDays(1),
            plateAppearances: 4, atBats: 4, hits: 1, triples: 1); // OBP = 0.250, SLG = 0.750, OPS = 1.000
        var seasonStats = Faker.FakePlayerSeasonStats(battingStatsByGames: new List<PlayerBattingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForBatting(percentChangeThreshold,
            gameBeforeComparisonDate.PlateAppearances, true, gameSinceComparisonDate.PlateAppearances, true
        );

        // Act
        seasonStats.AssessBattingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<BattingDeclineEvent>(seasonStats.DomainEvents[0]);
    }

    [Fact]
    public void AssessBattingPerformance_RequiredStatPercentChangeThresholdNotCrossed_NoEventsRaised()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 30m;
        var gameBeforeComparisonDate = Faker.FakePlayerBattingStats(gameDate: comparisonDate.AddDays(-1),
            plateAppearances: 4, atBats: 4, hits: 1, triples: 1); // OBP = 0.250, SLG = 0.750, OPS = 1.000
        var gameSinceComparisonDate = Faker.FakePlayerBattingStats(gameDate: comparisonDate.AddDays(1),
            plateAppearances: 4, atBats: 4, hits: 1, homeRuns: 1); // OBP = 0.250, SLG = 1.000, OPS = 1.250
        var seasonStats = Faker.FakePlayerSeasonStats(battingStatsByGames: new List<PlayerBattingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForBatting(percentChangeThreshold,
            gameBeforeComparisonDate.PlateAppearances, true, gameSinceComparisonDate.PlateAppearances, true
        );

        // Act
        seasonStats.AssessBattingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(0, seasonStats.DomainEvents.Count);
    }

    [Fact]
    public void AssessPitchingPerformance_NegligiblePitchingStats_NoEventsRaised()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 20m;
        var gameBeforeComparisonDate = Faker.FakePlayerPitchingStats(gameDate: comparisonDate.AddDays(-1),
            inningsPitched: 9, battersFaced: 27, earnedRuns: 3); // ERA = 3
        var gameSinceComparisonDate = Faker.FakePlayerPitchingStats(gameDate: comparisonDate.AddDays(1),
            inningsPitched: 9, battersFaced: 27, earnedRuns: 3); // ERA = 3
        var seasonStats = Faker.FakePlayerSeasonStats(pitchingStatsByGames: new List<PlayerPitchingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForPitching(percentChangeThreshold,
            gameBeforeComparisonDate.InningsPitched, gameBeforeComparisonDate.BattersFaced, false,
            gameSinceComparisonDate.InningsPitched, gameSinceComparisonDate.BattersFaced, false);

        // Act
        seasonStats.AssessPitchingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(0, seasonStats.DomainEvents.Count);
    }

    [Fact]
    public void AssessPitchingPerformance_ImprovedPitchingStats_RaisesPitchingImprovementDomainEvent()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 20m;
        var gameBeforeComparisonDate = Faker.FakePlayerPitchingStats(gameDate: comparisonDate.AddDays(-1),
            inningsPitched: 9, battersFaced: 27, earnedRuns: 3); // ERA = 3
        var gameSinceComparisonDate = Faker.FakePlayerPitchingStats(gameDate: comparisonDate.AddDays(1),
            inningsPitched: 9, battersFaced: 27, earnedRuns: 1); // ERA = 1
        var seasonStats = Faker.FakePlayerSeasonStats(pitchingStatsByGames: new List<PlayerPitchingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForPitching(percentChangeThreshold,
            gameBeforeComparisonDate.InningsPitched, gameBeforeComparisonDate.BattersFaced, true,
            gameSinceComparisonDate.InningsPitched, gameSinceComparisonDate.BattersFaced, true);

        // Act
        seasonStats.AssessPitchingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<PitchingImprovementEvent>(seasonStats.DomainEvents[0]);
    }

    [Fact]
    public void AssessPitchingPerformance_DecliningPitchingStats_RaisesPitchingDeclineDomainEvent()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 20m;
        var gameBeforeComparisonDate = Faker.FakePlayerPitchingStats(gameDate: comparisonDate.AddDays(-1),
            inningsPitched: 9, battersFaced: 27, earnedRuns: 1); // ERA = 1
        var gameSinceComparisonDate = Faker.FakePlayerPitchingStats(gameDate: comparisonDate.AddDays(1),
            inningsPitched: 9, battersFaced: 27, earnedRuns: 3); // ERA = 3
        var seasonStats = Faker.FakePlayerSeasonStats(pitchingStatsByGames: new List<PlayerPitchingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForPitching(percentChangeThreshold,
            gameBeforeComparisonDate.InningsPitched, gameBeforeComparisonDate.BattersFaced, true,
            gameSinceComparisonDate.InningsPitched, gameSinceComparisonDate.BattersFaced, true);

        // Act
        seasonStats.AssessPitchingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<PitchingDeclineEvent>(seasonStats.DomainEvents[0]);
    }

    [Fact]
    public void AssessPitchingPerformance_RequiredStatPercentChangeThresholdNotCrossed_NoEventsRaised()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 70m;
        var gameBeforeComparisonDate = Faker.FakePlayerPitchingStats(gameDate: comparisonDate.AddDays(-1),
            inningsPitched: 9, battersFaced: 27, earnedRuns: 3); // ERA = 3
        var gameSinceComparisonDate = Faker.FakePlayerPitchingStats(gameDate: comparisonDate.AddDays(1),
            inningsPitched: 9, battersFaced: 27, earnedRuns: 1); // ERA = 1
        var seasonStats = Faker.FakePlayerSeasonStats(pitchingStatsByGames: new List<PlayerPitchingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForPitching(percentChangeThreshold,
            gameBeforeComparisonDate.InningsPitched, gameBeforeComparisonDate.BattersFaced, true,
            gameSinceComparisonDate.InningsPitched, gameSinceComparisonDate.BattersFaced, true);

        // Act
        seasonStats.AssessPitchingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(0, seasonStats.DomainEvents.Count);
    }

    [Fact]
    public void AssessFieldingPerformance_NegligibleFieldingStats_NoEventsRaised()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 20m;
        var gameBeforeComparisonDate = Faker.FakePlayerFieldingStats(gameDate: comparisonDate.AddDays(-1),
            assists: 1, putouts: 1, errors: 1); // TC = 3, F% = 2/3
        var gameSinceComparisonDate = Faker.FakePlayerFieldingStats(gameDate: comparisonDate.AddDays(1),
            assists: 1, putouts: 1, errors: 1); // TC = 3, F% = 2/3
        var seasonStats = Faker.FakePlayerSeasonStats(fieldingStatsByGames: new List<PlayerFieldingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForFielding(percentChangeThreshold,
            gameBeforeComparisonDate.TotalChances.ToNaturalNumber(), false,
            gameSinceComparisonDate.TotalChances.ToNaturalNumber(), false);

        // Act
        seasonStats.AssessFieldingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(0, seasonStats.DomainEvents.Count);
    }

    [Fact]
    public void AssessFieldingPerformance_ImprovedFieldingStats_RaisesFieldingImprovementDomainEvent()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 10m;
        var gameBeforeComparisonDate = Faker.FakePlayerFieldingStats(gameDate: comparisonDate.AddDays(-1),
            assists: 1, putouts: 1, errors: 1); // TC = 3, F% = 2/3
        var gameSinceComparisonDate = Faker.FakePlayerFieldingStats(gameDate: comparisonDate.AddDays(1),
            assists: 1, putouts: 2, errors: 1); // TC = 4, F% = 3/4
        var seasonStats = Faker.FakePlayerSeasonStats(fieldingStatsByGames: new List<PlayerFieldingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForFielding(percentChangeThreshold,
            gameBeforeComparisonDate.TotalChances.ToNaturalNumber(), true,
            gameSinceComparisonDate.TotalChances.ToNaturalNumber(), true);

        // Act
        seasonStats.AssessFieldingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<FieldingImprovementEvent>(seasonStats.DomainEvents[0]);
    }

    [Fact]
    public void AssessFieldingPerformance_DecliningFieldingStats_RaisesFieldingDeclineDomainEvent()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 10m;
        var gameBeforeComparisonDate = Faker.FakePlayerFieldingStats(gameDate: comparisonDate.AddDays(-1),
            assists: 1, putouts: 2, errors: 1); // TC = 4, F% = 3/4
        var gameSinceComparisonDate = Faker.FakePlayerFieldingStats(gameDate: comparisonDate.AddDays(1),
            assists: 1, putouts: 1, errors: 1); // TC = 3, F% = 2/3
        var seasonStats = Faker.FakePlayerSeasonStats(fieldingStatsByGames: new List<PlayerFieldingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForFielding(percentChangeThreshold,
            gameBeforeComparisonDate.TotalChances.ToNaturalNumber(), true,
            gameSinceComparisonDate.TotalChances.ToNaturalNumber(), true);

        // Act
        seasonStats.AssessFieldingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(1, seasonStats.DomainEvents.Count);
        Assert.IsType<FieldingDeclineEvent>(seasonStats.DomainEvents[0]);
    }

    [Fact]
    public void AssessFieldingPerformance_StatPercentChangeThresholdNotCrossed_NoEventsRaised()
    {
        // Arrange
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal percentChangeThreshold = 30m;
        var gameBeforeComparisonDate = Faker.FakePlayerFieldingStats(gameDate: comparisonDate.AddDays(-1),
            assists: 1, putouts: 1, errors: 1); // TC = 3, F% = 2/3
        var gameSinceComparisonDate = Faker.FakePlayerFieldingStats(gameDate: comparisonDate.AddDays(1),
            assists: 1, putouts: 2, errors: 1); // TC = 4, F% = 3/4
        var seasonStats = Faker.FakePlayerSeasonStats(fieldingStatsByGames: new List<PlayerFieldingStatsByGame>()
        {
            gameBeforeComparisonDate, gameSinceComparisonDate
        });

        var mockAssessmentRequirements = GetMockPerformanceAssessmentRequirementsForFielding(percentChangeThreshold,
            gameBeforeComparisonDate.TotalChances.ToNaturalNumber(), true,
            gameSinceComparisonDate.TotalChances.ToNaturalNumber(), true);

        // Act
        seasonStats.AssessFieldingPerformance(comparisonDate, mockAssessmentRequirements.Object);

        // Assert
        Assert.Equal(0, seasonStats.DomainEvents.Count);
    }

    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var mlbId = MlbId.Create(1);
        var seasonYear = SeasonYear.Create(2024);
        var battingStats = new List<PlayerBattingStatsByGame>() { Faker.FakePlayerBattingStats() };
        var fieldingStats = new List<PlayerFieldingStatsByGame>() { Faker.FakePlayerFieldingStats() };
        var pitchingStats = new List<PlayerPitchingStatsByGame>() { Faker.FakePlayerPitchingStats() };

        // Act
        var actual = PlayerStatsBySeason.Create(mlbId, seasonYear, battingStats, pitchingStats, fieldingStats);

        // Assert
        Assert.Equal(mlbId, actual.PlayerMlbId);
        Assert.Equal(seasonYear, actual.SeasonYear);
        Assert.Equal(battingStats, actual.BattingStatsByGamesChronologically);
        Assert.Equal(fieldingStats, actual.FieldingStatsByGamesChronologically);
        Assert.Equal(pitchingStats, actual.PitchingStatsByGamesChronologically);
    }

    private Mock<IPerformanceAssessmentRequirements> GetMockPerformanceAssessmentRequirementsForBatting(
        decimal percentChangeThreshold, NaturalNumber plateAppearancesBeforeComparisonDate, bool beforeRequirementsMet,
        NaturalNumber plateAppearancesSinceComparisonDate, bool sinceRequirementsMet)
    {
        var mockAssessmentRequirements = new Mock<IPerformanceAssessmentRequirements>();
        mockAssessmentRequirements
            .Setup(x => x.StatPercentChangeThreshold)
            .Returns(percentChangeThreshold);
        mockAssessmentRequirements
            .Setup(x => x.AreBattingAssessmentRequirementsMet(plateAppearancesBeforeComparisonDate))
            .Returns(beforeRequirementsMet);
        mockAssessmentRequirements
            .Setup(x => x.AreBattingAssessmentRequirementsMet(plateAppearancesSinceComparisonDate))
            .Returns(sinceRequirementsMet);
        return mockAssessmentRequirements;
    }

    private Mock<IPerformanceAssessmentRequirements> GetMockPerformanceAssessmentRequirementsForPitching(
        decimal percentChangeThreshold, InningsCount inningsPitchedBeforeComparisonDate,
        NaturalNumber battersFacedBeforeComparisonDate, bool beforeRequirementsMet,
        InningsCount inningsPitchedSinceComparisonDate, NaturalNumber battersFacedSinceComparisonDate,
        bool sinceRequirementsMet)
    {
        var mockAssessmentRequirements = new Mock<IPerformanceAssessmentRequirements>();
        mockAssessmentRequirements
            .Setup(x => x.StatPercentChangeThreshold)
            .Returns(percentChangeThreshold);
        mockAssessmentRequirements
            .Setup(x => x.ArePitchingAssessmentRequirementsMet(inningsPitchedBeforeComparisonDate,
                battersFacedBeforeComparisonDate))
            .Returns(beforeRequirementsMet);
        mockAssessmentRequirements
            .Setup(x => x.ArePitchingAssessmentRequirementsMet(inningsPitchedSinceComparisonDate,
                battersFacedSinceComparisonDate))
            .Returns(sinceRequirementsMet);
        return mockAssessmentRequirements;
    }

    private Mock<IPerformanceAssessmentRequirements> GetMockPerformanceAssessmentRequirementsForFielding(
        decimal percentChangeThreshold, NaturalNumber totalChancesBeforeComparisonDate, bool beforeRequirementsMet,
        NaturalNumber totalChancesSinceComparisonDate, bool sinceRequirementsMet)
    {
        var mockAssessmentRequirements = new Mock<IPerformanceAssessmentRequirements>();
        mockAssessmentRequirements
            .Setup(x => x.StatPercentChangeThreshold)
            .Returns(percentChangeThreshold);
        mockAssessmentRequirements
            .Setup(x => x.AreFieldingAssessmentRequirementsMet(totalChancesBeforeComparisonDate))
            .Returns(beforeRequirementsMet);
        mockAssessmentRequirements
            .Setup(x => x.AreFieldingAssessmentRequirementsMet(totalChancesSinceComparisonDate))
            .Returns(sinceRequirementsMet);
        return mockAssessmentRequirements;
    }
}