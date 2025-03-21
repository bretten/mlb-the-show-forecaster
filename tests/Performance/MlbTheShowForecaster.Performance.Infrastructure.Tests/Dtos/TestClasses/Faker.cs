﻿using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Games;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests.Dtos.TestClasses;

public static class Faker
{
    public const int DefaultMlbId = 1;
    public const int DefaultTeamMlbId = 136;
    public const string DefaultFirstName = "First";
    public const string DefaultLastName = "Last";

    public static PlayerDto FakePlayerDto(int? mlbId = null, string? firstName = null, string? lastName = null,
        DateOnly birthdate = default, PositionDto? position = null, DateOnly mlbDebutDate = default,
        ArmSideDto? batSide = null, ArmSideDto? throwArm = null, CurrentTeamDto? team = null, bool active = false)
    {
        return new PlayerDto()
        {
            Id = mlbId ?? DefaultMlbId,
            FirstName = firstName ?? DefaultFirstName,
            LastName = lastName ?? DefaultLastName,
            Birthdate = birthdate == default ? new DateOnly(1990, 1, 1) : birthdate,
            Position = position ?? new PositionDto("Catcher", "C"),
            MlbDebutDate = mlbDebutDate == default ? new DateOnly(2010, 1, 1) : mlbDebutDate,
            BatSide = batSide ?? new ArmSideDto("L", "Left"),
            ThrowArm = throwArm ?? new ArmSideDto("L", "Left"),
            CurrentTeam = team ?? new CurrentTeamDto(DefaultTeamMlbId),
            Active = active
        };
    }

    public static TeamDto FakeTeamDto(int id = 10, string name = "Mariners")
    {
        return new TeamDto(id, name);
    }

    public static GameDto FakeGameDto(int id = 100)
    {
        return new GameDto(id);
    }

    public static GameHittingStatsDto FakeGameHittingStatsDto(string season = "2024",
        DateOnly date = default,
        string gameType = "R",
        bool isHome = true,
        bool isWin = true,
        TeamDto team = default,
        GameDto game = default,
        BattingStatsDto stat = default)
    {
        return new GameHittingStatsDto(Season: season,
            Date: date == default ? new DateOnly(2024, 4, 1) : date,
            GameType: gameType,
            IsHome: isHome,
            IsWin: isWin,
            Team: team == default ? FakeTeamDto() : team,
            Game: game == default ? FakeGameDto() : game,
            Stat: stat == default ? FakeBattingStatsDto() : stat
        );
    }

    public static GamePitchingStatsDto FakeGamePitchingStatsDto(string season = "2024",
        DateOnly date = default,
        string gameType = "R",
        bool isHome = true,
        bool isWin = true,
        TeamDto team = default,
        GameDto game = default,
        PitchingStatsDto stat = default)
    {
        return new GamePitchingStatsDto(Season: season,
            Date: date == default ? new DateOnly(2024, 4, 1) : date,
            GameType: gameType,
            IsHome: isHome,
            IsWin: isWin,
            Team: team == default ? FakeTeamDto() : team,
            Game: game == default ? FakeGameDto() : game,
            Stat: stat == default ? FakePitchingStatsDto() : stat
        );
    }

    public static GameFieldingStatsDto FakeGameFieldingStatsDto(string season = "2024",
        DateOnly date = default,
        string gameType = "R",
        bool isHome = true,
        bool isWin = true,
        TeamDto team = default,
        GameDto game = default,
        FieldingStatsDto stat = default)
    {
        return new GameFieldingStatsDto(Season: season,
            Date: date == default ? new DateOnly(2024, 4, 1) : date,
            GameType: gameType,
            IsHome: isHome,
            IsWin: isWin,
            Team: team == default ? FakeTeamDto() : team,
            Game: game == default ? FakeGameDto() : game,
            Stat: stat == default ? FakeFieldingStatsDto() : stat
        );
    }

