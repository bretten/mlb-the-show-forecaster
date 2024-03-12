using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// A player's pitching statistics for a single game
/// </summary>
public sealed class PlayerPitchingStatsByGame : PitchingStats
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
    public MlbId GameMlbId { get; }

    /// <summary>
    /// The MLB ID of the team
    /// </summary>
    public MlbId TeamMlbId { get; }

    /// <summary>
    /// The pitching result for this game
    /// </summary>
    public PitchingResult PitchingResult
    {
        get
        {
            if (_pitchingResult == null)
            {
                _pitchingResult = PitchingResult.Create(win: Wins.Value > 0,
                    loss: Losses.Value > 0,
                    gameStarted: GamesStarted.Value > 0,
                    gameFinished: GamesFinished.Value > 0,
                    completeGame: CompleteGames.Value > 0,
                    shutout: Shutouts.Value > 0,
                    hold: Holds.Value > 0,
                    save: Saves.Value > 0,
                    blownSave: BlownSaves.Value > 0,
                    saveOpportunity: SaveOpportunities.Value > 0
                );
            }

            return _pitchingResult;
        }
    }

    /// <summary>
    /// The pitching result for this game for lazy loading <see cref="PitchingResult"/>
    /// </summary>
    private PitchingResult? _pitchingResult;

    /// <summary>
    /// Determines the properties that are used in equality
    /// </summary>
    /// <returns>The values of the properties that are used in equality</returns>
    protected override IEnumerable<object?> GetNestedValues()
    {
        yield return PlayerMlbId.Value;
        yield return SeasonYear.Value;
        yield return GameDate;
        yield return GameMlbId.Value;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="gameDate">The date of the game</param>
    /// <param name="gameMlbId">The MLB ID of the game</param>
    /// <param name="teamMlbId">The MLB ID of the team</param>
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
    private PlayerPitchingStatsByGame(MlbId playerMlbId, SeasonYear seasonYear, DateTime gameDate, MlbId gameMlbId,
        MlbId teamMlbId, NaturalNumber wins, NaturalNumber losses, NaturalNumber gamesStarted, NaturalNumber gamesFinished,
        NaturalNumber completeGames, NaturalNumber shutouts, NaturalNumber holds, NaturalNumber saves,
        NaturalNumber blownSaves, NaturalNumber saveOpportunities, InningsCount inningsPitched, NaturalNumber hits,
        NaturalNumber doubles, NaturalNumber triples, NaturalNumber homeRuns, NaturalNumber runs,
        NaturalNumber earnedRuns, NaturalNumber strikeouts, NaturalNumber baseOnBalls, NaturalNumber intentionalWalks,
        NaturalNumber hitBatsmen, NaturalNumber outs, NaturalNumber groundOuts, NaturalNumber airOuts,
        NaturalNumber groundIntoDoublePlays, NaturalNumber numberOfPitches, NaturalNumber strikes,
        NaturalNumber wildPitches, NaturalNumber balks, NaturalNumber battersFaced, NaturalNumber atBats,
        NaturalNumber stolenBases, NaturalNumber caughtStealing, NaturalNumber pickoffs, NaturalNumber inheritedRunners,
        NaturalNumber inheritedRunnersScored, NaturalNumber catcherInterferences, NaturalNumber sacrificeBunts,
        NaturalNumber sacrificeFlies) : base(wins, losses, gamesStarted, gamesFinished, completeGames, shutouts, holds,
        saves, blownSaves, saveOpportunities, inningsPitched, hits, doubles, triples, homeRuns, runs, earnedRuns,
        strikeouts, baseOnBalls, intentionalWalks, hitBatsmen, outs, groundOuts, airOuts, groundIntoDoublePlays,
        numberOfPitches, strikes, wildPitches, balks, battersFaced, atBats, stolenBases, caughtStealing, pickoffs,
        inheritedRunners, inheritedRunnersScored, catcherInterferences, sacrificeBunts, sacrificeFlies)
    {
        PlayerMlbId = playerMlbId;
        SeasonYear = seasonYear;
        GameDate = gameDate;
        GameMlbId = gameMlbId;
        TeamMlbId = teamMlbId;
    }

    /// <summary>
    /// Creates <see cref="PlayerPitchingStatsByGame"/>
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="gameDate">The date of the game</param>
    /// <param name="gameMlbId">The MLB ID of the game</param>
    /// <param name="teamMlbId">The MLB ID of the team</param>
    /// <param name="win">True if the pitcher got the win for this game</param>
    /// <param name="loss">True if the pitcher got the loss for this game</param>
    /// <param name="gameStarted">True if the pitcher started this game</param>
    /// <param name="gameFinished">True if the pitcher was the last pitcher in the game as a relief pitcher</param>
    /// <param name="completeGame">True if the pitcher pitched the whole game</param>
    /// <param name="shutout">True if the pitcher pitched a shutout</param>
    /// <param name="hold">True if the pitcher earned a hold</param>
    /// <param name="save">True if the pitcher earned a save</param>
    /// <param name="blownSave">True if the pitcher failed to earn a save</param>
    /// <param name="saveOpportunity">True if this game was a save opportunity for the pitcher</param>
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
    /// <returns><see cref="PlayerPitchingStatsByGame"/></returns>
    public static PlayerPitchingStatsByGame Create(MlbId playerMlbId, SeasonYear seasonYear, DateTime gameDate,
        MlbId gameMlbId, MlbId teamMlbId, bool win, bool loss, bool gameStarted, bool gameFinished, bool completeGame,
        bool shutout, bool hold, bool save, bool blownSave, bool saveOpportunity, decimal inningsPitched, int hits,
        int doubles, int triples, int homeRuns, int runs, int earnedRuns, int strikeouts, int baseOnBalls,
        int intentionalWalks, int hitBatsmen, int outs, int groundOuts, int airOuts, int groundIntoDoublePlays,
        int numberOfPitches, int strikes, int wildPitches, int balks, int battersFaced, int atBats, int stolenBases,
        int caughtStealing, int pickoffs, int inheritedRunners, int inheritedRunnersScored, int catcherInterferences,
        int sacrificeBunts, int sacrificeFlies)
    {
        var w = win ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
        var l = loss ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
        var gs = gameStarted ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
        var gf = gameFinished ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
        var cg = completeGame ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
        var sho = shutout ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
        var hld = hold ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
        var sv = save ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
        var bs = blownSave ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
        var svo = saveOpportunity ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
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
        return new PlayerPitchingStatsByGame(playerMlbId, seasonYear, gameDate, gameMlbId, teamMlbId,
            wins: w,
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
}