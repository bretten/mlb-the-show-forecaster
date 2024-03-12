using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.BaseRunning;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

/// <summary>
/// Statistics for pitching
/// </summary>
public class PitchingStats : ValueObject
{
    /// <summary>
    /// The number of wins
    /// </summary>
    public NaturalNumber Wins { get; }

    /// <summary>
    /// The number of losses
    /// </summary>
    public NaturalNumber Losses { get; }

    /// <summary>
    /// The number of games started as a pitcher
    /// </summary>
    public NaturalNumber GamesStarted { get; }

    /// <summary>
    /// The number of times the pitcher was the last pitcher in the game as a relief pitcher
    /// </summary>
    public NaturalNumber GamesFinished { get; }

    /// <summary>
    /// The number of complete games pitched
    /// </summary>
    public NaturalNumber CompleteGames { get; }

    /// <summary>
    /// The number of shutouts
    /// </summary>
    public NaturalNumber Shutouts { get; }

    /// <summary>
    /// The number of holds
    /// </summary>
    public NaturalNumber Holds { get; }

    /// <summary>
    /// The number of saves
    /// </summary>
    public NaturalNumber Saves { get; }

    /// <summary>
    /// The number of blown saves
    /// </summary>
    public NaturalNumber BlownSaves { get; }

    /// <summary>
    /// The number of save opportunities
    /// </summary>
    public NaturalNumber SaveOpportunities { get; }

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public InningsCount InningsPitched { get; }

    /// <summary>
    /// The number of hits given up
    /// </summary>
    public NaturalNumber Hits { get; }

    /// <summary>
    /// The number of doubles given up
    /// </summary>
    public NaturalNumber Doubles { get; }

    /// <summary>
    /// The number of triples given up
    /// </summary>
    public NaturalNumber Triples { get; }

    /// <summary>
    /// The number of home runs given up
    /// </summary>
    public NaturalNumber HomeRuns { get; }

    /// <summary>
    /// The number of runs given up
    /// </summary>
    public NaturalNumber Runs { get; }

    /// <summary>
    /// The number of earned runs given up (runs that were a result of this pitcher giving up a hit)
    /// </summary>
    public NaturalNumber EarnedRuns { get; }

    /// <summary>
    /// The number of strikeouts
    /// </summary>
    public NaturalNumber Strikeouts { get; }

    /// <summary>
    /// The number of times the pitcher walked the batter
    /// </summary>
    public NaturalNumber BaseOnBalls { get; }

    /// <summary>
    /// The number of times the pitcher intentionally walked the batter
    /// </summary>
    public NaturalNumber IntentionalWalks { get; }

    /// <summary>
    /// The number of times the pitcher hit a batter with a pitch
    /// </summary>
    public NaturalNumber HitBatsmen { get; }

    /// <summary>
    /// The number of outs made by the team while this pitcher was active
    /// </summary>
    public NaturalNumber Outs { get; }

    /// <summary>
    /// The number of times a pitch resulted in a ground out
    /// </summary>
    public NaturalNumber GroundOuts { get; }

    /// <summary>
    /// The number of times a pitch resulted in a air/fly out
    /// </summary>
    public NaturalNumber AirOuts { get; }

    /// <summary>
    /// The number of double play ground outs induced
    /// </summary>
    public NaturalNumber GroundIntoDoublePlays { get; }

    /// <summary>
    /// The number of pitches thrown this game
    /// </summary>
    public NaturalNumber NumberOfPitches { get; }

    /// <summary>
    /// The number of strikes thrown by the pitcher
    /// </summary>
    public NaturalNumber Strikes { get; }

    /// <summary>
    /// The number of wild pitches thrown
    /// </summary>
    public NaturalNumber WildPitches { get; }

    /// <summary>
    /// The number of balks
    /// </summary>
    public NaturalNumber Balks { get; }

    /// <summary>
    /// The number of batters faced, pitcher version of plate appearance
    /// </summary>
    public NaturalNumber BattersFaced { get; }

    /// <summary>
    /// The number of at-bats
    /// </summary>
    public NaturalNumber AtBats { get; }

    /// <summary>
    /// The number of bases stolen against this pitcher
    /// </summary>
    public NaturalNumber StolenBases { get; }

    /// <summary>
    /// The number of times a runner was caught stealing against this pitcher
    /// </summary>
    public NaturalNumber CaughtStealing { get; }