    public static BattingStatsDto FakeBattingStatsDto(string summary = "1-3 | BB, 2 K",
        int gamesPlayed = 1,
        int groundOuts = 2,
        int airOuts = 3,
        int runs = 4,
        int doubles = 5,
        int triples = 6,
        int homeRuns = 7,
        int strikeOuts = 8,
        int baseOnBalls = 9,
        int intentionalWalks = 10,
        int hits = 11,
        int hitByPitch = 12,
        string avg = ".111",
        int atBats = 13,
        string obp = ".222",
        string slg = ".333",
        string ops = ".444",
        int caughtStealing = 14,
        int stolenBases = 15,
        string stolenBasePercentage = ".555",
        int groundIntoDoublePlay = 16,
        int groundIntoTriplePlay = 17,
        int numberOfPitches = 18,
        int plateAppearances = 19,
        int totalBases = 20,
        int rbi = 21,
        int leftOnBase = 22,
        int sacBunts = 23,
        int sacFlies = 24,
        string babip = ".666",
        string groundOutsToAirOuts = ".777",
        int catcherInterferences = 25,
        string atBatsPerHomeRun = ".888",
        int scalar = 1)
    {
        return new BattingStatsDto
        {
            Summary = summary,
            GamesPlayed = scalar * gamesPlayed,
            GroundOuts = scalar * groundOuts,
            AirOuts = scalar * airOuts,
            Runs = scalar * runs,
            Doubles = scalar * doubles,
            Triples = scalar * triples,
            HomeRuns = scalar * homeRuns,
            StrikeOuts = scalar * strikeOuts,
            BaseOnBalls = scalar * baseOnBalls,
            IntentionalWalks = scalar * intentionalWalks,
            Hits = scalar * hits,
            HitByPitch = scalar * hitByPitch,
            Avg = avg,
            AtBats = scalar * atBats,
            Obp = obp,
            Slg = slg,
            Ops = ops,
            CaughtStealing = scalar * caughtStealing,
            StolenBases = scalar * stolenBases,
            StolenBasePercentage = stolenBasePercentage,
            GroundIntoDoublePlay = scalar * groundIntoDoublePlay,
            GroundIntoTriplePlay = scalar * groundIntoTriplePlay,
            NumberOfPitches = scalar * numberOfPitches,
            PlateAppearances = scalar * plateAppearances,
            TotalBases = scalar * totalBases,
            Rbi = scalar * rbi,
            LeftOnBase = scalar * leftOnBase,
            SacBunts = scalar * sacBunts,
            SacFlies = scalar * sacFlies,
            Babip = babip,
            GroundOutsToAirOuts = groundOutsToAirOuts,
            CatcherInterferences = scalar * catcherInterferences,
            AtBatsPerHomeRun = atBatsPerHomeRun
        };
    }

