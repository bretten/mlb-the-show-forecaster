using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.BaseRunning;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

/// <summary>
/// Statistics for batting
/// </summary>
public class BattingStats : ValueObject
{
    /// <summary>
    /// The number of plate appearances
    /// </summary>
    public NaturalNumber PlateAppearances { get; }

    /// <summary>
    /// The number of at bats
    /// </summary>
    public NaturalNumber AtBats { get; }

    /// <summary>
    /// The number of runs scored
    /// </summary>
    public NaturalNumber Runs { get; }

    /// <summary>
    /// The number of hits
    /// </summary>
    public NaturalNumber Hits { get; }

    /// <summary>
    /// The number of doubles
    /// </summary>
    public NaturalNumber Doubles { get; }

    /// <summary>
    /// The number of triples
    /// </summary>
    public NaturalNumber Triples { get; }

    /// <summary>
    /// The number of home runs
    /// </summary>
    public NaturalNumber HomeRuns { get; }

    /// <summary>
    /// The number of runs batted in
    /// </summary>
    public NaturalNumber RunsBattedIn { get; }

    /// <summary>
    /// The number of walks
    /// </summary>
    public NaturalNumber BaseOnBalls { get; }

    /// <summary>
    /// The number of intentional walks
    /// </summary>
    public NaturalNumber IntentionalWalks { get; }

    /// <summary>
    /// The number of strikeouts
    /// </summary>
    public NaturalNumber Strikeouts { get; }

    /// <summary>
    /// The number of stolen bases
    /// </summary>
    public NaturalNumber StolenBases { get; }

    /// <summary>
    /// The number of times caught stealing
    /// </summary>
    public NaturalNumber CaughtStealing { get; }

    /// <summary>
    /// The number of times the player was hit by a pitch
    /// </summary>
    public NaturalNumber HitByPitch { get; }

    /// <summary>
    /// The number of sacrifice bunts
    /// </summary>
    public NaturalNumber SacrificeBunts { get; }

    /// <summary>
    /// The number of sacrifice flies
    /// </summary>
    public NaturalNumber SacrificeFlies { get; }

    /// <summary>
    /// The number of pitches the player saw as a batter
    /// </summary>
    public NaturalNumber NumberOfPitchesSeen { get; }

    /// <summary>
    /// The number of runners the player did not advance when batting and their out results in the end of the inning
    /// </summary>
    public NaturalNumber LeftOnBase { get; }

    /// <summary>
    /// The number of times the batter grounded out
    /// </summary>
    public NaturalNumber GroundOuts { get; }

    /// <summary>
    /// The number of times the batter grounded into a double play
    /// </summary>
    public NaturalNumber GroundIntoDoublePlays { get; }

    /// <summary>
    /// The number of times the batter grounded into a triple play
    /// </summary>
    public NaturalNumber GroundIntoTriplePlays { get; }

    /// <summary>
    /// The number of times the batter hit a fly ball that led to an out
    /// </summary>
    public NaturalNumber AirOuts { get; }

    /// <summary>
    /// The number of times a catcher interfered with the batter's plate appearance
    /// </summary>
    public NaturalNumber CatchersInterference { get; }

    /// <summary>
    /// Batting average
    /// </summary>
    public BattingAverage BattingAverage => BattingAverage.Create(Hits.Value, AtBats.Value);

    /// <summary>
    /// On-base percentage
    /// </summary>
    public OnBasePercentage OnBasePercentage =>
        OnBasePercentage.Create(Hits.Value, BaseOnBalls.Value, HitByPitch.Value, AtBats.Value, SacrificeFlies.Value);

    /// <summary>
    /// Batting average on balls in play
    /// </summary>
    public BattingAverageOnBallsInPlay BattingAverageOnBallsInPlay =>
        BattingAverageOnBallsInPlay.Create(Hits.Value, HomeRuns.Value, AtBats.Value, Strikeouts.Value,
            SacrificeFlies.Value);

    /// <summary>
    /// Total bases
    /// </summary>
    public TotalBases TotalBases => TotalBases.CreateWithHits(Hits.Value, Doubles.Value, Triples.Value, HomeRuns.Value);

    /// <summary>
    /// Slugging
    /// </summary>
    public Slugging Slugging => Slugging.Create(TotalBases, AtBats.Value);

    /// <summary>
    /// On-base plus slugging
    /// </summary>
    public OnBasePlusSlugging OnBasePlusSlugging => OnBasePlusSlugging.Create(OnBasePercentage, Slugging);

