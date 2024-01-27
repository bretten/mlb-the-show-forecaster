using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

public sealed class PlayerBattingStatsByGame : Entity
{
    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId MlbId { get; private set; }

    /// <summary>
    /// The season
    /// </summary>
    public SeasonYear SeasonYear { get; private set; }

    /// <summary>
    /// The date of the game
    /// </summary>
    public DateTime GameDate { get; private set; }

    /// <summary>
    /// The MLB ID of the game
    /// </summary>
    public MlbId GameId { get; private set; }

    /// <summary>
    /// The MLB ID of the team
    /// </summary>
    public MlbId TeamId { get; private set; }

    /// <summary>
    /// The number of plate appearances
    /// </summary>
    public NaturalNumber PlateAppearances { get; private set; }

    /// <summary>
    /// The number of at bats
    /// </summary>
    public NaturalNumber AtBats { get; private set; }

    /// <summary>
    /// The number of runs scored
    /// </summary>
    public NaturalNumber Runs { get; private set; }

    /// <summary>
    /// The number of hits
    /// </summary>
    public NaturalNumber Hits { get; private set; }

    /// <summary>
    /// The number of doubles
    /// </summary>
    public NaturalNumber Doubles { get; private set; }

    /// <summary>
    /// The number of triples
    /// </summary>
    public NaturalNumber Triples { get; private set; }

    /// <summary>
    /// The number of home runs
    /// </summary>
    public NaturalNumber HomeRuns { get; private set; }

    /// <summary>
    /// The number of runs batted in
    /// </summary>
    public NaturalNumber RunsBattedIn { get; private set; }

    /// <summary>
    /// The number of walks
    /// </summary>
    public NaturalNumber BaseOnBalls { get; private set; }

    /// <summary>
    /// The number of intentional walks
    /// </summary>
    public NaturalNumber IntentionalWalks { get; private set; }

    /// <summary>
    /// The number of strike outs
    /// </summary>
    public NaturalNumber StrikeOuts { get; private set; }

    /// <summary>
    /// The number of stolen bases
    /// </summary>
    public NaturalNumber StolenBases { get; private set; }

    /// <summary>
    /// The number of times caught stealing
    /// </summary>
    public NaturalNumber CaughtStealing { get; private set; }

    /// <summary>
    /// The number of times the player was hit by a pitch
    /// </summary>
    public NaturalNumber HitByPitch { get; private set; }

    /// <summary>
    /// The number of sacrifice bunts
    /// </summary>
    public NaturalNumber SacrificeBunts { get; private set; }

    /// <summary>
    /// The number of sacrifice flies
    /// </summary>
    public NaturalNumber SacrificeFlies { get; private set; }

    /// <summary>
    /// The number of pitches the player saw as a batter
    /// </summary>
    public NaturalNumber NumberOfPitchesSeen { get; private set; }

    /// <summary>
    /// The number of runners the player did not advance when batting and
    /// their out results in the end of the inning
    /// </summary>
    public NaturalNumber LeftOnBase { get; private set; }

    /// <summary>
    /// The number of times the batter grounded out
    /// </summary>
    public NaturalNumber GroundOuts { get; private set; }

    /// <summary>
    /// The number of times the batter grounded into a double play
    /// </summary>
    public NaturalNumber GroundIntoDoublePlays { get; private set; }

    /// <summary>
    /// The number of times the batter grounded into a triple play
    /// </summary>
    public NaturalNumber GroundIntoTriplePlays { get; private set; }

    /// <summary>
    /// The number of times the batter hit a fly ball that led to an out
    /// </summary>
    public NaturalNumber AirOuts { get; private set; }

    /// <summary>
    /// The number of times a catcher interfered with the batter's plate appearance
    /// </summary>
    public NaturalNumber CatchersInterference { get; private set; }

    /// <summary>
    /// Batting average
    /// </summary>
    public BattingAverage BattingAverage => BattingAverage.Create(Hits, AtBats);

    /// <summary>
    /// On-base percentage
    /// </summary>
    public OnBasePercentage OnBasePercentage =>
        OnBasePercentage.Create(Hits, BaseOnBalls, HitByPitch, AtBats, SacrificeFlies);

    /// <summary>
    /// Total bases
    /// </summary>
    public TotalBases TotalBases => TotalBases.CreateWithHits(Hits, Doubles, Triples, HomeRuns);

    /// <summary>
    /// Slugging
    /// </summary>
    public Slugging Slugging => Slugging.Create(TotalBases, AtBats);

    /// <summary>
    /// On-base plus slugging
    /// </summary>
    public OnBasePlusSlugging OnBasePlusSlugging => OnBasePlusSlugging.Create(OnBasePercentage, Slugging);

    /// <summary>
    /// Stolen base percentage 
    /// </summary>
    public StolenBasePercentage StolenBasePercentage => StolenBasePercentage.Create(StolenBases, CaughtStealing);

    /// <summary>
    /// Batting average on balls in play
    /// </summary>
    public BattingAverageOnBallsInPlay BattingAverageOnBallsInPlay =>
        BattingAverageOnBallsInPlay.Create(Hits, HomeRuns, AtBats, StrikeOuts, SacrificeFlies);

    private PlayerBattingStatsByGame(MlbId mlbId, SeasonYear seasonYear, DateTime gameDate, MlbId gameId,
        MlbId teamId, NaturalNumber plateAppearances, NaturalNumber atBats, NaturalNumber runs, NaturalNumber hits,
        NaturalNumber doubles, NaturalNumber triples, NaturalNumber homeRuns, NaturalNumber runsBattedIn,
        NaturalNumber baseOnBalls, NaturalNumber intentionalWalks, NaturalNumber strikeOuts, NaturalNumber stolenBases,
        NaturalNumber caughtStealing, NaturalNumber hitByPitch, NaturalNumber sacrificeBunts,
        NaturalNumber sacrificeFlies, NaturalNumber numberOfPitchesSeen, NaturalNumber leftOnBase,
        NaturalNumber groundOuts, NaturalNumber groundIntoDoublePlays, NaturalNumber groundIntoTriplePlays,
        NaturalNumber airOuts, NaturalNumber catchersInterference) : base(Guid.NewGuid())
    {
        MlbId = mlbId;
        SeasonYear = seasonYear;
        GameDate = gameDate;
        GameId = gameId;
        TeamId = teamId;
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
        StrikeOuts = strikeOuts;
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

    public static PlayerBattingStatsByGame Create(MlbId mlbId, SeasonYear seasonYear, DateTime gameDate, MlbId gameId,
        MlbId teamId, uint plateAppearances, uint atBats, uint runs, uint hits, uint doubles, uint triples,
        uint homeRuns, uint runsBattedIn, uint baseOnBalls, uint intentionalWalks, uint strikeOuts, uint stolenBases,
        uint caughtStealing, uint hitByPitch, uint sacrificeBunts, uint sacrificeFlies, uint numberOfPitchesSeen,
        uint leftOnBase, uint groundOuts, uint groundIntoDoublePlays, uint groundIntoTriplePlays, uint airOuts,
        uint catchersInterference)
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
        var k = NaturalNumber.Create(strikeOuts);
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
        return new PlayerBattingStatsByGame(mlbId: mlbId, seasonYear: seasonYear, gameId: gameId, gameDate: gameDate,
            teamId: teamId,
            plateAppearances: pa,
            atBats: ab,
            runs: r,
            hits: h,
            doubles: d,
            triples: t,
            homeRuns: hr,
            runsBattedIn: rbi,
            baseOnBalls: walks,
            intentionalWalks: iWalks,
            strikeOuts: k,
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
}