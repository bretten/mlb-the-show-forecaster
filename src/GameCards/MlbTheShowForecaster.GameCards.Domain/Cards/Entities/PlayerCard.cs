using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;
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
    /// <param name="theShowId">The card ID from MLB The Show</param>
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
    /// <param name="newOverallRating">The new overall rating</param>
    /// <param name="newAttributes">The player's new attributes</param>
    public void ChangePlayerRating(DateOnly date, OverallRating newOverallRating, PlayerCardAttributes newAttributes)
    {
        if (_historicalRatings.Any(x => x.StartDate <= date && x.EndDate > date))
        {
            throw new PlayerCardHistoricalRatingExistsException(
                $"A player rating already exists for card = {TheShowId.Value} and date = {date.ToShortDateString()}");
        }

        // The end date of the last rating state is the beginning of the rating state that is currently being replaced
        var previousEndDate = _historicalRatings.MaxBy(x => x.EndDate)?.EndDate ?? DateOnly.MinValue;

        // Add previous values to history before updating with new values
        _historicalRatings.Add(
            PlayerCardHistoricalRating.Create(previousEndDate, date, OverallRating, PlayerCardAttributes));

        // Notify subscribers that the player card overall rating has changed
        if (OverallRating.Value < newOverallRating.Value)
        {
            RaiseDomainEvent(new PlayerCardOverallRatingImprovedEvent(TheShowId, PreviousOverallRating: OverallRating,
                PreviousPlayerCardAttributes: PlayerCardAttributes, NewOverallRating: newOverallRating,
                NewPlayerCardAttributes: PlayerCardAttributes,
                RarityChanged: OverallRating.Rarity != newOverallRating.Rarity));
        }
        else if (OverallRating.Value > newOverallRating.Value)
        {
            RaiseDomainEvent(new PlayerCardOverallRatingDeclinedEvent(TheShowId, PreviousOverallRating: OverallRating,
                PreviousPlayerCardAttributes: PlayerCardAttributes, NewOverallRating: newOverallRating,
                NewPlayerCardAttributes: PlayerCardAttributes,
                RarityChanged: OverallRating.Rarity != newOverallRating.Rarity));
        }
        // If the overall rating hasn't changed, it means the player has negligible changes, and is not important or actionable

        // Set the new values
        OverallRating = newOverallRating;
        PlayerCardAttributes = newAttributes;
    }

    /// <summary>
    /// Changes the player card's team
    /// </summary>
    /// <param name="newTeamShortName">The new team</param>
    public void ChangeTeam(TeamShortName newTeamShortName)
    {
        TeamShortName = newTeamShortName;
    }

    /// <summary>
    /// Creates a <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="theShowId">The card ID from MLB The Show</param>
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