using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

/// <summary>
/// Represents a Player's batting, fielding and pitching statistics by individual game for a whole season
/// </summary>
public sealed class PlayerStatsBySeason : AggregateRoot
{
    /// <summary>
    /// The player's batting stats by game
    /// </summary>
    private readonly List<PlayerBattingStatsByGame> _battingStatsByGames;

    /// <summary>
    /// The player's fielding stats by game
    /// </summary>
    private readonly List<PlayerFieldingStatsByGame> _fieldingStatsByGames;

    /// <summary>
    /// The player's pitching stats by game
    /// </summary>
    private readonly List<PlayerPitchingStatsByGame> _pitchingStatsByGames;

    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId PlayerId { get; }

    /// <summary>
    /// The season
    /// </summary>
    public SeasonYear SeasonYear { get; }

    /// <summary>
    /// The player's batting stats by game in chronological order
    /// </summary>
    public List<PlayerBattingStatsByGame> BattingStatsByGamesChronologically =>
        _battingStatsByGames.OrderBy(x => x.GameDate).ToList();

    /// <summary>
    /// The player's fielding stats by game in chronological order
    /// </summary>
    public List<PlayerFieldingStatsByGame> FieldingStatsByGamesChronologically =>
        _fieldingStatsByGames.OrderBy(x => x.GameDate).ToList();

    /// <summary>
    /// The player's pitching stats by game in chronological order
    /// </summary>
    public List<PlayerPitchingStatsByGame> PitchingStatsByGamesChronologically =>
        _pitchingStatsByGames.OrderBy(x => x.GameDate).ToList();

    private PlayerStatsBySeason(MlbId playerId, SeasonYear seasonYear,
        List<PlayerBattingStatsByGame> battingStatsByGames, List<PlayerFieldingStatsByGame> fieldingStatsByGames,
        List<PlayerPitchingStatsByGame> pitchingStatsByGames) : base(Guid.NewGuid())
    {
        PlayerId = playerId;
        SeasonYear = seasonYear;
        _battingStatsByGames = battingStatsByGames;
        _fieldingStatsByGames = fieldingStatsByGames;
        _pitchingStatsByGames = pitchingStatsByGames;
    }

    public NaturalNumber Games => NaturalNumber.Create(_battingStatsByGames.Count);

    public NaturalNumber AtBats => NaturalNumber.Create(_battingStatsByGames.Sum(x => x.AtBats.Value));

    public BattingAverage BattingAverage => BattingAverage.Create(_battingStatsByGames.Sum(x => x.Hits.Value),
        _battingStatsByGames.Sum(x => x.AtBats.Value));

    public void LogBattingGame(PlayerBattingStatsByGame stats)
    {
    }

    public static PlayerStatsBySeason Create(MlbId playerId, SeasonYear seasonYear,
        List<PlayerBattingStatsByGame> battingStatsByGames, List<PlayerFieldingStatsByGame> fieldingStatsByGames,
        List<PlayerPitchingStatsByGame> pitchingStatsByGames)
    {
        return new PlayerStatsBySeason(playerId, seasonYear, battingStatsByGames, fieldingStatsByGames,
            pitchingStatsByGames);
    }
}