namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.TestClasses;

/// <summary>
/// Test stats
/// </summary>
public static class TestStats
{
    public static class Batting
    {
        public const int PlateAppearances = 105;
        public const int AtBats = 84;

        public const int Runs = 20;
        public const int Hits = 50;
        public const int Doubles = 13;
        public const int Triples = 12;
        public const int HomeRuns = 11;
        public const int RunsBattedIn = 21;

        public const int Strikeouts = 10;
        public const int GroundOuts = 17;
        public const int GroundIntoDoublePlays = 9;
        public const int GroundIntoTriplePlays = 8;
        public const int AirOuts = 7;

        public const int BaseOnBalls = 6;
        public const int IntentionalWalks = 5;
        public const int HitByPitch = 4;
        public const int SacrificeBunts = 3;
        public const int SacrificeFlies = 2;
        public const int CatcherInterferences = 1;

        public const int LeftOnBase = 30;
        public const int NumberOfPitchesSeen = 100;
        public const int StolenBases = 31;
        public const int CaughtStealing = 32;

        public const decimal BattingAverage = 0.595m;
        public const decimal OnBasePercentage = 0.588m;
        public const decimal Slugging = 1.429m;
        public const decimal OnBasePlusSlugging = 2.017m;
    }

    public static class Pitching
    {
        public const int InningsPitched = 20;
        public const int BattersFaced = 100;
        public const int AtBats = 80;

        public const int Hits = 10;
        public const int Doubles = 2;
        public const int Triples = 3;
        public const int HomeRuns = 4;
        public const int Runs = 5;
        public const int EarnedRuns = 4;

        public const int Strikeouts = 5;
        public const int GroundIntoDoublePlays = 6;
        public const int GroundOuts = 7;
        public const int AirOuts = 8;
        public const int Outs = 60;

        public const int BaseOnBalls = 10;
        public const int IntentionalWalks = 5;
        public const int HitBatsmen = 5;
        public const int SacrificeBunts = 3;
        public const int SacrificeFlies = 2;
        public const int CatcherInterferences = 1;

        public const int NumberOfPitches = 100;
        public const int Strikes = 50;
        public const int WildPitches = 7;
        public const int Balks = 6;

        public const int StolenBases = 5;
        public const int CaughtStealing = 4;
        public const int Pickoffs = 3;
        public const int InheritedRunners = 2;
        public const int InheritedRunnersScored = 1;
    }

    public static class Fielding
    {
        public const int InningsPlayed = 7;
        public const int Assists = 6;
        public const int Putouts = 5;
        public const int Errors = 4;
        public const int ThrowingErrors = 3;
        public const int DoublePlays = 2;
        public const int TriplePlays = 1;

        public const int StolenBases = 15;
        public const int CaughtStealing = 14;
        public const int Pickoffs = 13;

        public const int WildPitches = 12;
        public const int PassedBalls = 11;
        public const int CatcherInterferences = 10;
    }
}