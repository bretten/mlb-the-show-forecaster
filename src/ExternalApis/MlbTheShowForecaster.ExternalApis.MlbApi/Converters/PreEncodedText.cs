using System.Text.Json;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters;

/// <summary>
/// Performance wise, when writing JSON using <see cref="Utf8JsonWriter"/> it is better to pre-encode known property names:
/// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-utf8jsonwriter#write-with-utf-8-text
/// </summary>
public static class PreEncodedText
{
    public static class General
    {
        public static readonly JsonEncodedText Id = JsonEncodedText.Encode("id");
        public static readonly JsonEncodedText Group = JsonEncodedText.Encode("group");
        public static readonly JsonEncodedText DisplayName = JsonEncodedText.Encode("displayName");
        public static readonly JsonEncodedText Splits = JsonEncodedText.Encode("splits");
        public static readonly JsonEncodedText Season = JsonEncodedText.Encode("season");
        public static readonly JsonEncodedText Stat = JsonEncodedText.Encode("stat");
        public static readonly JsonEncodedText Date = JsonEncodedText.Encode("date");
        public static readonly JsonEncodedText GameType = JsonEncodedText.Encode("gameType");
        public static readonly JsonEncodedText IsHome = JsonEncodedText.Encode("isHome");
        public static readonly JsonEncodedText IsWin = JsonEncodedText.Encode("isWin");
        public static readonly JsonEncodedText Team = JsonEncodedText.Encode("team");
        public static readonly JsonEncodedText Game = JsonEncodedText.Encode("game");
        public static readonly JsonEncodedText GamePk = JsonEncodedText.Encode("gamePk");
        public static readonly JsonEncodedText Name = JsonEncodedText.Encode("name");
        public static readonly JsonEncodedText Abbreviation = JsonEncodedText.Encode("abbreviation");
    }

    public static class Hitting
    {
        public static readonly JsonEncodedText DisplayName = JsonEncodedText.Encode("hitting");
        public static readonly JsonEncodedText Summary = JsonEncodedText.Encode("summary");
        public static readonly JsonEncodedText GamesPlayed = JsonEncodedText.Encode("gamesPlayed");
        public static readonly JsonEncodedText GroundOuts = JsonEncodedText.Encode("groundOuts");
        public static readonly JsonEncodedText AirOuts = JsonEncodedText.Encode("airOuts");
        public static readonly JsonEncodedText Runs = JsonEncodedText.Encode("runs");
        public static readonly JsonEncodedText Doubles = JsonEncodedText.Encode("doubles");
        public static readonly JsonEncodedText Triples = JsonEncodedText.Encode("triples");
        public static readonly JsonEncodedText HomeRuns = JsonEncodedText.Encode("homeRuns");
        public static readonly JsonEncodedText StrikeOuts = JsonEncodedText.Encode("strikeOuts");
        public static readonly JsonEncodedText BaseOnBalls = JsonEncodedText.Encode("baseOnBalls");
        public static readonly JsonEncodedText IntentionalWalks = JsonEncodedText.Encode("intentionalWalks");
        public static readonly JsonEncodedText Hits = JsonEncodedText.Encode("hits");
        public static readonly JsonEncodedText HitByPitch = JsonEncodedText.Encode("hitByPitch");
        public static readonly JsonEncodedText Avg = JsonEncodedText.Encode("avg");
        public static readonly JsonEncodedText AtBats = JsonEncodedText.Encode("atBats");
        public static readonly JsonEncodedText Obp = JsonEncodedText.Encode("obp");
        public static readonly JsonEncodedText Slg = JsonEncodedText.Encode("slg");
        public static readonly JsonEncodedText Ops = JsonEncodedText.Encode("ops");
        public static readonly JsonEncodedText CaughtStealing = JsonEncodedText.Encode("caughtStealing");
        public static readonly JsonEncodedText StolenBases = JsonEncodedText.Encode("stolenBases");
        public static readonly JsonEncodedText StolenBasePercentage = JsonEncodedText.Encode("stolenBasePercentage");
        public static readonly JsonEncodedText GroundIntoDoublePlay = JsonEncodedText.Encode("groundIntoDoublePlay");
        public static readonly JsonEncodedText GroundIntoTriplePlay = JsonEncodedText.Encode("groundIntoTriplePlay");
        public static readonly JsonEncodedText NumberOfPitches = JsonEncodedText.Encode("numberOfPitches");
        public static readonly JsonEncodedText PlateAppearances = JsonEncodedText.Encode("plateAppearances");
        public static readonly JsonEncodedText TotalBases = JsonEncodedText.Encode("totalBases");
        public static readonly JsonEncodedText Rbi = JsonEncodedText.Encode("rbi");
        public static readonly JsonEncodedText LeftOnBase = JsonEncodedText.Encode("leftOnBase");
        public static readonly JsonEncodedText SacBunts = JsonEncodedText.Encode("sacBunts");
        public static readonly JsonEncodedText SacFlies = JsonEncodedText.Encode("sacFlies");
        public static readonly JsonEncodedText Babip = JsonEncodedText.Encode("babip");
        public static readonly JsonEncodedText GroundOutsToAirouts = JsonEncodedText.Encode("groundOutsToAirouts");
        public static readonly JsonEncodedText CatchersInterference = JsonEncodedText.Encode("catchersInterference");
        public static readonly JsonEncodedText AtBatsPerHomeRun = JsonEncodedText.Encode("atBatsPerHomeRun");
    }

