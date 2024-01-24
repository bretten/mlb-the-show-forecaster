using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Entities.PlayerSeasons;

public sealed class PlayerBattingStatsByGame : Entity
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
    /// The number of plate appearances
    /// </summary>
    public int PlateAppearances { get; private set; }

    /// <summary>
    /// The number of at bats
    /// </summary>
    public int AtBats { get; private set; }

    /// <summary>
    /// The number of runs scored
    /// </summary>
    public int Runs { get; private set; }

    /// <summary>
    /// The number of hits
    /// </summary>
    public int Hits { get; private set; }

    /// <summary>
    /// The number of doubles
    /// </summary>
    public int Doubles { get; private set; }

    /// <summary>
    /// The number of triples
    /// </summary>
    public int Triples { get; private set; }

    /// <summary>
    /// The number of home runs
    /// </summary>
    public int HomeRuns { get; private set; }

    /// <summary>
    /// The number of runs batted in
    /// </summary>
    public int RunsBattedIn { get; private set; }

    /// <summary>
    /// The number of walks
    /// </summary>
    public int BaseOnBalls { get; private set; }

    /// <summary>
    /// The number of intentional walks
    /// </summary>
    public int IntentionalWalks { get; private set; }

    /// <summary>
    /// The number of strike outs
    /// </summary>
    public int StrikeOuts { get; private set; }

    /// <summary>
    /// The number of stolen bases
    /// </summary>
    public int StolenBases { get; private set; }

    /// <summary>
    /// The number of times caught stealing
    /// </summary>
    public int CaughtStealing { get; private set; }

    /// <summary>
    /// The number of times the player was hit by a pitch
    /// </summary>
    public int HitByPitch { get; private set; }

    /// <summary>
    /// The number of sacrifice bunts
    /// </summary>
    public int SacrificeBunts { get; private set; }

    /// <summary>
    /// The number of sacrifice flies
    /// </summary>
    public int SacrificeFlies { get; private set; }

    /// <summary>
    /// The number of pitches the player saw as a batter
    /// </summary>
    public int NumberOfPitchesSeen { get; private set; }

    /// <summary>
    /// The number of runners the player did not advance when batting and
    /// their out results in the end of the inning
    /// </summary>
    public int LeftOnBase { get; private set; }

    /// <summary>
    /// The number of times the batter grounded out
    /// </summary>
    public int GroundOuts { get; private set; }

    /// <summary>
    /// The number of times the batter grounded into a double play
    /// </summary>
    public int GroundIntoDoublePlays { get; private set; }

    /// <summary>
    /// The number of times the batter grounded into a triple play
    /// </summary>
    public int GroundIntoTriplePlays { get; private set; }

    /// <summary>
    /// The number of times the batter hit a fly ball that led to an out
    /// </summary>
    public int AirOuts { get; private set; }

    /// <summary>
    /// The number of times a catcher interfered with the batter's plate appearance
    /// </summary>
    public int CatchersInterference { get; private set; }

    /// <summary>
    /// Batting average = hits / at-bats
    /// </summary>
    public float BattingAverage => (float)Hits / AtBats;

    /// <summary>
    /// On-base percentage = (hits + walks + hit by pitch) / (at-bats + walks + hit by pitch + sac flies)
    /// </summary>
    public float OnBasePercentage =>
        (float)(Hits + BaseOnBalls + HitByPitch) / (AtBats + BaseOnBalls + HitByPitch + SacrificeFlies);

    /// <summary>
    /// Total bases = (1 * 1B) + (2 * 2B) + (3 * 3B) + (4 * HR)
    /// = (1 * [hits - 2B - 3B - HR]) + (2 * 2B) + (3 * 3B) + (4 * HR)
    /// = hits + 2B + (2 * 3B) + (3 * HR)
    /// </summary>
    public float TotalBases => (float)Hits + Doubles + (2 * Triples) + (3 * HomeRuns);

    /// <summary>
    /// Slugging = TB / AB
    /// </summary>
    public float Slugging => (float)(TotalBases / AtBats);

    /// <summary>
    /// On-base plus slugging = [AB(H + BB + HBP) + TB(AB + BB + SF + HBP)] / [ AB(AB + BB + SF + HBP) ]
    /// </summary>
    public float OnBasePlusSlugging => (float)(OnBasePercentage + Slugging);

    /// <summary>
    /// Stolen base percentage = stolen bases / stole base attempts
    /// = stolen bases / (caught stealing + stolen bases)
    /// </summary>
    public float StolenBasePercentage => ((float)StolenBases / (StolenBases + CaughtStealing));

    /// <summary>
    /// Batting average on balls in play = (H - HR) / (AB - K - HR + SF)
    /// </summary>
    public float BattingAverageOnBallsInPlay =>
        ((float)Hits - HomeRuns) / (AtBats - StrikeOuts - HomeRuns + SacrificeFlies);

    public PlayerBattingStatsByGame(Guid id) : base(id)
    {
    }
}