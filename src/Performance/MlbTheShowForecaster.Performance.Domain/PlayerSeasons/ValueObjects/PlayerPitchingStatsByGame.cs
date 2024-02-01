using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.BaseRunning;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

public sealed class PlayerPitchingStatsByGame : ValueObject
{
    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId PlayerId { get; }

    /// <summary>
    /// The season
    /// </summary>
    public SeasonYear SeasonYear { get; }

    /// <summary>
    /// The date of the game
    /// </summary>
    public DateTime GameDate { get; }

    /// <summary>
    /// The MLB ID of the game
    /// </summary>
    public MlbId GameId { get; }

    /// <summary>
    /// The MLB ID of the team
    /// </summary>
    public MlbId TeamId { get; }

    /// <summary>
    /// True if the pitcher got the win for this game
    /// </summary>
    public bool Win { get; }

    /// <summary>
    /// True if the pitcher got the loss for this game
    /// </summary>
    public bool Loss { get; }

    /// <summary>
    /// True if the pitcher started this game
    /// </summary>
    public bool GameStarted { get; }

    /// <summary>
    /// True if the pitcher was the last pitcher in the game as a relief pitcher
    /// </summary>
    public bool GameFinished { get; }

    /// <summary>
    /// True if the pitcher pitched the whole game
    /// </summary>
    public bool CompleteGame { get; }

    /// <summary>
    /// True if the pitcher pitched a shutout
    /// </summary>
    public bool Shutout { get; }

    /// <summary>
    /// True if the pitcher earned a hold
    /// </summary>
    public bool Hold { get; }

    /// <summary>
    /// True if the pitcher earned a save
    /// </summary>
    public bool Save { get; }

    /// <summary>
    /// True if the pitcher failed to earn a save
    /// </summary>
    public bool BlownSave { get; }

    /// <summary>
    /// True if this game was a save opportunity for the pitcher
    /// </summary>
    public bool SaveOpportunity { get; }

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
    /// Number of times the pitcher hit a batter with a pitch
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
    public NaturalNumber PickOffs { get; }

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
    public NaturalNumber CatchersInterferences { get; }

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
        SacrificeFlies.Value, CatchersInterferences.Value);

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
    /// Determines the properties that are used in equality
    /// </summary>
    /// <returns>The values of the properties that are used in equality</returns>
    protected override IEnumerable<object?> GetNestedValues()
    {
        yield return PlayerId.Value;
        yield return SeasonYear.Value;
        yield return GameDate;
        yield return GameId.Value;
    }

    private PlayerPitchingStatsByGame(MlbId playerId, SeasonYear seasonYear, DateTime gameDate, MlbId gameId,
        MlbId teamId,
        bool win, bool loss, bool gameStarted, bool gameFinished, bool completeGame, bool shutout, bool hold, bool save,
        bool blownSave, bool saveOpportunity, InningsCount inningsPitched, NaturalNumber hits, NaturalNumber doubles,
        NaturalNumber triples, NaturalNumber homeRuns, NaturalNumber runs, NaturalNumber earnedRuns,
        NaturalNumber strikeouts, NaturalNumber baseOnBalls, NaturalNumber intentionalWalks, NaturalNumber hitBatsmen,
        NaturalNumber outs, NaturalNumber groundOuts, NaturalNumber airOuts, NaturalNumber groundIntoDoublePlays,
        NaturalNumber numberOfPitches, NaturalNumber strikes, NaturalNumber wildPitches, NaturalNumber balks,
        NaturalNumber battersFaced, NaturalNumber atBats, NaturalNumber stolenBases, NaturalNumber caughtStealing,
        NaturalNumber pickOffs, NaturalNumber inheritedRunners, NaturalNumber inheritedRunnersScored,
        NaturalNumber catchersInterferences, NaturalNumber sacrificeBunts, NaturalNumber sacrificeFlies)
    {
        PlayerId = playerId;
        SeasonYear = seasonYear;
        GameDate = gameDate;
        GameId = gameId;
        TeamId = teamId;
        Win = win;
        Loss = loss;
        GameStarted = gameStarted;
        GameFinished = gameFinished;
        CompleteGame = completeGame;
        Shutout = shutout;
        Hold = hold;
        Save = save;
        BlownSave = blownSave;
        SaveOpportunity = saveOpportunity;
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
        PickOffs = pickOffs;
        InheritedRunners = inheritedRunners;
        InheritedRunnersScored = inheritedRunnersScored;
        CatchersInterferences = catchersInterferences;
        SacrificeBunts = sacrificeBunts;
        SacrificeFlies = sacrificeFlies;
    }

    public static PlayerPitchingStatsByGame Create(MlbId playerId, SeasonYear seasonYear, DateTime gameDate,
        MlbId gameId,
        MlbId teamId, bool win, bool loss, bool gameStarted, bool gameFinished, bool completeGame, bool shutout,
        bool hold, bool save, bool blownSave, bool saveOpportunity, decimal inningsPitched, uint hits,
        uint doubles, uint triples, uint homeRuns, uint runs, uint earnedRuns, uint strikeouts, uint baseOnBalls,
        uint intentionalWalks, uint hitBatsmen, uint outs, uint groundOuts, uint airOuts, uint groundIntoDoublePlays,
        uint numberOfPitches, uint strikes, uint wildPitches, uint balks, uint battersFaced, uint atBats,
        uint stolenBases, uint caughtStealing, uint pickOffs, uint inheritedRunners, uint inheritedRunnersScored,
        uint catchersInterferences, uint sacrificeBunts, uint sacrificeFlies)
    {
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
        var pk = NaturalNumber.Create(pickOffs);
        var ir = NaturalNumber.Create(inheritedRunners);
        var irs = NaturalNumber.Create(inheritedRunnersScored);
        var ci = NaturalNumber.Create(catchersInterferences);
        var sh = NaturalNumber.Create(sacrificeBunts);
        var sf = NaturalNumber.Create(sacrificeFlies);
        return new PlayerPitchingStatsByGame(playerId, seasonYear, gameDate, gameId, teamId, win, loss, gameStarted,
            gameFinished, completeGame, shutout, hold, save, blownSave, saveOpportunity,
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
            pickOffs: pk,
            inheritedRunners: ir,
            inheritedRunnersScored: irs,
            catchersInterferences: ci,
            sacrificeBunts: sh,
            sacrificeFlies: sf);
    }
}