﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.BaseRunning;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// A player's batting statistics for a single game
/// </summary>
public sealed class PlayerBattingStatsByGame : ValueObject
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
    /// The number of runners the player did not advance when batting and
    /// their out results in the end of the inning
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

    private PlayerBattingStatsByGame(MlbId playerId, SeasonYear seasonYear, DateTime gameDate, MlbId gameId,
        MlbId teamId, NaturalNumber plateAppearances, NaturalNumber atBats, NaturalNumber runs, NaturalNumber hits,
        NaturalNumber doubles, NaturalNumber triples, NaturalNumber homeRuns, NaturalNumber runsBattedIn,
        NaturalNumber baseOnBalls, NaturalNumber intentionalWalks, NaturalNumber strikeouts, NaturalNumber stolenBases,
        NaturalNumber caughtStealing, NaturalNumber hitByPitch, NaturalNumber sacrificeBunts,
        NaturalNumber sacrificeFlies, NaturalNumber numberOfPitchesSeen, NaturalNumber leftOnBase,
        NaturalNumber groundOuts, NaturalNumber groundIntoDoublePlays, NaturalNumber groundIntoTriplePlays,
        NaturalNumber airOuts, NaturalNumber catchersInterference)
    {
        PlayerId = playerId;
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

    public static PlayerBattingStatsByGame Create(MlbId playerId, SeasonYear seasonYear, DateTime gameDate,
        MlbId gameId,
        MlbId teamId, uint plateAppearances, uint atBats, uint runs, uint hits, uint doubles, uint triples,
        uint homeRuns, uint runsBattedIn, uint baseOnBalls, uint intentionalWalks, uint strikeouts, uint stolenBases,
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
        return new PlayerBattingStatsByGame(playerId: playerId, seasonYear: seasonYear, gameId: gameId,
            gameDate: gameDate,
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
}