    /// <summary>
    /// The number of pick offs made by this pitcher
    /// </summary>
    public NaturalNumber Pickoffs { get; }

    /// <summary>
    /// The number of runners on base when the pitcher enters the game
    /// </summary>
    public NaturalNumber InheritedRunners { get; }

    /// <summary>
    /// The number of inherited runners allowed to score
    /// </summary>
    public NaturalNumber InheritedRunnersScored { get; }

    /// <summary>
    /// The number of times a catcher interfered with the batter's plate appearance
    /// </summary>
    public NaturalNumber CatcherInterferences { get; }

    /// <summary>
    /// The number of sacrifice bunts made against the pitcher
    /// </summary>
    public NaturalNumber SacrificeBunts { get; }

    /// <summary>
    /// The number of sacrifice flies made against the pitcher
    /// </summary>
    public NaturalNumber SacrificeFlies { get; }

    /// <summary>
    /// Quality start
    /// </summary>
    public QualityStart QualityStart => QualityStart.Create(InningsPitched.Value, EarnedRuns.Value);

    /// <summary>
    /// The pitcher's earned run average
    /// </summary>
    public EarnedRunAverage EarnedRunAverage => EarnedRunAverage.Create(EarnedRuns.Value, InningsPitched.Value);

    /// <summary>
    /// Opponents' batting average
    /// </summary>
    public OpponentsBattingAverage OpponentsBattingAverage => OpponentsBattingAverage.Create(
        Hits.Value, BattersFaced.Value, BaseOnBalls.Value, HitBatsmen.Value, SacrificeBunts.Value,
        SacrificeFlies.Value, CatcherInterferences.Value);

    /// <summary>
    /// Opponents batting average
    /// </summary>
    public OnBasePercentage OpponentsOnBasePercentage => OnBasePercentage.Create(Hits.Value, BaseOnBalls.Value,
        HitBatsmen.Value, AtBats.Value, SacrificeFlies.Value);

    /// <summary>
    /// Total bases
    /// </summary>
    public TotalBases TotalBases => TotalBases.CreateWithHits(Hits.Value, Doubles.Value, Triples.Value, HomeRuns.Value);

    /// <summary>
    /// Opponents' slugging
    /// </summary>
    public Slugging Slugging => Slugging.Create(TotalBases, AtBats.Value);

    /// <summary>
    /// Opponents' on-base plus slugging
    /// </summary>
    public OnBasePlusSlugging OpponentsOnBasePlusSlugging =>
        OnBasePlusSlugging.Create(OpponentsOnBasePercentage, Slugging);

    /// <summary>
    /// Pitches per inning
    /// </summary>
    public PitchesPerInning PitchesPerInning => PitchesPerInning.Create(NumberOfPitches.Value, InningsPitched.Value);

    /// <summary>
    /// Strike percentage
    /// </summary>
    public StrikePercentage StrikePercentage => StrikePercentage.Create(Strikes.Value, NumberOfPitches.Value);

    /// <summary>
    /// Walks plus hits per inning pitched
    /// </summary>
    public WalksPlusHitsPerInningPitched WalksPlusHitsPerInningPitched =>
        WalksPlusHitsPerInningPitched.Create(BaseOnBalls.Value, Hits.Value, InningsPitched.Value);

    /// <summary>
    /// Strikeout to walk ratio
    /// </summary>
    public StrikeoutToWalkRatio StrikeoutToWalkRatio =>
        StrikeoutToWalkRatio.Create(Strikeouts.Value, BaseOnBalls.Value);

    /// <summary>
    /// Hits per 9 innings
    /// </summary>
    public HitsPerNine HitsPer9 => HitsPerNine.Create(Hits.Value, InningsPitched.Value);

    /// <summary>
    /// Strikeouts per 9 innings
    /// </summary>
    public StrikeoutsPerNine StrikeoutsPer9 => StrikeoutsPerNine.Create(Strikeouts.Value, InningsPitched.Value);

    /// <summary>
    /// Walks per 9 innings
    /// </summary>
    public BaseOnBallsPerNine BaseOnBallsPer9 => BaseOnBallsPerNine.Create(BaseOnBalls.Value, InningsPitched.Value);

