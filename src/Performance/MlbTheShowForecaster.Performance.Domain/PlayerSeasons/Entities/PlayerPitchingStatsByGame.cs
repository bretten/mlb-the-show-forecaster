using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

public sealed class PlayerPitchingStatsByGame : Entity
{
    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId MlbId { get; private set; }

    /// <summary>
    /// The season
    /// </summary>
    public int SeasonYear { get; private set; }

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
    /// True if the pitcher got the win for this game
    /// </summary>
    public bool Win { get; private set; }

    /// <summary>
    /// True if the pitcher got the loss for this game
    /// </summary>
    public int Loss { get; private set; }

    /// <summary>
    /// The pitcher's earned run average
    /// </summary>
    public int EarnedRunAverage { get; private set; }

    /// <summary>
    /// True if the pitcher started this game
    /// </summary>
    public bool GameStarted { get; private set; }

    /// <summary>
    /// True if the pitcher was the last pitcher in the game as a relief pitcher
    /// </summary>
    public bool GameFinished { get; private set; }

    /// <summary>
    /// True if the pitcher pitched the whole game
    /// </summary>
    public bool CompleteGame { get; private set; }

    /// <summary>
    /// True if the pitcher pitched a shutout
    /// </summary>
    public bool Shutout { get; private set; }

    /// <summary>
    /// True if the pitcher earned a hold
    /// </summary>
    public bool Hold { get; private set; }

    /// <summary>
    /// True if the pitcher earned a save
    /// </summary>
    public bool Save { get; private set; }

    /// <summary>
    /// True if the pitcher failed to earn a save
    /// </summary>
    public bool BlownSave { get; private set; }

    /// <summary>
    /// True if this game was a save opportunity for the pitcher
    /// </summary>
    public bool SaveOpportunity { get; private set; }

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public int InningsPitched { get; private set; }

    /// <summary>
    /// The number of hits given up
    /// </summary>
    public int Hits { get; private set; }

    /// <summary>
    /// The number of runs given up
    /// </summary>
    public int Runs { get; private set; }

    /// <summary>
    /// The number of earned runs given up (runs that were a result of this pitcher giving up a hit)
    /// </summary>
    public int EarnedRuns { get; private set; }

    /// <summary>
    /// The number of home runs given up
    /// </summary>
    public int HomeRuns { get; private set; }

    /// <summary>
    /// The number of pitches thrown this game
    /// </summary>
    public int NumberOfPitches { get; private set; }

    /// <summary>
    /// Number of times the pitcher hit a batter with a pitch
    /// </summary>
    public int HitBatsmen { get; private set; }

    /// <summary>
    /// The number of times the pitcher walked the batter
    /// </summary>
    public int BaseOnBalls { get; private set; }

    /// <summary>
    /// The number of times the pitcher intentionally walked the batter
    /// </summary>
    public int IntentionalWalks { get; private set; }

    /// <summary>
    /// The number of strike outs
    /// </summary>
    public int StrikeOuts { get; private set; }

    /// <summary>
    /// The number of times a pitch resulted in a ground out
    /// </summary>
    public int GroundOuts { get; private set; }

    /// <summary>
    /// The number of times a pitch resulted in a air/fly out
    /// </summary>
    public int AirOuts { get; private set; }

    /// <summary>
    /// The number of doubles given up
    /// </summary>
    public int Doubles { get; private set; }

    /// <summary>
    /// The number of triples given up
    /// </summary>
    public int Triples { get; private set; }

    /// <summary>
    /// The number of double play ground outs induced
    /// </summary>
    public int GroundIntoDoublePlays { get; private set; }

    /// <summary>
    /// The number of wild pitches thrown
    /// </summary>
    public int WildPitches { get; private set; }

    /// <summary>
    /// The number of balks
    /// </summary>
    public int Balks { get; private set; }

    /// <summary>
    /// The number of bases stolen against this pitcher
    /// </summary>
    public int StolenBases { get; private set; }

    /// <summary>
    /// The number of times a runner was caught stealing against this pitcher
    /// </summary>
    public int CaughtStealing { get; private set; }

    /// <summary>
    /// The number of pick offs made by this pitcher
    /// </summary>
    public int PickOffs { get; private set; }

    /// <summary>
    /// The number of strikes thrown by the pitcher
    /// </summary>
    public int Strikes { get; private set; }

