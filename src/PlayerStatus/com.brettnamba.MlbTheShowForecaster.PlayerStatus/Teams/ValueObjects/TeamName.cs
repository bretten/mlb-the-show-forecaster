using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

/// <summary>
/// A MLB Team's name
/// </summary>
public sealed class TeamName : ValueObject
{
    /// <summary>
    /// The underlying name value
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The name value</param>
    private TeamName(string value)
    {
        Value = value;
    }

    public static TeamName Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new EmptyTeamNameException("Team name cannot be empty");
        }

        return new TeamName(name);
    }
}