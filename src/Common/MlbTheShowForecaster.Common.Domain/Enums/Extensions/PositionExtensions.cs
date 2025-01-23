namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums.Extensions;

/// <summary>
/// Extension methods for <see cref="Position"/>
/// </summary>
public static class PositionExtensions
{
    /// <summary>
    /// Returns true if the position is only a pitching position
    /// </summary>
    /// <param name="position">The position</param>
    /// <returns>True if the position is only a pitching position, otherwise false</returns>
    public static bool IsOnlyPitcher(this Position position)
    {
        return position is Position.Pitcher or Position.StartingPitcher or Position.ReliefPitcher
            or Position.ClosingPitcher;
    }

    /// <summary>
    /// Returns true if the position is only a batting position
    /// </summary>
    /// <param name="position">The position</param>
    /// <returns>True if the position is only a batting position, otherwise false</returns>
    public static bool IsOnlyBatter(this Position position)
    {
        return !IsOnlyPitcher(position) && position is not Position.None && position is not Position.TwoWayPlayer;
    }

    /// <summary>
    /// Returns true if the position is a two-way player
    /// </summary>
    /// <param name="position">The position</param>
    /// <returns>True if the position is a two-way player, otherwise false</returns>
    public static bool IsTwoWayPlayer(this Position position)
    {
        return position == Position.TwoWayPlayer;
    }
}