    /// <summary>
    /// The number of batters faced, pitcher version of plate appearance
    /// </summary>
    public int BattersFaced { get; private set; }

    /// <summary>
    /// The number of at-bats
    /// </summary>
    public float AtBats { get; private set; }

    /// <summary>
    /// The number of runners on base when the pitcher enters the game
    /// </summary>
    public int InheritedRunners { get; private set; }

    /// <summary>
    /// The number of inherited runners allowed to score
    /// </summary>
    public int InheritedRunnersScored { get; private set; }

    /// <summary>
    /// The number of outs made by the team while this pitcher was active
    /// </summary>
    public int Outs { get; private set; }

    /// <summary>
    /// The number of times a catcher interfered with the batter's plate appearance
    /// </summary>
    public int CatchersInterference { get; private set; }

    /// <summary>
    /// The number of sacrifice bunts made against the pitcher
    /// </summary>
    public int SacrificeBunts { get; private set; }

    /// <summary>
    /// The number of sacrifice flies made against the pitcher
    /// </summary>
    public int SacrificeFlies { get; private set; }

    /// <summary>
    /// Quality start
    /// </summary>
    public bool QualityStart => InningsPitched >= 6 && EarnedRuns <= 3;

    /// <summary>
    /// Batting average against = H / (BF - BB - HBP - SH - SF - CINT)
    /// </summary>
    public float OpponentsBattingAverage => ((float)(Hits)) /
                                            (BattersFaced - BaseOnBalls - HitBatsmen - SacrificeBunts - SacrificeFlies -
                                             CatchersInterference);

    /// <summary>
    /// Walks plus hits per inning pitched = (BB + H) / IP
    /// </summary>
    public float WalksPlusHitsPerInningPitched => (float)(BaseOnBalls + Hits) / InningsPitched;

    /// <summary>
    /// Stolen base percentage = SB / (SB + CS)
    /// </summary>
    public float StolenBasePercentage => (float)(StolenBases) / (StolenBases + CaughtStealing);

    /// <summary>
    /// Strike percentage = K / Pitches
    /// </summary>
    public float StrikePercentage => (float)Strikes / NumberOfPitches;

    /// <summary>
    /// Pitches per inning = Pitches/Inning
    /// </summary>
    public float PitchesPerInning => (float)NumberOfPitches / InningsPitched;

    /// <summary>
    /// Runs scored per 9 = 9 * (R / IP)
    /// </summary>
    public float RunsScoredPer9 => 9 * ((float)Runs / InningsPitched);

    /// <summary>
    /// Batters faced
    /// </summary>
    public float OpponentsOnBasePercentage => (float)(Hits + BaseOnBalls + HitBatsmen) /
                                              (AtBats + BaseOnBalls + HitBatsmen + SacrificeFlies);

    /// <summary>
    /// Total bases = (1 * 1B) + (2 * 2B) + (3 * 3B) + (4 * HR)
    /// = (1 * [hits - 2B - 3B - HR]) + (2 * 2B) + (3 * 3B) + (4 * HR)
    /// = hits + 2B + (2 * 3B) + (3 * HR)
    /// </summary>
    public float TotalBases => (float)Hits + Doubles + (2 * Triples) + (3 * HomeRuns);

    /// <summary>
    /// Slugging
    /// </summary>
    public float Slugging => (float)(TotalBases / AtBats);

    /// <summary>
    /// On-base plus slugging
    /// </summary>
    public float OpponentsOnBasePlusSlugging => (float)(OpponentsOnBasePercentage + Slugging);

    /// <summary>
    /// Strikeouts per 9 innings
    /// </summary>
    public float StrikeoutsPer9Inn => 9 * ((float)StrikeOuts / InningsPitched);

    /// <summary>
    /// Walks per 9 innings
    /// </summary>
    public float WalksPer9Inn => 9 * ((float)BaseOnBalls / InningsPitched);

    /// <summary>
    /// HR per 9 innings
    /// </summary>
    public float HomeRunsPer9 => 9 * ((float)HomeRuns / InningsPitched);

    /// <summary>
    /// Hits per 9 innings
    /// </summary>
    public float HitsPer9Inn => 9 * ((float)Hits / InningsPitched);

    /// <summary>
    /// Strikeout to walk ratio
    /// </summary>
    public float StrikeoutWalkRatio => (float)StrikeOuts / BaseOnBalls;

    public PlayerPitchingStatsByGame(Guid id) : base(id)
    {
    }
}