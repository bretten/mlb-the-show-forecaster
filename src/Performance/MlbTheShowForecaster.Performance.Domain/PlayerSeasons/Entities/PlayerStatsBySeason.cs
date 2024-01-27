using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

public sealed class PlayerStatsBySeason : AggregateRoot
{
    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId MlbId { get; private set; }

    /// <summary>
    /// The season
    /// </summary>
    public int SeasonYear { get; private set; }

    public List<PlayerBattingStatsByGame> BattingStatsByGames { get; private set; }
    public List<PlayerPitchingStatsByGame> PitchingStatsByGames { get; private set; }
    public List<PlayerFieldingStatsByGame> FieldingStatsByGames { get; private set; }

    public PlayerStatsBySeason(Guid id) : base(id)
    {
    }
}