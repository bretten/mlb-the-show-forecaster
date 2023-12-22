using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

/// <summary>
/// Represents a MLB Team's abbreviation
/// </summary>
public sealed class TeamAbbreviation : ValueObject
{
    /// <summary>
    /// The underlying abbreviation value
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The abbreviation</param>
    private TeamAbbreviation(string value)
    {
        Value = value;
    }

    public static TeamAbbreviation Create(string abbreviation)
    {
        if (abbreviation.Length < 2 || abbreviation.Length > 3)
        {
            throw new InvalidTeamAbbreviationException(
                $"Invalid Team abbreviation. It can only be 2 or 3 characters: {abbreviation}");
        }

        return new TeamAbbreviation(abbreviation);
    }
}