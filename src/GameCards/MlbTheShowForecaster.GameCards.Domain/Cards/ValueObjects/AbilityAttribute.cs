using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

/// <summary>
/// Represents the ability of a player in a specific attribute, such as contact vs left pitchers or power vs
/// right pitchers. The ability can range from 0 to 125, with 125 meaning the player is exceptional in that ability
/// </summary>
public sealed class AbilityAttribute : ValueObject
{
    /// <summary>
    /// The minimum allowed value
    /// </summary>
    private const int MinValue = 0;

    /// <summary>
    /// The maximum allowed value
    /// </summary>
    private const int MaxValue = 125;

    /// <summary>
    /// The underlying ability rating
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The ability rating</param>
    [JsonConstructor]
    private AbilityAttribute(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="AbilityAttribute"/>
    /// </summary>
    /// <param name="abilityRating">The ability rating</param>
    /// <returns><see cref="AbilityAttribute"/></returns>
    /// <exception cref="AbilityAttributeOutOfRangeException">Thrown if the specified rating value is not within the valid range</exception>
    public static AbilityAttribute Create(int abilityRating)
    {
        if (abilityRating < 0 || abilityRating > 125)
        {
            throw new AbilityAttributeOutOfRangeException(
                $"The attribute rating of {abilityRating} is not between {MinValue} and {MaxValue}");
        }

        return new AbilityAttribute(abilityRating);
    }
}