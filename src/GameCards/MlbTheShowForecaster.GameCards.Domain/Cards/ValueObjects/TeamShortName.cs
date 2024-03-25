using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

/// <summary>
/// Represents a team's short name
/// </summary>
public sealed class TeamShortName : ValueObject
{
    /// <summary>
    /// The underlying team short name
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The team short name</param>
    private TeamShortName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="TeamShortName"/>
    /// </summary>
    /// <param name="shortTeamName">The team short name</param>
    /// <returns><see cref="TeamShortName"/></returns>
    public static TeamShortName Create(string shortTeamName)
    {
        if (string.IsNullOrWhiteSpace(shortTeamName))
        {
            throw new EmptyTeamShortNameException("The team short name is empty");
        }

        return new TeamShortName(shortTeamName);
    }
}