    /// <summary>
    /// Runs scored per 9 innings
    /// </summary>
    public RunsScoredPerNine RunsScoredPer9 => RunsScoredPerNine.Create(Runs.Value, InningsPitched.Value);

    /// <summary>
    /// HR per 9 innings
    /// </summary>
    public HomeRunsPerNine HomeRunsPer9 => HomeRunsPerNine.Create(HomeRuns.Value, InningsPitched.Value);

    /// <summary>
    /// Stolen base percentage
    /// </summary>
    public StolenBasePercentage StolenBasePercentage =>
        StolenBasePercentage.Create(StolenBases.Value, CaughtStealing.Value);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="wins">The number of wins</param>
    /// <param name="losses">The number of losses</param>
    /// <param name="gamesStarted">The number of games started as a pitcher</param>
    /// <param name="gamesFinished">The number of times the pitcher was the last pitcher in the game as a relief pitcher</param>
    /// <param name="completeGames">The number of complete games pitched</param>
    /// <param name="shutouts">The number of shutouts</param>
    /// <param name="holds">The number of holds</param>
    /// <param name="saves">The number of saves</param>
    /// <param name="blownSaves">The number of blown saves</param>
    /// <param name="saveOpportunities">The number of save opportunities</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <param name="hits">The number of hits given up</param>
    /// <param name="doubles">The number of doubles given up</param>
    /// <param name="triples">The number of triples given up</param>
    /// <param name="homeRuns">The number of home runs given up</param>
    /// <param name="runs">The number of runs given up</param>
    /// <param name="earnedRuns">The number of earned runs given up (runs that were a result of this pitcher giving up a hit)</param>
    /// <param name="strikeouts">The number of strikeouts</param>
    /// <param name="baseOnBalls">The number of times the pitcher walked the batter</param>
    /// <param name="intentionalWalks">The number of times the pitcher intentionally walked the batter</param>
    /// <param name="hitBatsmen">The number of times the pitcher hit a batter with a pitch</param>
    /// <param name="outs">The number of outs made by the team while this pitcher was active</param>
    /// <param name="groundOuts">The number of times a pitch resulted in a ground out</param>
    /// <param name="airOuts">The number of times a pitch resulted in a air/fly out</param>
    /// <param name="groundIntoDoublePlays">The number of double play ground outs induced</param>
    /// <param name="numberOfPitches">The number of pitches thrown this game</param>
    /// <param name="strikes">The number of strikes thrown by the pitcher</param>
    /// <param name="wildPitches">The number of wild pitches thrown</param>
    /// <param name="balks">The number of balks</param>
    /// <param name="battersFaced">The number of batters faced, pitcher version of plate appearance</param>
    /// <param name="atBats">The number of at-bats</param>
    /// <param name="stolenBases">The number of bases stolen against this pitcher</param>
    /// <param name="caughtStealing">The number of times a runner was caught stealing against this pitcher</param>
    /// <param name="pickoffs">The number of pick offs made by this pitcher</param>
    /// <param name="inheritedRunners">The number of runners on base when the pitcher enters the game</param>
    /// <param name="inheritedRunnersScored">The number of inherited runners allowed to score</param>
    /// <param name="catcherInterferences">The number of times a catcher interfered with the batter's plate appearance</param>
    /// <param name="sacrificeBunts">The number of sacrifice bunts made against the pitcher</param>
    /// <param name="sacrificeFlies">The number of sacrifice flies made against the pitcher</param>
    protected PitchingStats(NaturalNumber wins, NaturalNumber losses, NaturalNumber gamesStarted,
        NaturalNumber gamesFinished, NaturalNumber completeGames, NaturalNumber shutouts, NaturalNumber holds,
        NaturalNumber saves, NaturalNumber blownSaves, NaturalNumber saveOpportunities, InningsCount inningsPitched,
        NaturalNumber hits, NaturalNumber doubles, NaturalNumber triples, NaturalNumber homeRuns, NaturalNumber runs,
        NaturalNumber earnedRuns, NaturalNumber strikeouts, NaturalNumber baseOnBalls, NaturalNumber intentionalWalks,
        NaturalNumber hitBatsmen, NaturalNumber outs, NaturalNumber groundOuts, NaturalNumber airOuts,
        NaturalNumber groundIntoDoublePlays, NaturalNumber numberOfPitches, NaturalNumber strikes,
        NaturalNumber wildPitches, NaturalNumber balks, NaturalNumber battersFaced, NaturalNumber atBats,
        NaturalNumber stolenBases, NaturalNumber caughtStealing, NaturalNumber pickoffs, NaturalNumber inheritedRunners,
        NaturalNumber inheritedRunnersScored, NaturalNumber catcherInterferences, NaturalNumber sacrificeBunts,
        NaturalNumber sacrificeFlies)
    {
        Wins = wins;
        Losses = losses;
        GamesStarted = gamesStarted;
        GamesFinished = gamesFinished;
        CompleteGames = completeGames;
        Shutouts = shutouts;
        Holds = holds;
        Saves = saves;
        BlownSaves = blownSaves;
        SaveOpportunities = saveOpportunities;
        InningsPitched = inningsPitched;
        Hits = hits;
        Doubles = doubles;
        Triples = triples;
        HomeRuns = homeRuns;
        Runs = runs;
        EarnedRuns = earnedRuns;
        Strikeouts = strikeouts;
        BaseOnBalls = baseOnBalls;
        IntentionalWalks = intentionalWalks;
        HitBatsmen = hitBatsmen;
        Outs = outs;
        GroundOuts = groundOuts;
        AirOuts = airOuts;
        GroundIntoDoublePlays = groundIntoDoublePlays;
        NumberOfPitches = numberOfPitches;
        Strikes = strikes;
        WildPitches = wildPitches;
        Balks = balks;
        BattersFaced = battersFaced;
        AtBats = atBats;
        StolenBases = stolenBases;
        CaughtStealing = caughtStealing;
        Pickoffs = pickoffs;
        InheritedRunners = inheritedRunners;
        InheritedRunnersScored = inheritedRunnersScored;
        CatcherInterferences = catcherInterferences;
        SacrificeBunts = sacrificeBunts;
        SacrificeFlies = sacrificeFlies;
    }