    /// <summary>
    /// Stolen base percentage
    /// </summary>
    public StolenBasePercentage StolenBasePercentage =>
        StolenBasePercentage.Create(StolenBases.Value, CaughtStealing.Value);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="plateAppearances">The number of plate appearances</param>
    /// <param name="atBats">The number of at bats</param>
    /// <param name="runs">The number of runs scored</param>
    /// <param name="hits">The number of hits</param>
    /// <param name="doubles">The number of doubles</param>
    /// <param name="triples">The number of triples</param>
    /// <param name="homeRuns">The number of home runs</param>
    /// <param name="runsBattedIn">The number of runs batted in</param>
    /// <param name="baseOnBalls">The number of walks</param>
    /// <param name="intentionalWalks">The number of intentional walks</param>
    /// <param name="strikeouts">The number of strikeouts</param>
    /// <param name="stolenBases">The number of stolen bases</param>
    /// <param name="caughtStealing">The number of times caught stealing</param>
    /// <param name="hitByPitch">The number of times the player was hit by a pitch</param>
    /// <param name="sacrificeBunts">The number of sacrifice bunts</param>
    /// <param name="sacrificeFlies">The number of sacrifice flies</param>
    /// <param name="numberOfPitchesSeen">The number of pitches the player saw as a batter</param>
    /// <param name="leftOnBase">The number of runners the player did not advance when batting and their out results in the end of the inning</param>
    /// <param name="groundOuts">The number of times the batter grounded out</param>
    /// <param name="groundIntoDoublePlays">The number of times the batter grounded into a double play</param>
    /// <param name="groundIntoTriplePlays">The number of times the batter grounded into a triple play</param>
    /// <param name="airOuts">The number of times the batter hit a fly ball that led to an out</param>
    /// <param name="catchersInterference">The number of times a catcher interfered with the batter's plate appearance</param>
    protected BattingStats(NaturalNumber plateAppearances, NaturalNumber atBats, NaturalNumber runs, NaturalNumber hits,
        NaturalNumber doubles, NaturalNumber triples, NaturalNumber homeRuns, NaturalNumber runsBattedIn,
        NaturalNumber baseOnBalls, NaturalNumber intentionalWalks, NaturalNumber strikeouts, NaturalNumber stolenBases,
        NaturalNumber caughtStealing, NaturalNumber hitByPitch, NaturalNumber sacrificeBunts,
        NaturalNumber sacrificeFlies, NaturalNumber numberOfPitchesSeen, NaturalNumber leftOnBase,
        NaturalNumber groundOuts, NaturalNumber groundIntoDoublePlays, NaturalNumber groundIntoTriplePlays,
        NaturalNumber airOuts, NaturalNumber catchersInterference)
    {
        PlateAppearances = plateAppearances;
        AtBats = atBats;
        Runs = runs;
        Hits = hits;
        Doubles = doubles;
        Triples = triples;
        HomeRuns = homeRuns;
        RunsBattedIn = runsBattedIn;
        BaseOnBalls = baseOnBalls;
        IntentionalWalks = intentionalWalks;
        Strikeouts = strikeouts;
        StolenBases = stolenBases;
        CaughtStealing = caughtStealing;
        HitByPitch = hitByPitch;
        SacrificeBunts = sacrificeBunts;
        SacrificeFlies = sacrificeFlies;
        NumberOfPitchesSeen = numberOfPitchesSeen;
        LeftOnBase = leftOnBase;
        GroundOuts = groundOuts;
        GroundIntoDoublePlays = groundIntoDoublePlays;
        GroundIntoTriplePlays = groundIntoTriplePlays;
        AirOuts = airOuts;
        CatchersInterference = catchersInterference;
    }

