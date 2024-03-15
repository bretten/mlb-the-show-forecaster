using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

/// <summary>
/// Represents a player card in MLB The Show
/// </summary>
public sealed class PlayerCard : Card
{
    /// <summary>
    /// The different ratings this player card has had
    /// </summary>
    private readonly List<PlayerCardHistoricalRating> _historicalRatings;

    /// <summary>
    /// The player's team name abbreviated
    /// </summary>
    public TeamShortName TeamShortName { get; private set; }

    /// <summary>
    /// The overall rating of the card
    /// </summary>
    public OverallRating OverallRating { get; private set; }

    /// <summary>
    /// The player ability attributes
    /// </summary>
    public PlayerCardAttributes PlayerCardAttributes { get; private set; }

    /// <summary>
    /// The different ratings this player card has had in chronological order
    /// </summary>
    public IReadOnlyList<PlayerCardHistoricalRating> HistoricalRatingsChronologically =>
        _historicalRatings.OrderBy(x => x.StartDate).ToImmutableList();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="theShowId">The ID from MLB The Show</param>
    /// <param name="type">The card type</param>
    /// <param name="image">The card image</param>
    /// <param name="name">The name of the card</param>
    /// <param name="rarity">The rarity of the card</param>
    /// <param name="series">The series of the card</param>
    /// <param name="teamShortName">The player's team name abbreviated</param>
    /// <param name="overallRating">The overall rating of the card</param>
    /// <param name="playerCardAttributes">The player ability attributes</param>
    private PlayerCard(CardId theShowId, CardType type, CardImage image, string name, Rarity rarity, CardSeries series,
        TeamShortName teamShortName, OverallRating overallRating, PlayerCardAttributes playerCardAttributes) : base(
        theShowId, type, image, name, rarity, series)
    {
        TeamShortName = teamShortName;
        OverallRating = overallRating;
        PlayerCardAttributes = playerCardAttributes;
        _historicalRatings = new List<PlayerCardHistoricalRating>();
    }

    /// <summary>
    /// Changes the player rating
    /// </summary>
    /// <param name="date">The date the rating change took place</param>
    /// <param name="overallRating">The new overall rating</param>
    /// <param name="attributes">The player's attributes</param>
    public void ChangePlayerRating(DateOnly date, OverallRating overallRating, PlayerCardAttributes attributes)
    {
        if (_historicalRatings.Any(x => x.StartDate <= date && x.EndDate > date))
        {
            throw new PlayerCardHistoricalRatingExistsException(
                $"A player rating already exists for card = {TheShowId.Value} and date = {date.ToShortDateString()}");
        }

        OverallRating = overallRating;
        PlayerCardAttributes = attributes;

        // The end date of the last rating state is the beginning of the rating state that is currently being replaced
        var previousEndDate = _historicalRatings.MaxBy(x => x.EndDate)?.EndDate ?? DateOnly.MinValue;

        _historicalRatings.Add(PlayerCardHistoricalRating.Create(previousEndDate, date, overallRating, attributes));
    }

    /// <summary>
    /// Changes the player card's team
    /// </summary>
    /// <param name="teamShortName">The new team</param>
    public void ChangeTeam(TeamShortName teamShortName)
    {
        TeamShortName = teamShortName;
    }

    /// <summary>
    /// Creates a <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="theShowId">The ID from MLB The Show</param>
    /// <param name="type">The card type</param>
    /// <param name="image">The card image</param>
    /// <param name="name">The name of the card</param>
    /// <param name="rarity">The rarity of the card</param>
    /// <param name="series">The series of the card</param>
    /// <param name="teamShortName">The player's team name abbreviated</param>
    /// <param name="overallRating">The overall rating of the card</param>
    /// <param name="playerCardAttributes">The player ability attributes</param>
    /// <returns><see cref="PlayerCard"/></returns>
    public static PlayerCard Create(CardId theShowId, CardType type, CardImage image, string name, Rarity rarity,
        CardSeries series, TeamShortName teamShortName, OverallRating overallRating,
        PlayerCardAttributes playerCardAttributes)
    {
        return new PlayerCard(theShowId, type, image, name, rarity, series, teamShortName, overallRating,
            playerCardAttributes);
    }
}