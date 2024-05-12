using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerSeason FakePlayerSeason(int playerMlbId = 1, ushort seasonYear = 2024,
        List<PlayerGameBattingStats>? playerGameBattingStats = null,
        List<PlayerGamePitchingStats>? playerGamePitchingStats = null,
        List<PlayerGameFieldingStats>? playerGameFieldingStats = null)
    {
        return new PlayerSeason(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            playerGameBattingStats ?? new List<PlayerGameBattingStats>(),
            playerGamePitchingStats ?? new List<PlayerGamePitchingStats>(),
            playerGameFieldingStats ?? new List<PlayerGameFieldingStats>()
        );
    }

    public static PlayerGameBattingStats FakePlayerGameBattingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameMlbId = 10000, int teamMlbId = 100, int plateAppearances = 1, int atBats = 2,
        int runs = 3, int hits = 4, int doubles = 5, int triples = 6, int homeRuns = 7, int runsBattedIn = 8,
        int baseOnBalls = 9, int intentionalWalks = 10, int strikeouts = 11, int stolenBases = 12,
        int caughtStealing = 13, int hitByPitch = 14, int sacrificeBunts = 15, int sacrificeFlies = 16,
        int numberOfPitchesSeen = 17, int leftOnBase = 18, int groundOuts = 19, int groundIntoDoublePlays = 20,
        int groundIntoTriplePlays = 21, int airOuts = 22, int catcherInterferences = 23, int scalar = 1)
    {
        return new PlayerGameBattingStats(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameMlbId), MlbId.Create(teamMlbId),
            PlateAppearances: NaturalNumber.Create(scalar * plateAppearances),
            AtBats: NaturalNumber.Create(scalar * atBats),
            Runs: NaturalNumber.Create(scalar * runs),
            Hits: NaturalNumber.Create(scalar * hits),
            Doubles: NaturalNumber.Create(scalar * doubles),
            Triples: NaturalNumber.Create(scalar * triples),
            HomeRuns: NaturalNumber.Create(scalar * homeRuns),
            RunsBattedIn: NaturalNumber.Create(scalar * runsBattedIn),
            BaseOnBalls: NaturalNumber.Create(scalar * baseOnBalls),
            IntentionalWalks: NaturalNumber.Create(scalar * intentionalWalks),
            Strikeouts: NaturalNumber.Create(scalar * strikeouts),
            StolenBases: NaturalNumber.Create(scalar * stolenBases),
            CaughtStealing: NaturalNumber.Create(scalar * caughtStealing),
            HitByPitch: NaturalNumber.Create(scalar * hitByPitch),
            SacrificeBunts: NaturalNumber.Create(scalar * sacrificeBunts),
            SacrificeFlies: NaturalNumber.Create(scalar * sacrificeFlies),
            NumberOfPitchesSeen: NaturalNumber.Create(scalar * numberOfPitchesSeen),
            LeftOnBase: NaturalNumber.Create(scalar * leftOnBase),
            GroundOuts: NaturalNumber.Create(scalar * groundOuts),
            GroundIntoDoublePlays: NaturalNumber.Create(scalar * groundIntoDoublePlays),
            GroundIntoTriplePlays: NaturalNumber.Create(scalar * groundIntoTriplePlays),
            AirOuts: NaturalNumber.Create(scalar * airOuts),
            CatcherInterferences: NaturalNumber.Create(scalar * catcherInterferences)
        );
    }

    public static PlayerGamePitchingStats FakePlayerGamePitchingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameMlbId = 10000, int teamMlbId = 100, bool win = false, bool loss = false,
        bool gameStarted = false, bool gameFinished = false, bool completeGame = false, bool shutout = false,
        bool hold = false, bool save = false, bool blownSave = false, bool saveOpportunity = false,
        decimal inningsPitched = 1, int hits = 2, int doubles = 3, int triples = 4, int homeRuns = 5, int runs = 6,
        int earnedRuns = 7, int strikeouts = 8, int baseOnBalls = 9, int intentionalWalks = 10, int hitBatsmen = 11,
        int outs = 12, int groundOuts = 13, int airOuts = 14, int groundIntoDoublePlays = 15, int numberOfPitches = 16,
        int strikes = 17, int wildPitches = 18, int balks = 19, int battersFaced = 20, int atBats = 21,
        int stolenBases = 22, int caughtStealing = 23, int pickoffs = 24, int inheritedRunners = 25,
        int inheritedRunnersScored = 26, int catcherInterferences = 27, int sacrificeBunts = 28,
        int sacrificeFlies = 29, int scalar = 1)
    {
        return new PlayerGamePitchingStats(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameMlbId), MlbId.Create(teamMlbId),
            Win: win,
            Loss: loss,
            GameStarted: gameStarted,
            GameFinished: gameFinished,
            CompleteGame: completeGame,
            Shutout: shutout,
            Hold: hold,
            Save: save,
            BlownSave: blownSave,
            SaveOpportunity: saveOpportunity,
            InningsPitched: InningsCount.Create(scalar * inningsPitched),
            Hits: NaturalNumber.Create(scalar * hits),
            Doubles: NaturalNumber.Create(scalar * doubles),
            Triples: NaturalNumber.Create(scalar * triples),
            HomeRuns: NaturalNumber.Create(scalar * homeRuns),
            Runs: NaturalNumber.Create(scalar * runs),
            EarnedRuns: NaturalNumber.Create(scalar * earnedRuns),
            Strikeouts: NaturalNumber.Create(scalar * strikeouts),
            BaseOnBalls: NaturalNumber.Create(scalar * baseOnBalls),
            IntentionalWalks: NaturalNumber.Create(scalar * intentionalWalks),
            HitBatsmen: NaturalNumber.Create(scalar * hitBatsmen),
            Outs: NaturalNumber.Create(scalar * outs),
            GroundOuts: NaturalNumber.Create(scalar * groundOuts),
            AirOuts: NaturalNumber.Create(scalar * airOuts),
            GroundIntoDoublePlays: NaturalNumber.Create(scalar * groundIntoDoublePlays),
            NumberOfPitches: NaturalNumber.Create(scalar * numberOfPitches),
            Strikes: NaturalNumber.Create(scalar * strikes),
            WildPitches: NaturalNumber.Create(scalar * wildPitches),
            Balks: NaturalNumber.Create(scalar * balks),
            BattersFaced: NaturalNumber.Create(scalar * battersFaced),
            AtBats: NaturalNumber.Create(scalar * atBats),
            StolenBases: NaturalNumber.Create(scalar * stolenBases),
            CaughtStealing: NaturalNumber.Create(scalar * caughtStealing),
            Pickoffs: NaturalNumber.Create(scalar * pickoffs),
            InheritedRunners: NaturalNumber.Create(scalar * inheritedRunners),
            InheritedRunnersScored: NaturalNumber.Create(scalar * inheritedRunnersScored),
            CatcherInterferences: NaturalNumber.Create(scalar * catcherInterferences),
            SacrificeBunts: NaturalNumber.Create(scalar * sacrificeBunts),
            SacrificeFlies: NaturalNumber.Create(scalar * sacrificeFlies)
        );
    }

    public static PlayerGameFieldingStats FakePlayerGameFieldingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameMlbId = 10000, int teamMlbId = 100, Position position = Position.Catcher,
        bool gameStarted = false, decimal inningsPlayed = 1, int assists = 2, int putouts = 3, int errors = 4,
        int throwingErrors = 5, int doublePlays = 6, int triplePlays = 7, int caughtStealing = 8, int stolenBases = 9,
        int passedBalls = 10, int catcherInterferences = 11, int wildPitches = 12, int pickoffs = 13, int scalar = 1)
    {
        return new PlayerGameFieldingStats(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameMlbId), MlbId.Create(teamMlbId),
            Position: position,
            GameStarted: gameStarted,
            InningsPlayed: InningsCount.Create(scalar * inningsPlayed),
            Assists: NaturalNumber.Create(scalar * assists),
            Putouts: NaturalNumber.Create(scalar * putouts),
            Errors: NaturalNumber.Create(scalar * errors),
            ThrowingErrors: NaturalNumber.Create(scalar * throwingErrors),
            DoublePlays: NaturalNumber.Create(scalar * doublePlays),
            TriplePlays: NaturalNumber.Create(scalar * triplePlays),
            CaughtStealing: NaturalNumber.Create(scalar * caughtStealing),
            StolenBases: NaturalNumber.Create(scalar * stolenBases),
            PassedBalls: NaturalNumber.Create(scalar * passedBalls),
            CatcherInterferences: NaturalNumber.Create(scalar * catcherInterferences),
            WildPitches: NaturalNumber.Create(scalar * wildPitches),
            Pickoffs: NaturalNumber.Create(scalar * pickoffs)
        );
    }
}