    /// <summary>
    /// Creates <see cref="BattingStats"/>
    /// </summary>
    /// <param name="plateAppearances">The number of plate appearances</param>
    /// <param name="atBats">The number of at bats</param>
    /// <param name="runs">The number of runs scored</param>
    /// <param name="hits">The number of hits</param>
    /// <param name="doubles">The number of doubles</param>
    /// <param name="triples">The number of triples</param>
    /// <param name="homeRuns">The number of home runs</param>
    /// <param name="runsBattedIn">The number of runs batted in</param>
    /// <param name="baseOnBalls">The number of walks</param>
    /// <param name="intentionalWalks">The number of intentional walks</param>
    /// <param name="strikeouts">The number of strikeouts</param>
    /// <param name="stolenBases">The number of stolen bases</param>
    /// <param name="caughtStealing">The number of times caught stealing</param>
    /// <param name="hitByPitch">The number of times the player was hit by a pitch</param>
    /// <param name="sacrificeBunts">The number of sacrifice bunts</param>
    /// <param name="sacrificeFlies">The number of sacrifice flies</param>
    /// <param name="numberOfPitchesSeen">The number of pitches the player saw as a batter</param>
    /// <param name="leftOnBase">The number of runners the player did not advance when batting and their out results in the end of the inning</param>
    /// <param name="groundOuts">The number of times the batter grounded out</param>
    /// <param name="groundIntoDoublePlays">The number of times the batter grounded into a double play</param>
    /// <param name="groundIntoTriplePlays">The number of times the batter grounded into a triple play</param>
    /// <param name="airOuts">The number of times the batter hit a fly ball that led to an out</param>
    /// <param name="catchersInterference">The number of times a catcher interfered with the batter's plate appearance</param>
    /// <returns><see cref="BattingStats"/></returns>
    public static BattingStats Create(int plateAppearances, int atBats, int runs, int hits, int doubles, int triples,
        int homeRuns, int runsBattedIn, int baseOnBalls, int intentionalWalks, int strikeouts, int stolenBases,
        int caughtStealing, int hitByPitch, int sacrificeBunts, int sacrificeFlies, int numberOfPitchesSeen,
        int leftOnBase, int groundOuts, int groundIntoDoublePlays, int groundIntoTriplePlays, int airOuts,
        int catchersInterference)
    {
        var pa = NaturalNumber.Create(plateAppearances);
        var ab = NaturalNumber.Create(atBats);
        var r = NaturalNumber.Create(runs);
        var h = NaturalNumber.Create(hits);
        var d = NaturalNumber.Create(doubles);
        var t = NaturalNumber.Create(triples);
        var hr = NaturalNumber.Create(homeRuns);
        var rbi = NaturalNumber.Create(runsBattedIn);
        var walks = NaturalNumber.Create(baseOnBalls);
        var iWalks = NaturalNumber.Create(intentionalWalks);
        var k = NaturalNumber.Create(strikeouts);
        var sb = NaturalNumber.Create(stolenBases);
        var cs = NaturalNumber.Create(caughtStealing);
        var hbp = NaturalNumber.Create(hitByPitch);
        var sacB = NaturalNumber.Create(sacrificeBunts);
        var sacF = NaturalNumber.Create(sacrificeFlies);
        var pitchCount = NaturalNumber.Create(numberOfPitchesSeen);
        var lob = NaturalNumber.Create(leftOnBase);
        var go = NaturalNumber.Create(groundOuts);
        var goDp = NaturalNumber.Create(groundIntoDoublePlays);
        var goTp = NaturalNumber.Create(groundIntoTriplePlays);
        var ao = NaturalNumber.Create(airOuts);
        var ci = NaturalNumber.Create(catchersInterference);
        return new BattingStats(plateAppearances: pa,
            atBats: ab,
            runs: r,
            hits: h,
            doubles: d,
            triples: t,
            homeRuns: hr,
            runsBattedIn: rbi,
            baseOnBalls: walks,
            intentionalWalks: iWalks,
            strikeouts: k,
            stolenBases: sb,
            caughtStealing: cs,
            hitByPitch: hbp,
            sacrificeBunts: sacB,
            sacrificeFlies: sacF,
            numberOfPitchesSeen: pitchCount,
            leftOnBase: lob,
            groundOuts: go,
            groundIntoDoublePlays: goDp,
            groundIntoTriplePlays: goTp,
            airOuts: ao,
            catchersInterference: ci
        );
    }

    /// <summary>
    /// Creates an aggregate <see cref="BattingStats"/>
    /// </summary>
    /// <param name="battingStatsCollection">A collection of batting stats</param>
    /// <returns>Aggregated <see cref="BattingStats"/></returns>
    public static BattingStats Create(IEnumerable<BattingStats> battingStatsCollection)
    {
        var battingStatsArray = battingStatsCollection as BattingStats[] ?? battingStatsCollection.ToArray();
        return Create(
            plateAppearances: battingStatsArray.Sum(x => x.PlateAppearances.Value),
            atBats: battingStatsArray.Sum(x => x.AtBats.Value),
            runs: battingStatsArray.Sum(x => x.Runs.Value),
            hits: battingStatsArray.Sum(x => x.Hits.Value),
            doubles: battingStatsArray.Sum(x => x.Doubles.Value),
            triples: battingStatsArray.Sum(x => x.Triples.Value),
            homeRuns: battingStatsArray.Sum(x => x.HomeRuns.Value),
            runsBattedIn: battingStatsArray.Sum(x => x.RunsBattedIn.Value),
            baseOnBalls: battingStatsArray.Sum(x => x.BaseOnBalls.Value),
            intentionalWalks: battingStatsArray.Sum(x => x.IntentionalWalks.Value),
            strikeouts: battingStatsArray.Sum(x => x.Strikeouts.Value),
            stolenBases: battingStatsArray.Sum(x => x.StolenBases.Value),
            caughtStealing: battingStatsArray.Sum(x => x.CaughtStealing.Value),
            hitByPitch: battingStatsArray.Sum(x => x.HitByPitch.Value),
            sacrificeBunts: battingStatsArray.Sum(x => x.SacrificeBunts.Value),
            sacrificeFlies: battingStatsArray.Sum(x => x.SacrificeFlies.Value),
            numberOfPitchesSeen: battingStatsArray.Sum(x => x.NumberOfPitchesSeen.Value),
            leftOnBase: battingStatsArray.Sum(x => x.LeftOnBase.Value),
            groundOuts: battingStatsArray.Sum(x => x.GroundOuts.Value),
            groundIntoDoublePlays: battingStatsArray.Sum(x => x.GroundIntoDoublePlays.Value),
            groundIntoTriplePlays: battingStatsArray.Sum(x => x.GroundIntoTriplePlays.Value),
            airOuts: battingStatsArray.Sum(x => x.AirOuts.Value),
            catchersInterference: battingStatsArray.Sum(x => x.CatchersInterference.Value)
        );
    }
}