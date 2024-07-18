using System.Collections.Immutable;
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
    /// The player ability attributes
    /// </summary>
    public PlayerCardAttributes PlayerCardAttributes { get; private set; } = null!;

    /// <summary>
    /// The different ratings this player card has had in chronological order
    /// </summary>
    public IReadOnlyList<PlayerCardHistoricalRating> HistoricalRatingsChronologically =>
        _historicalRatings.OrderBy(x => x.StartDate).ToImmutableList();

    /// <summary>
    /// A temporary and minor <see cref="OverallRating"/> change that is influenced by real-world events or match-ups
    /// </summary>
    public OverallRating? TemporaryOverallRating => GetCurrentTemporaryRatingFromHistory()?.OverallRating;

    /// <summary>
    /// True if the player card has a significant rating and attribute boost
    /// </summary>
    public bool IsBoosted => _historicalRatings.Any(x => x.IsBoost && !x.EndDate.HasValue);

    /// <summary>
    /// True if the player card has a temporary rating
    /// </summary>
    public bool HasTemporaryRating => TemporaryOverallRating != null;

    /// <summary>
    /// The start of the season for this card
    /// </summary>
    private DateOnly StartOfSeason => new(Year.Value, 1, 1);

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
        // Before replacing the current rating, set its end date in the history
        var currentRating = GetCurrentRatingFromHistory();
        if (currentRating != null)
        {
            // The end date for the current rating is when the new rating begins
            currentRating.End(date);
        }
        else
        {
            // If there was no rating in the history, this is the first change. Add the original state to the history
            AddBaselineHistoricalRating(StartOfSeason, date, OverallRating, PlayerCardAttributes);
        }

        // The new rating is added to the history, without an end date
        AddBaselineHistoricalRating(date, null, newOverallRating, newAttributes);

        // Raise domain events based on whether the rating increased or decreased
        AssessOverallRatingChange(newOverallRating, newAttributes);

        // Set the new rarity
        if (OverallRating.Rarity != newOverallRating.Rarity)
        {
            ChangeRarity(newOverallRating.Rarity);
        }

        // Set the new values
        OverallRating = newOverallRating;
        PlayerCardAttributes = newAttributes;
    }

    /// <summary>
    /// Assigns a temporary <see cref="OverallRating"/> change for the card
    /// </summary>
    /// <param name="date">The date the temporary rating was assigned</param>
    /// <param name="temporaryOverallRating">The temporary <see cref="OverallRating"/></param>
    public void SetTemporaryRating(DateOnly date, OverallRating temporaryOverallRating)
    {
        // Add the temporary rating to the history
        AddTemporaryHistoricalRating(date, null, temporaryOverallRating, PlayerCardAttributes);

        // Raise domain events based on whether the rating increased or decreased
        AssessOverallRatingChange(temporaryOverallRating, PlayerCardAttributes);
    }

    /// <summary>
    /// Removes the temporary <see cref="OverallRating"/>
    /// </summary>
    /// <param name="date">The date the temporary rating ended</param>
    public void RemoveTemporaryRating(DateOnly date)
    {
        // End the current temporary rating
        var temporaryRatingHistory = GetCurrentTemporaryRatingFromHistory();
        temporaryRatingHistory?.End(date);
    }

    /// <summary>
    /// Significantly increases the rating and attributes of the card
    /// </summary>
    /// <param name="date">The date of the boost</param>
    /// <param name="boostedAttributes">The boosted <see cref="PlayerCardAttributes"/></param>
    public void Boost(DateOnly date, PlayerCardAttributes boostedAttributes)
    {
        var boostedRating = OverallRating.Max();
        PlayerCardAttributes = boostedAttributes;

        // Add the boosted rating to the history
        AddBoostedHistoricalRating(date, null, boostedRating, boostedAttributes);

        // Raise a domain event that this card has a significant rating boost
        RaiseDomainEvent(new PlayerCardBoostedEvent(ExternalId, boostedRating, boostedAttributes));
    }

    /// <summary>
    /// Removes the significant boost of rating and attributes
    /// </summary>
    /// <param name="date">The date the boost was removed</param>
    /// <param name="normalAttributes">The normal <see cref="PlayerCardAttributes"/></param>
    public void RemoveBoost(DateOnly date, PlayerCardAttributes normalAttributes)
    {
        PlayerCardAttributes = normalAttributes;

        // Add the end date on the boosted rating in the history
        var temporaryRatingHistory = GetCurrentTemporaryRatingFromHistory();
        temporaryRatingHistory?.End(date);
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
                $"A player rating already exists for card = {ExternalId.Value}, type = {rating.Type} and StartDate = {rating.StartDate.ToShortDateString()}");
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
        return _historicalRatings.Any(x => date == x.StartDate);
    }

    /// <summary>
    /// Determines if the <see cref="PlayerCardHistoricalRating"/> exists in <see cref="_historicalRatings"/>
    /// </summary>
    /// <param name="rating">The <see cref="PlayerCardHistoricalRating"/> to check</param>
    /// <returns>True if the <see cref="PlayerCardHistoricalRating"/> exists, otherwise false</returns>
    private bool DoesHistoricalRatingExist(PlayerCardHistoricalRating rating)
    {
        return _historicalRatings.Any(x => x.StartDate == rating.StartDate && x.Type == rating.Type);
    }

    /// <summary>
    /// Compares the current <see cref="OverallRating"/> to the specified new rating. If there is an increase,
    /// it raises a domain improvement event. If there is a decrease, it raises a domain decline event
    /// </summary>
    /// <param name="newRating">The new <see cref="OverallRating"/></param>
    /// <param name="newAttributes">The new <see cref="PlayerCardAttributes"/></param>
    private void AssessOverallRatingChange(OverallRating newRating, PlayerCardAttributes newAttributes)
    {
        var rarityChanged = OverallRating.Rarity != newRating.Rarity;

        // Raise domain events based on whether the rating increased or decreased
        if (OverallRating.Value < newRating.Value)
        {
            RaiseDomainEvent(new PlayerCardOverallRatingImprovedEvent(ExternalId,
                PreviousOverallRating: OverallRating,
                PreviousPlayerCardAttributes: PlayerCardAttributes,
                NewOverallRating: newRating,
                NewPlayerCardAttributes: newAttributes,
                RarityChanged: rarityChanged));
        }
        else if (OverallRating.Value > newRating.Value)
        {
            RaiseDomainEvent(new PlayerCardOverallRatingDeclinedEvent(ExternalId,
                PreviousOverallRating: OverallRating,
                PreviousPlayerCardAttributes: PlayerCardAttributes,
                NewOverallRating: newRating,
                NewPlayerCardAttributes: newAttributes,
                RarityChanged: rarityChanged));
        }
        // If the overall rating hasn't changed, it means the player has negligible changes, and is not important or actionable
    }

    /// <summary>
    /// Gets the <see cref="PlayerCardHistoricalRating"/> from the history that represents the current state of the card after the most recent change 
    /// </summary>
    /// <returns>The current/most recent <see cref="PlayerCardHistoricalRating"/> or null if there has been no change</returns>
    private PlayerCardHistoricalRating? GetCurrentRatingFromHistory()
    {
        return _historicalRatings.Where(x => x.IsBaseline && !x.EndDate.HasValue)
            .MaxBy(x => x.StartDate);
    }

    /// <summary>
    /// Gets a <see cref="PlayerCardHistoricalRating"/> that represents the current, temporary rating that is overriding
    /// the card's normal rating
    /// <para>The reason for the temporary rating change is due to a real-world accomplishment by the player or
    /// a match-up predicted by MLB Inside Edge</para>
    /// </summary>
    /// <returns>The current, temporary <see cref="PlayerCardHistoricalRating"/> or null if there is no temporary change</returns>
    private PlayerCardHistoricalRating? GetCurrentTemporaryRatingFromHistory()
    {
        return _historicalRatings.Where(x => (x.IsTemporary || x.IsBoost) && !x.EndDate.HasValue)
            .MaxBy(x => x.StartDate);
    }

    /// <summary>
    /// Adds a <see cref="PlayerCardHistoricalRating"/> to the history that represents a baseline rating change for the card
    /// </summary>
    /// <param name="startDate">The start date of the rating</param>
    /// <param name="endDate">When the rating ended</param>
    /// <param name="overallRating">The <see cref="OverallRating"/> at this point in time</param>
    /// <param name="attributes">The <see cref="PlayerCardAttributes"/> at this point in time</param>
    private void AddBaselineHistoricalRating(DateOnly startDate, DateOnly? endDate, OverallRating overallRating,
        PlayerCardAttributes attributes)
    {
        AddHistoricalRating(PlayerCardHistoricalRating.Baseline(startDate, endDate, overallRating, attributes));
    }

    /// <summary>
    /// Adds a <see cref="PlayerCardHistoricalRating"/> to the history that represents a temporary rating change for the card
    /// </summary>
    /// <param name="startDate">The start date of the rating</param>
    /// <param name="endDate">When the rating ended</param>
    /// <param name="overallRating">The <see cref="OverallRating"/> at this point in time</param>
    /// <param name="attributes">The <see cref="PlayerCardAttributes"/> at this point in time</param>
    private void AddTemporaryHistoricalRating(DateOnly startDate, DateOnly? endDate, OverallRating overallRating,
        PlayerCardAttributes attributes)
    {
        AddHistoricalRating(PlayerCardHistoricalRating.Temporary(startDate, endDate, overallRating, attributes));
    }

    /// <summary>
    /// Adds a <see cref="PlayerCardHistoricalRating"/> to the history that represents a boosted rating change for the card
    /// </summary>
    /// <param name="startDate">The start date of the rating</param>
    /// <param name="endDate">When the rating ended</param>
    /// <param name="overallRating">The <see cref="OverallRating"/> at this point in time</param>
    /// <param name="attributes">The <see cref="PlayerCardAttributes"/> at this point in time</param>
    private void AddBoostedHistoricalRating(DateOnly startDate, DateOnly? endDate, OverallRating overallRating,
        PlayerCardAttributes attributes)
    {
        AddHistoricalRating(PlayerCardHistoricalRating.Boost(startDate, endDate, overallRating, attributes));
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