﻿using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
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
    /// The player card's primary position
    /// </summary>
    public Position Position { get; private set; }

    /// <summary>
    /// The player's team name abbreviated
    /// </summary>
    public TeamShortName TeamShortName { get; private set; }

    /// <summary>
    /// The overall rating of the card
    /// </summary>
    public OverallRating OverallRating { get; private set; }

    /// <summary>
    /// A temporary and minor overall rating change based off of real-world stats
    /// </summary>
    public OverallRating? TemporaryOverallRating { get; private set; }

    /// <summary>
    /// True if the player card has a significant rating and attribute boost
    /// </summary>
    public bool IsBoosted { get; private set; }

    /// <summary>
    /// The player ability attributes
    /// </summary>
    public PlayerCardAttributes PlayerCardAttributes { get; private set; } = null!;

    /// <summary>
    /// The different ratings this player card has had in chronological order
    /// </summary>
    public IReadOnlyList<PlayerCardHistoricalRating> HistoricalRatingsChronologically =>
        _historicalRatings.OrderBy(x => x.StartDate).ToImmutableList();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="year">The year of MLB The Show</param>
    /// <param name="externalId">The card ID from MLB The Show</param>
    /// <param name="type">The card type</param>
    /// <param name="imageLocation">The card image location</param>
    /// <param name="name">The name of the card</param>
    /// <param name="rarity">The rarity of the card</param>
    /// <param name="series">The series of the card</param>
    /// <param name="position">The player card's primary position</param>
    /// <param name="teamShortName">The player's team name abbreviated</param>
    /// <param name="overallRating">The overall rating of the card</param>
    /// <param name="playerCardAttributes">The player ability attributes</param>
    private PlayerCard(SeasonYear year, CardExternalId externalId, CardType type, CardImageLocation imageLocation,
        CardName name, Rarity rarity, CardSeries series, Position position, TeamShortName teamShortName,
        OverallRating overallRating, PlayerCardAttributes playerCardAttributes) : base(year, externalId, type,
        imageLocation, name, rarity, series)
    {
        Position = position;
        TeamShortName = teamShortName;
        OverallRating = overallRating;
        PlayerCardAttributes = playerCardAttributes;
        _historicalRatings = new List<PlayerCardHistoricalRating>();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="year">The year of MLB The Show</param>
    /// <param name="externalId">The card ID from MLB The Show</param>
    /// <param name="type">The card type</param>
    /// <param name="imageLocation">The card image location</param>
    /// <param name="name">The name of the card</param>
    /// <param name="rarity">The rarity of the card</param>
    /// <param name="series">The series of the card</param>
    /// <param name="position">The player card's primary position</param>
    /// <param name="teamShortName">The player's team name abbreviated</param>
    /// <param name="overallRating">The overall rating of the card</param>
    private PlayerCard(SeasonYear year, CardExternalId externalId, CardType type, CardImageLocation imageLocation,
        CardName name, Rarity rarity, CardSeries series, Position position, TeamShortName teamShortName,
        OverallRating overallRating) : base(year, externalId, type, imageLocation, name, rarity, series)
    {
        Position = position;
        TeamShortName = teamShortName;
        OverallRating = overallRating;
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
                $"A player rating already exists for card = {ExternalId.Value} and date = {date.ToShortDateString()}");
        }

        // The end date of the last rating state is the beginning of the rating state that is currently being replaced
        var previousEndDate = _historicalRatings.MaxBy(x => x.EndDate)?.EndDate ?? new DateOnly(date.Year, 1, 1);

        // Add previous values to history before updating with new values
        _historicalRatings.Add(
            PlayerCardHistoricalRating.Create(previousEndDate, date, OverallRating, PlayerCardAttributes));

        var rarityChanged = OverallRating.Rarity != newOverallRating.Rarity;
        // Notify subscribers that the player card overall rating has changed
        if (OverallRating.Value < newOverallRating.Value)
        {
            RaiseDomainEvent(new PlayerCardOverallRatingImprovedEvent(ExternalId, PreviousOverallRating: OverallRating,
                PreviousPlayerCardAttributes: PlayerCardAttributes, NewOverallRating: newOverallRating,
                NewPlayerCardAttributes: newAttributes, RarityChanged: rarityChanged));
        }
        else if (OverallRating.Value > newOverallRating.Value)
        {
            RaiseDomainEvent(new PlayerCardOverallRatingDeclinedEvent(ExternalId, PreviousOverallRating: OverallRating,
                PreviousPlayerCardAttributes: PlayerCardAttributes, NewOverallRating: newOverallRating,
                NewPlayerCardAttributes: newAttributes, RarityChanged: rarityChanged));
        }
        // If the overall rating hasn't changed, it means the player has negligible changes, and is not important or actionable

        // Set the new values
        OverallRating = newOverallRating;
        PlayerCardAttributes = newAttributes;
        if (rarityChanged)
        {
            ChangeRarity(newOverallRating.Rarity);
        }
    }

    /// <summary>
    /// Assigns a temporary overall rating change for the card
    /// </summary>
    /// <param name="date">The date the temporary rating was assigned</param>
    /// <param name="temporaryOverallRating">The temporary <see cref="OverallRating"/></param>
    public void SetTemporaryRating(DateOnly date, OverallRating temporaryOverallRating)
    {
        TemporaryOverallRating = temporaryOverallRating;

        AddHistoricalRating(
            PlayerCardHistoricalRating.Create(date, date, temporaryOverallRating, PlayerCardAttributes));

        // Notify subscribers that the player card overall rating has changed
        if (temporaryOverallRating.Value > OverallRating.Value)
        {
            RaiseDomainEvent(new PlayerCardOverallRatingTemporarilyImprovedEvent(ExternalId,
                PreviousOverallRating: OverallRating, NewOverallRating: temporaryOverallRating));
        }
        else if (temporaryOverallRating.Value < OverallRating.Value)
        {
            RaiseDomainEvent(new PlayerCardOverallRatingTemporarilyDeclinedEvent(ExternalId,
                PreviousOverallRating: OverallRating, NewOverallRating: temporaryOverallRating));
        }
    }

    /// <summary>
    /// Removes the temporary overall rating
    /// </summary>
    public void RemoveTemporaryRating(DateOnly date)
    {
        TemporaryOverallRating = null;

        AddHistoricalRating(PlayerCardHistoricalRating.Create(date, date, OverallRating, PlayerCardAttributes));
    }

    /// <summary>
    /// Significantly increases the rating and attributes of the card
    /// </summary>
    /// <param name="date">The date of the boost</param>
    /// <param name="boostedAttributes">The boosted <see cref="PlayerCardAttributes"/></param>
    public void Boost(DateOnly date, PlayerCardAttributes boostedAttributes)
    {
        IsBoosted = true;
        TemporaryOverallRating = OverallRating.Create(99);
        PlayerCardAttributes = boostedAttributes;

        AddHistoricalRating(PlayerCardHistoricalRating.Create(date, date, TemporaryOverallRating, boostedAttributes));

        RaiseDomainEvent(new PlayerCardBoostedEvent(ExternalId, TemporaryOverallRating, boostedAttributes));
    }

    /// <summary>
    /// Removes the significant boost of rating and attributes
    /// </summary>
    /// <param name="date">The date the boost was removed</param>
    /// <param name="normalAttributes">The normal <see cref="PlayerCardAttributes"/></param>
    public void RemoveBoost(DateOnly date, PlayerCardAttributes normalAttributes)
    {
        IsBoosted = false;
        TemporaryOverallRating = null;
        PlayerCardAttributes = normalAttributes;

        AddHistoricalRating(PlayerCardHistoricalRating.Create(date, date, OverallRating, normalAttributes));
    }

    /// <summary>
    /// Adds a <see cref="PlayerCardHistoricalRating"/> to the history
    /// </summary>
    /// <param name="rating"><see cref="PlayerCardHistoricalRating"/></param>
    /// <exception cref="PlayerCardHistoricalRatingExistsException">Thrown if the <see cref="PlayerCardHistoricalRating"/> already exists</exception>
    public void AddHistoricalRating(PlayerCardHistoricalRating rating)
    {
        if (DoesHistoricalRatingExist(rating))
        {
            throw new PlayerCardHistoricalRatingExistsException(
                $"A player rating already exists for card = {ExternalId.Value} and StartDate = {rating.StartDate.ToShortDateString()} and EndDate = {rating.EndDate.ToShortDateString()}");
        }

        _historicalRatings.Add(rating);
    }

    /// <summary>
    /// Changes the player card's primary position
    /// </summary>
    /// <param name="newPosition">The new position</param>
    public void ChangePosition(Position newPosition)
    {
        RaiseDomainEvent(
            new PlayerCardPositionChangedEvent(ExternalId, OldPosition: Position, NewPosition: newPosition));
        Position = newPosition;
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
    /// Returns true if a rating change has already been applied for the specified date
    /// </summary>
    /// <param name="date">The date to check if a rating was applied for</param>
    /// <returns>True if a rating change has already been applied for the specified date, otherwise false</returns>
    public bool IsRatingAppliedFor(DateOnly date)
    {
        return _historicalRatings.Any(x => x.StartDate <= date && date < x.EndDate);
    }

    /// <summary>
    /// Determines if the <see cref="PlayerCardHistoricalRating"/> exists in <see cref="_historicalRatings"/>
    /// </summary>
    /// <param name="rating">The <see cref="PlayerCardHistoricalRating"/> to check</param>
    /// <returns>True if the <see cref="PlayerCardHistoricalRating"/> exists, otherwise false</returns>
    private bool DoesHistoricalRatingExist(PlayerCardHistoricalRating rating)
    {
        return _historicalRatings.Any(x => x.StartDate == rating.StartDate && x.EndDate == rating.EndDate);
    }

    /// <summary>
    /// Creates a <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="year">The year of MLB The Show</param>
    /// <param name="cardExternalId">The card ID from MLB The Show</param>
    /// <param name="type">The card type</param>
    /// <param name="imageLocation">The card image location</param>
    /// <param name="name">The name of the card</param>
    /// <param name="rarity">The rarity of the card</param>
    /// <param name="series">The series of the card</param>
    /// <param name="position">The player card's primary position</param>
    /// <param name="teamShortName">The player's team name abbreviated</param>
    /// <param name="overallRating">The overall rating of the card</param>
    /// <param name="playerCardAttributes">The player ability attributes</param>
    /// <returns><see cref="PlayerCard"/></returns>
    public static PlayerCard Create(SeasonYear year, CardExternalId cardExternalId, CardType type,
        CardImageLocation imageLocation, CardName name, Rarity rarity, CardSeries series, Position position,
        TeamShortName teamShortName, OverallRating overallRating, PlayerCardAttributes playerCardAttributes)
    {
        return new PlayerCard(year, cardExternalId, type, imageLocation, name, rarity, series, position, teamShortName,
            overallRating, playerCardAttributes);
    }
}