using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.ValueObjects;

/// <summary>
/// Represents the ID that is given to a player by MLB
/// </summary>
public sealed class MlbId : ValueObject
{
    /// <summary>
    /// The underlying MLB ID value
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The MLB ID</param>
    private MlbId(int value)
    {
        Value = value;
    }

    public static MlbId Create(int mlbId)
    {
        if (mlbId < 1)
        {
            throw new InvalidMlbIdException($"MLB ID is not a valid ID as defined by MLB.com: {mlbId}");
        }

        return new MlbId(mlbId);
    }
}