    /// <summary>
    /// Creates <see cref="PitchingStats"/>
    /// </summary>
    /// <param name="wins">The number of wins</param>
    /// <param name="losses">The number of losses</param>
    /// <param name="gamesStarted">The number of games started as a pitcher</param>
    /// <param name="gamesFinished">The number of times the pitcher was the last pitcher in the game as a relief pitcher</param>
    /// <param name="completeGames">The number of complete games pitched</param>
    /// <param name="shutouts">The number of shutouts</param>
    /// <param name="holds">The number of holds</param>
    /// <param name="saves">The number of saves</param>
    /// <param name="blownSaves">The number of blown saves</param>
    /// <param name="saveOpportunities">The number of save opportunities</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <param name="hits">The number of hits given up</param>
    /// <param name="doubles">The number of doubles given up</param>
    /// <param name="triples">The number of triples given up</param>
    /// <param name="homeRuns">The number of home runs given up</param>
    /// <param name="runs">The number of runs given up</param>
    /// <param name="earnedRuns">The number of earned runs given up (runs that were a result of this pitcher giving up a hit)</param>
    /// <param name="strikeouts">The number of strikeouts</param>
    /// <param name="baseOnBalls">The number of times the pitcher walked the batter</param>
    /// <param name="intentionalWalks">The number of times the pitcher intentionally walked the batter</param>
    /// <param name="hitBatsmen">The number of times the pitcher hit a batter with a pitch</param>
    /// <param name="outs">The number of outs made by the team while this pitcher was active</param>
    /// <param name="groundOuts">The number of times a pitch resulted in a ground out</param>
    /// <param name="airOuts">The number of times a pitch resulted in a air/fly out</param>
    /// <param name="groundIntoDoublePlays">The number of double play ground outs induced</param>
    /// <param name="numberOfPitches">The number of pitches thrown this game</param>
    /// <param name="strikes">The number of strikes thrown by the pitcher</param>
    /// <param name="wildPitches">The number of wild pitches thrown</param>
    /// <param name="balks">The number of balks</param>
    /// <param name="battersFaced">The number of batters faced, pitcher version of plate appearance</param>
    /// <param name="atBats">The number of at-bats</param>
    /// <param name="stolenBases">The number of bases stolen against this pitcher</param>
    /// <param name="caughtStealing">The number of times a runner was caught stealing against this pitcher</param>
    /// <param name="pickoffs">The number of pick offs made by this pitcher</param>
    /// <param name="inheritedRunners">The number of runners on base when the pitcher enters the game</param>
    /// <param name="inheritedRunnersScored">The number of inherited runners allowed to score</param>
    /// <param name="catcherInterferences">The number of times a catcher interfered with the batter's plate appearance</param>
    /// <param name="sacrificeBunts">The number of sacrifice bunts made against the pitcher</param>
    /// <param name="sacrificeFlies">The number of sacrifice flies made against the pitcher</param>
    /// <returns><see cref="PitchingStats"/></returns>
    public static PitchingStats Create(int wins, int losses, int gamesStarted, int gamesFinished, int completeGames,
        int shutouts, int holds, int saves, int blownSaves, int saveOpportunities, decimal inningsPitched, int hits,
        int doubles, int triples, int homeRuns, int runs, int earnedRuns, int strikeouts, int baseOnBalls,
        int intentionalWalks, int hitBatsmen, int outs, int groundOuts, int airOuts, int groundIntoDoublePlays,
        int numberOfPitches, int strikes, int wildPitches, int balks, int battersFaced, int atBats, int stolenBases,
        int caughtStealing, int pickoffs, int inheritedRunners, int inheritedRunnersScored, int catcherInterferences,
        int sacrificeBunts, int sacrificeFlies)
    {
        var w = NaturalNumber.Create(wins);
        var l = NaturalNumber.Create(losses);
        var gs = NaturalNumber.Create(gamesStarted);
        var gf = NaturalNumber.Create(gamesFinished);
        var cg = NaturalNumber.Create(completeGames);
        var sho = NaturalNumber.Create(shutouts);
        var hld = NaturalNumber.Create(holds);
        var sv = NaturalNumber.Create(saves);
        var bs = NaturalNumber.Create(blownSaves);
        var svo = NaturalNumber.Create(saveOpportunities);
        var ip = InningsCount.Create(inningsPitched);
        var h = NaturalNumber.Create(hits);
        var r = NaturalNumber.Create(runs);
        var er = NaturalNumber.Create(earnedRuns);
        var d = NaturalNumber.Create(doubles);
        var t = NaturalNumber.Create(triples);
        var hr = NaturalNumber.Create(homeRuns);
        var k = NaturalNumber.Create(strikeouts);
        var bb = NaturalNumber.Create(baseOnBalls);
        var ibb = NaturalNumber.Create(intentionalWalks);
        var hb = NaturalNumber.Create(hitBatsmen);
        var o = NaturalNumber.Create(outs);
        var go = NaturalNumber.Create(groundOuts);
        var ao = NaturalNumber.Create(airOuts);
        var gidp = NaturalNumber.Create(groundIntoDoublePlays);
        var np = NaturalNumber.Create(numberOfPitches);
        var strikeCount = NaturalNumber.Create(strikes);
        var wp = NaturalNumber.Create(wildPitches);
        var bk = NaturalNumber.Create(balks);
        var bf = NaturalNumber.Create(battersFaced);
        var ab = NaturalNumber.Create(atBats);
        var sb = NaturalNumber.Create(stolenBases);
        var cs = NaturalNumber.Create(caughtStealing);
        var pk = NaturalNumber.Create(pickoffs);
        var ir = NaturalNumber.Create(inheritedRunners);
        var irs = NaturalNumber.Create(inheritedRunnersScored);
        var ci = NaturalNumber.Create(catcherInterferences);
        var sh = NaturalNumber.Create(sacrificeBunts);
        var sf = NaturalNumber.Create(sacrificeFlies);
        return new PitchingStats(wins: w,
            losses: l,
            gamesStarted: gs,
            gamesFinished: gf,
            completeGames: cg,
            shutouts: sho,
            holds: hld,
            saves: sv,
            blownSaves: bs,
            saveOpportunities: svo,
            inningsPitched: ip,
            hits: h,
            doubles: d,
            triples: t,
            homeRuns: hr,
            runs: r,
            earnedRuns: er,
            strikeouts: k,
            baseOnBalls: bb,
            intentionalWalks: ibb,
            hitBatsmen: hb,
            outs: o,
            groundOuts: go,
            airOuts: ao,
            groundIntoDoublePlays: gidp,
            numberOfPitches: np,
            strikes: strikeCount,
            wildPitches: wp,
            balks: bk,
            battersFaced: bf,
            atBats: ab,
            stolenBases: sb,
            caughtStealing: cs,
            pickoffs: pk,
            inheritedRunners: ir,
            inheritedRunnersScored: irs,
            catcherInterferences: ci,
            sacrificeBunts: sh,
            sacrificeFlies: sf
        );
    }

