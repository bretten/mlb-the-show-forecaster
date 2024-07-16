using System.Collections.Immutable;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents all the <see cref="RosterUpdate"/>s that have occurred
/// </summary>
/// <param name="RosterUpdates">All of the <see cref="RosterUpdate"/>s up until this point</param>
public readonly record struct AllRosterUpdates(IEnumerable<RosterUpdate> RosterUpdates)
{
    /// <summary>
    /// Returns all <see cref="RosterUpdate"/>s in chronological order
    /// </summary>
    public IReadOnlyCollection<RosterUpdate> OldToNew => RosterUpdates.OrderBy(x => x.Date).ToImmutableList();

    /// <summary>
    /// Returns all <see cref="RosterUpdate"/>s in order of most recent to oldest
    /// </summary>
    public IReadOnlyCollection<RosterUpdate> NewToOld => RosterUpdates.OrderByDescending(x => x.Date).ToImmutableList();
}