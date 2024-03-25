using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

/// <summary>
/// Represents the overall rating of a card
/// </summary>
public sealed class OverallRating : ValueObject
{
    /// <summary>
    /// The minimum allowed value
    /// </summary>
    private const int MinValue = 40;

    /// <summary>
    /// The maximum allowed value
    /// </summary>
    private const int MaxValue = 99;

    /// <summary>
    /// Diamond rarity max rating
    /// </summary>
    private const int DiamondUpperEndpoint = MaxValue;

    /// <summary>
    /// Diamond rarity min rating
    /// </summary>
    private const int DiamondLowerEndpoint = 85;

    /// <summary>
    /// Gold rarity max rating
    /// </summary>
    private const int GoldUpperEndpoint = 84;

    /// <summary>
    /// Gold rarity min rating
    /// </summary>
    private const int GoldLowerEndpoint = 80;

    /// <summary>
    /// Silver rarity max rating
    /// </summary>
    private const int SilverUpperEndpoint = 79;

    /// <summary>
    /// Silver rarity min rating
    /// </summary>
    private const int SilverLowerEndpoint = 75;

    /// <summary>
    /// Bronze rarity max rating
    /// </summary>
    private const int BronzeUpperEndpoint = 74;

    /// <summary>
    /// Bronze rarity min rating
    /// </summary>
    private const int BronzeLowerEndpoint = 65;

    /// <summary>
    /// Common rarity max rating
    /// </summary>
    private const int CommonUpperEndpoint = 64;

    /// <summary>
    /// Common rarity min rating
    /// </summary>
    private const int CommonLowerEndpoint = MinValue;

    /// <summary>
    /// The underlying rating value
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The rating</param>
    private OverallRating(int value)
    {
        Value = value;
    }

    /// <summary>
    /// The rarity implied by the rating
    /// </summary>
    private Rarity? _rarity;

    /// <summary>
    /// The rarity implied by the rating
    /// </summary>
    /// <exception cref="OverallRatingCannotDetermineRarityException">Thrown when the <see cref="Value"/> does not correspond to a rarity</exception>
    public Rarity Rarity
    {
        get
        {
            if (_rarity.HasValue)
            {
                return _rarity.Value;
            }

            if (IsDiamond())
            {
                _rarity = Rarity.Diamond;
            }
            else if (IsGold())
            {
                _rarity = Rarity.Gold;
            }
            else if (IsSilver())
            {
                _rarity = Rarity.Silver;
            }
            else if (IsBronze())
            {
                _rarity = Rarity.Bronze;
            }
            else
            {
                _rarity = Rarity.Common;
            }

            return _rarity.Value;
        }
    }

    /// <summary>
    /// Checks if the rating implies a Diamond rarity
    /// </summary>
    /// <returns>True if the rarity is Diamond, otherwise false</returns>
    private bool IsDiamond()
    {
        return Value >= DiamondLowerEndpoint && Value <= DiamondUpperEndpoint;
    }

    /// <summary>
    /// Checks if the rating implies a Gold rarity
    /// </summary>
    /// <returns>True if the rarity is Gold, otherwise false</returns>
    private bool IsGold()
    {
        return Value >= GoldLowerEndpoint && Value <= GoldUpperEndpoint;
    }

    /// <summary>
    /// Checks if the rating implies a Silver rarity
    /// </summary>
    /// <returns>True if the rarity is Silver, otherwise false</returns>
    private bool IsSilver()
    {
        return Value >= SilverLowerEndpoint && Value <= SilverUpperEndpoint;
    }

    /// <summary>
    /// Checks if the rating implies a Bronze rarity
    /// </summary>
    /// <returns>True if the rarity is Bronze, otherwise false</returns>
    private bool IsBronze()
    {
        return Value >= BronzeLowerEndpoint && Value <= BronzeUpperEndpoint;
    }

    /// <summary>
    /// Creates a <see cref="OverallRating"/>
    /// </summary>
    /// <param name="rating">The rating</param>
    /// <returns><see cref="OverallRating"/></returns>
    /// <exception cref="OverallRatingOutOfRangeException">Thrown if the specified rating value is not within the valid range</exception>
    public static OverallRating Create(int rating)
    {
        if (rating < MinValue || rating > MaxValue)
        {
            throw new OverallRatingOutOfRangeException(
                $"The overall rating of {rating} is not between {MinValue} and {MaxValue}");
        }

        return new OverallRating(rating);
    }
}