    public static PitchingStatsDto FakePitchingStatsDto(string summary = "",
        int gamesPlayed = 1,
        int gamesStarted = 2,
        int groundOuts = 3,
        int airOuts = 4,
        int runs = 5,
        int doubles = 6,
        int triples = 7,
        int homeRuns = 8,
        int strikeOuts = 9,
        int baseOnBalls = 10,
        int intentionalWalks = 11,
        int hits = 12,
        int hitByPitch = 13,
        string avg = "",
        int atBats = 14,
        string obp = "",
        string slg = "",
        string ops = "",
        int caughtStealing = 15,
        int stolenBases = 16,
        string stolenBasePercentage = "",
        int groundIntoDoublePlay = 17,
        int numberOfPitches = 18,
        string era = "",
        string inningsPitched = "1",
        int wins = 19,
        int losses = 20,
        int saves = 21,
        int saveOpportunities = 22,
        int holds = 23,
        int blownSaves = 24,
        int earnedRuns = 25,
        string whip = "",
        int battersFaced = 26,
        int outs = 27,
        int gamesPitched = 28,
        int completeGames = 29,
        int shutouts = 30,
        int strikes = 31,
        string strikePercentage = "",
        int hitBatsmen = 32,
        int balks = 33,
        int wildPitches = 34,
        int pickoffs = 35,
        int totalBases = 36,
        string groundOutsToAirOuts = "",
        string winPercentage = "",
        string pitchesPerInning = "",
        int gamesFinished = 37,
        string strikeoutWalkRatio = "",
        string strikeoutsPer9Inn = "",
        string walksPer9Inn = "",
        string hitsPer9Inn = "",
        string runsScoredPer9 = "",
        string homeRunsPer9 = "",
        int inheritedRunners = 38,
        int inheritedRunnersScored = 39,
        int catcherInterferences = 40,
        int sacBunts = 41,
        int sacFlies = 42,
        int scalar = 1
    )
    {
        return new PitchingStatsDto
        {
            Summary = summary,
            GamesPlayed = scalar * gamesPlayed,
            GamesStarted = scalar * gamesStarted,
            GroundOuts = scalar * groundOuts,
            AirOuts = scalar * airOuts,
            Runs = scalar * runs,
            Doubles = scalar * doubles,
            Triples = scalar * triples,
            HomeRuns = scalar * homeRuns,
            StrikeOuts = scalar * strikeOuts,
            BaseOnBalls = scalar * baseOnBalls,
            IntentionalWalks = scalar * intentionalWalks,
            Hits = scalar * hits,
            HitByPitch = scalar * hitByPitch,
            Avg = avg,
            AtBats = scalar * atBats,
            Obp = obp,
            Slg = slg,
            Ops = ops,
            CaughtStealing = scalar * caughtStealing,
            StolenBases = scalar * stolenBases,
            StolenBasePercentage = stolenBasePercentage,
            GroundIntoDoublePlay = scalar * groundIntoDoublePlay,
            NumberOfPitches = scalar * numberOfPitches,
            Era = era,
            InningsPitched = inningsPitched,
            Wins = scalar * wins,
            Losses = scalar * losses,
            Saves = scalar * saves,
            SaveOpportunities = scalar * saveOpportunities,
            Holds = scalar * holds,
            BlownSaves = scalar * blownSaves,
            EarnedRuns = scalar * earnedRuns,
            Whip = whip,
            BattersFaced = scalar * battersFaced,
            Outs = scalar * outs,
            GamesPitched = scalar * gamesPitched,
            CompleteGames = scalar * completeGames,
            Shutouts = scalar * shutouts,
            Strikes = scalar * strikes,
            StrikePercentage = strikePercentage,
            HitBatsmen = scalar * hitBatsmen,
            Balks = scalar * balks,
            WildPitches = scalar * wildPitches,
            Pickoffs = scalar * pickoffs,
            TotalBases = scalar * totalBases,
            GroundOutsToAirOuts = groundOutsToAirOuts,
            WinPercentage = winPercentage,
            PitchesPerInning = pitchesPerInning,
            GamesFinished = scalar * gamesFinished,
            StrikeoutWalkRatio = strikeoutWalkRatio,
            StrikeoutsPer9Inn = strikeoutsPer9Inn,
            WalksPer9Inn = walksPer9Inn,
            HitsPer9Inn = hitsPer9Inn,
            RunsScoredPer9 = runsScoredPer9,
            HomeRunsPer9 = homeRunsPer9,
            InheritedRunners = scalar * inheritedRunners,
            InheritedRunnersScored = scalar * inheritedRunnersScored,
            CatcherInterferences = scalar * catcherInterferences,
            SacBunts = scalar * sacBunts,
            SacFlies = scalar * sacFlies
        };
    }

    public static FieldingStatsDto FakeFieldingStatsDto(int gamesPlayed = 1,
        int gamesStarted = 2,
        int caughtStealing = 3,
        int stolenBases = 4,
        string stolenBasePercentage = "",
        int assists = 5,
        int putouts = 6,
        int errors = 7,
        int chances = 8,
        string fielding = "",
        PositionDto position = default,
        string rangeFactorPerGame = "",
        string rangeFactorPer9Inn = "",
        string innings = "1",
        int games = 9,
        int passedBall = 10,
        int doublePlays = 11,
        int triplePlays = 12,
        string catcherEra = "",
        int catcherInterferences = 13,
        int wildPitches = 14,
        int throwingErrors = 15,
        int pickoffs = 16,
        int scalar = 1)
    {
        return new FieldingStatsDto(
            GamesPlayed: scalar * gamesPlayed,
            GamesStarted: scalar * gamesStarted,
            CaughtStealing: scalar * caughtStealing,
            StolenBases: scalar * stolenBases,
            StolenBasePercentage: stolenBasePercentage,
            Assists: scalar * assists,
            Putouts: scalar * putouts,
            Errors: scalar * errors,
            Chances: scalar * chances,
            Fielding: fielding,
            Position: position == default ? new PositionDto("Catcher", "C") : position,
            RangeFactorPerGame: rangeFactorPerGame,
            RangeFactorPer9Inn: rangeFactorPer9Inn,
            Innings: innings,
            Games: scalar * games,
            PassedBall: scalar * passedBall,
            DoublePlays: scalar * doublePlays,
            TriplePlays: scalar * triplePlays,
            CatcherEra: catcherEra,
            CatcherInterferences: scalar * catcherInterferences,
            WildPitches: scalar * wildPitches,
            ThrowingErrors: scalar * throwingErrors,
            Pickoffs: scalar * pickoffs
        );
    }
}