    public static class Pitching
    {
        public static readonly JsonEncodedText DisplayName = JsonEncodedText.Encode("hitting");
        public static readonly JsonEncodedText Summary = JsonEncodedText.Encode("summary");
        public static readonly JsonEncodedText GamesPlayed = JsonEncodedText.Encode("gamesPlayed");
        public static readonly JsonEncodedText GamesStarted = JsonEncodedText.Encode("gamesStarted");
        public static readonly JsonEncodedText GroundOuts = JsonEncodedText.Encode("groundOuts");
        public static readonly JsonEncodedText AirOuts = JsonEncodedText.Encode("airOuts");
        public static readonly JsonEncodedText Runs = JsonEncodedText.Encode("runs");
        public static readonly JsonEncodedText Doubles = JsonEncodedText.Encode("doubles");
        public static readonly JsonEncodedText Triples = JsonEncodedText.Encode("triples");
        public static readonly JsonEncodedText HomeRuns = JsonEncodedText.Encode("homeRuns");
        public static readonly JsonEncodedText StrikeOuts = JsonEncodedText.Encode("strikeOuts");
        public static readonly JsonEncodedText BaseOnBalls = JsonEncodedText.Encode("baseOnBalls");
        public static readonly JsonEncodedText IntentionalWalks = JsonEncodedText.Encode("intentionalWalks");
        public static readonly JsonEncodedText Hits = JsonEncodedText.Encode("hits");
        public static readonly JsonEncodedText HitByPitch = JsonEncodedText.Encode("hitByPitch");
        public static readonly JsonEncodedText Avg = JsonEncodedText.Encode("avg");
        public static readonly JsonEncodedText AtBats = JsonEncodedText.Encode("atBats");
        public static readonly JsonEncodedText Obp = JsonEncodedText.Encode("obp");
        public static readonly JsonEncodedText Slg = JsonEncodedText.Encode("slg");
        public static readonly JsonEncodedText Ops = JsonEncodedText.Encode("ops");
        public static readonly JsonEncodedText CaughtStealing = JsonEncodedText.Encode("caughtStealing");
        public static readonly JsonEncodedText StolenBases = JsonEncodedText.Encode("stolenBases");
        public static readonly JsonEncodedText StolenBasePercentage = JsonEncodedText.Encode("stolenBasePercentage");
        public static readonly JsonEncodedText GroundIntoDoublePlay = JsonEncodedText.Encode("groundIntoDoublePlay");
        public static readonly JsonEncodedText NumberOfPitches = JsonEncodedText.Encode("numberOfPitches");
        public static readonly JsonEncodedText Era = JsonEncodedText.Encode("era");
        public static readonly JsonEncodedText InningsPitched = JsonEncodedText.Encode("inningsPitched");
        public static readonly JsonEncodedText Wins = JsonEncodedText.Encode("wins");
        public static readonly JsonEncodedText Losses = JsonEncodedText.Encode("losses");
        public static readonly JsonEncodedText Saves = JsonEncodedText.Encode("saves");
        public static readonly JsonEncodedText SaveOpportunities = JsonEncodedText.Encode("saveOpportunities");
        public static readonly JsonEncodedText Holds = JsonEncodedText.Encode("holds");
        public static readonly JsonEncodedText BlownSaves = JsonEncodedText.Encode("blownSaves");
        public static readonly JsonEncodedText EarnedRuns = JsonEncodedText.Encode("earnedRuns");
        public static readonly JsonEncodedText Whip = JsonEncodedText.Encode("whip");
        public static readonly JsonEncodedText BattersFaced = JsonEncodedText.Encode("battersFaced");
        public static readonly JsonEncodedText Outs = JsonEncodedText.Encode("outs");
        public static readonly JsonEncodedText GamesPitched = JsonEncodedText.Encode("gamesPitched");
        public static readonly JsonEncodedText CompleteGames = JsonEncodedText.Encode("completeGames");
        public static readonly JsonEncodedText Shutouts = JsonEncodedText.Encode("shutouts");
        public static readonly JsonEncodedText Strikes = JsonEncodedText.Encode("strikes");
        public static readonly JsonEncodedText StrikePercentage = JsonEncodedText.Encode("strikePercentage");
        public static readonly JsonEncodedText HitBatsmen = JsonEncodedText.Encode("hitBatsmen");
        public static readonly JsonEncodedText Balks = JsonEncodedText.Encode("balks");
        public static readonly JsonEncodedText WildPitches = JsonEncodedText.Encode("wildPitches");
        public static readonly JsonEncodedText Pickoffs = JsonEncodedText.Encode("pickoffs");
        public static readonly JsonEncodedText TotalBases = JsonEncodedText.Encode("totalBases");
        public static readonly JsonEncodedText GroundOutsToAirouts = JsonEncodedText.Encode("groundOutsToAirouts");
        public static readonly JsonEncodedText WinPercentage = JsonEncodedText.Encode("winPercentage");
        public static readonly JsonEncodedText PitchesPerInning = JsonEncodedText.Encode("pitchesPerInning");
        public static readonly JsonEncodedText GamesFinished = JsonEncodedText.Encode("gamesFinished");
        public static readonly JsonEncodedText StrikeoutWalkRatio = JsonEncodedText.Encode("strikeoutWalkRatio");
        public static readonly JsonEncodedText StrikeoutsPer9Inn = JsonEncodedText.Encode("strikeoutsPer9Inn");
        public static readonly JsonEncodedText WalksPer9Inn = JsonEncodedText.Encode("walksPer9Inn");
        public static readonly JsonEncodedText HitsPer9Inn = JsonEncodedText.Encode("hitsPer9Inn");
        public static readonly JsonEncodedText RunsScoredPer9 = JsonEncodedText.Encode("runsScoredPer9");
        public static readonly JsonEncodedText HomeRunsPer9 = JsonEncodedText.Encode("homeRunsPer9");
        public static readonly JsonEncodedText InheritedRunners = JsonEncodedText.Encode("inheritedRunners");