    /// <summary>
    /// Creates an aggregate <see cref="PitchingStats"/>
    /// </summary>
    /// <param name="pitchingStatsCollection">A collection of pitching stats</param>
    /// <returns>Aggregated <see cref="PitchingStats"/></returns>
    public static PitchingStats Create(IEnumerable<PitchingStats> pitchingStatsCollection)
    {
        var pitchingStatsArray = pitchingStatsCollection as PitchingStats[] ?? pitchingStatsCollection.ToArray();
        return Create(wins: pitchingStatsArray.Sum(x => x.Wins.Value),
            losses: pitchingStatsArray.Sum(x => x.Losses.Value),
            gamesStarted: pitchingStatsArray.Sum(x => x.GamesStarted.Value),
            gamesFinished: pitchingStatsArray.Sum(x => x.GamesFinished.Value),
            completeGames: pitchingStatsArray.Sum(x => x.CompleteGames.Value),
            shutouts: pitchingStatsArray.Sum(x => x.Shutouts.Value),
            holds: pitchingStatsArray.Sum(x => x.Holds.Value),
            saves: pitchingStatsArray.Sum(x => x.Saves.Value),
            blownSaves: pitchingStatsArray.Sum(x => x.BlownSaves.Value),
            saveOpportunities: pitchingStatsArray.Sum(x => x.SaveOpportunities.Value),
            inningsPitched: pitchingStatsArray.Sum(x => x.InningsPitched.Value),
            hits: pitchingStatsArray.Sum(x => x.Hits.Value),
            doubles: pitchingStatsArray.Sum(x => x.Doubles.Value),
            triples: pitchingStatsArray.Sum(x => x.Triples.Value),
            homeRuns: pitchingStatsArray.Sum(x => x.HomeRuns.Value),
            runs: pitchingStatsArray.Sum(x => x.Runs.Value),
            earnedRuns: pitchingStatsArray.Sum(x => x.EarnedRuns.Value),
            strikeouts: pitchingStatsArray.Sum(x => x.Strikeouts.Value),
            baseOnBalls: pitchingStatsArray.Sum(x => x.BaseOnBalls.Value),
            intentionalWalks: pitchingStatsArray.Sum(x => x.IntentionalWalks.Value),
            hitBatsmen: pitchingStatsArray.Sum(x => x.HitBatsmen.Value),
            outs: pitchingStatsArray.Sum(x => x.Outs.Value),
            groundOuts: pitchingStatsArray.Sum(x => x.GroundOuts.Value),
            airOuts: pitchingStatsArray.Sum(x => x.AirOuts.Value),
            groundIntoDoublePlays: pitchingStatsArray.Sum(x => x.GroundIntoDoublePlays.Value),
            numberOfPitches: pitchingStatsArray.Sum(x => x.NumberOfPitches.Value),
            strikes: pitchingStatsArray.Sum(x => x.Strikes.Value),
            wildPitches: pitchingStatsArray.Sum(x => x.WildPitches.Value),
            balks: pitchingStatsArray.Sum(x => x.Balks.Value),
            battersFaced: pitchingStatsArray.Sum(x => x.BattersFaced.Value),
            atBats: pitchingStatsArray.Sum(x => x.AtBats.Value),
            stolenBases: pitchingStatsArray.Sum(x => x.StolenBases.Value),
            caughtStealing: pitchingStatsArray.Sum(x => x.CaughtStealing.Value),
            pickoffs: pitchingStatsArray.Sum(x => x.Pickoffs.Value),
            inheritedRunners: pitchingStatsArray.Sum(x => x.InheritedRunners.Value),
            inheritedRunnersScored: pitchingStatsArray.Sum(x => x.InheritedRunnersScored.Value),
            catcherInterferences: pitchingStatsArray.Sum(x => x.CatcherInterferences.Value),
            sacrificeBunts: pitchingStatsArray.Sum(x => x.SacrificeBunts.Value),
            sacrificeFlies: pitchingStatsArray.Sum(x => x.SacrificeFlies.Value)
        );
    }
}