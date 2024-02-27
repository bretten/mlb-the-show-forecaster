using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// A player's batting statistics for a single game
/// </summary>
public sealed class PlayerBattingStatsByGame : BattingStats
{
    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId PlayerMlbId { get; }

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
    /// Determines the properties that are used in equality
    /// </summary>
    /// <returns>The values of the properties that are used in equality</returns>
    protected override IEnumerable<object?> GetNestedValues()
    {
        yield return PlayerMlbId.Value;
        yield return SeasonYear.Value;
        yield return GameDate;
        yield return GameId.Value;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="gameDate">The date of the game</param>
    /// <param name="gameId">The MLB ID of the game</param>
    /// <param name="teamId">The MLB ID of the team</param>
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
    private PlayerBattingStatsByGame(MlbId playerMlbId, SeasonYear seasonYear, DateTime gameDate, MlbId gameId,
        MlbId teamId, NaturalNumber plateAppearances, NaturalNumber atBats, NaturalNumber runs, NaturalNumber hits,
        NaturalNumber doubles, NaturalNumber triples, NaturalNumber homeRuns, NaturalNumber runsBattedIn,
        NaturalNumber baseOnBalls, NaturalNumber intentionalWalks, NaturalNumber strikeouts, NaturalNumber stolenBases,
        NaturalNumber caughtStealing, NaturalNumber hitByPitch, NaturalNumber sacrificeBunts,
        NaturalNumber sacrificeFlies, NaturalNumber numberOfPitchesSeen, NaturalNumber leftOnBase,
        NaturalNumber groundOuts, NaturalNumber groundIntoDoublePlays, NaturalNumber groundIntoTriplePlays,
        NaturalNumber airOuts, NaturalNumber catchersInterference) : base(plateAppearances, atBats, runs, hits, doubles,
        triples, homeRuns, runsBattedIn, baseOnBalls, intentionalWalks, strikeouts, stolenBases, caughtStealing,
        hitByPitch, sacrificeBunts, sacrificeFlies, numberOfPitchesSeen, leftOnBase, groundOuts, groundIntoDoublePlays,
        groundIntoTriplePlays, airOuts, catchersInterference)
    {
        PlayerMlbId = playerMlbId;
        SeasonYear = seasonYear;
        GameDate = gameDate;
        GameId = gameId;
        TeamId = teamId;
    }

    /// <summary>
    /// Creates <see cref="PlayerBattingStatsByGame"/>
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="gameDate">The date of the game</param>
    /// <param name="gameId">The MLB ID of the game</param>
    /// <param name="teamId">The MLB ID of the team</param>
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
    /// <returns><see cref="PlayerBattingStatsByGame"/></returns>
    public static PlayerBattingStatsByGame Create(MlbId playerMlbId, SeasonYear seasonYear, DateTime gameDate,
        MlbId gameId, MlbId teamId, int plateAppearances, int atBats, int runs, int hits, int doubles, int triples,
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
        return new PlayerBattingStatsByGame(playerMlbId, seasonYear, gameDate, gameId, teamId,
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