        public static readonly JsonEncodedText
            InheritedRunnersScored = JsonEncodedText.Encode("inheritedRunnersScored");

        public static readonly JsonEncodedText CatchersInterference = JsonEncodedText.Encode("catchersInterference");
        public static readonly JsonEncodedText SacBunts = JsonEncodedText.Encode("sacBunts");
        public static readonly JsonEncodedText SacFlies = JsonEncodedText.Encode("sacFlies");
    }

    public static class Fielding
    {
        public static readonly JsonEncodedText DisplayName = JsonEncodedText.Encode("fielding");
        public static readonly JsonEncodedText GamesPlayed = JsonEncodedText.Encode("gamesPlayed");
        public static readonly JsonEncodedText GamesStarted = JsonEncodedText.Encode("gamesStarted");
        public static readonly JsonEncodedText CaughtStealing = JsonEncodedText.Encode("caughtStealing");
        public static readonly JsonEncodedText StolenBases = JsonEncodedText.Encode("stolenBases");
        public static readonly JsonEncodedText StolenBasePercentage = JsonEncodedText.Encode("stolenBasePercentage");
        public static readonly JsonEncodedText Assists = JsonEncodedText.Encode("assists");
        public static readonly JsonEncodedText PutOuts = JsonEncodedText.Encode("putOuts");
        public static readonly JsonEncodedText Errors = JsonEncodedText.Encode("errors");
        public static readonly JsonEncodedText Chances = JsonEncodedText.Encode("chances");
        public static readonly JsonEncodedText FieldingPercentage = JsonEncodedText.Encode("fielding");
        public static readonly JsonEncodedText Position = JsonEncodedText.Encode("position");
        public static readonly JsonEncodedText RangeFactorPerGame = JsonEncodedText.Encode("rangeFactorPerGame");
        public static readonly JsonEncodedText RangeFactorPer9Inn = JsonEncodedText.Encode("rangeFactorPer9Inn");
        public static readonly JsonEncodedText Innings = JsonEncodedText.Encode("innings");
        public static readonly JsonEncodedText Games = JsonEncodedText.Encode("games");
        public static readonly JsonEncodedText PassedBall = JsonEncodedText.Encode("passedBall");
        public static readonly JsonEncodedText DoublePlays = JsonEncodedText.Encode("doublePlays");
        public static readonly JsonEncodedText TriplePlays = JsonEncodedText.Encode("triplePlays");
        public static readonly JsonEncodedText CatcherEra = JsonEncodedText.Encode("catcherERA");
        public static readonly JsonEncodedText CatchersInterference = JsonEncodedText.Encode("catchersInterference");
        public static readonly JsonEncodedText WildPitches = JsonEncodedText.Encode("wildPitches");
        public static readonly JsonEncodedText ThrowingErrors = JsonEncodedText.Encode("throwingErrors");
        public static readonly JsonEncodedText Pickoffs = JsonEncodedText.Encode("pickoffs");
    }
}