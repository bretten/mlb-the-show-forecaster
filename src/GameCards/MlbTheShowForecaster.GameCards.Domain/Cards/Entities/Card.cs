﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

/// <summary>
/// Represents a Card in MLB The Show
/// </summary>
public abstract class Card : AggregateRoot
{
    /// <summary>
    /// The year of MLB The Show
    /// </summary>
    public SeasonYear Year { get; }

    /// <summary>
    /// The card ID from MLB The Show
    /// </summary>
    public CardExternalId ExternalId { get; }

    /// <summary>
    /// The card type
    /// </summary>
    public CardType Type { get; }

    /// <summary>
    /// The card image location
    /// </summary>
    public CardImageLocation ImageLocation { get; }

    /// <summary>
    /// The name of the card
    /// </summary>
    public CardName Name { get; }

    /// <summary>
    /// The rarity of the card
    /// </summary>
    public Rarity Rarity { get; private set; }

    /// <summary>
    /// The series of the card
    /// </summary>
    public CardSeries Series { get; }

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
    protected Card(SeasonYear year, CardExternalId externalId, CardType type, CardImageLocation imageLocation,
        CardName name, Rarity rarity, CardSeries series) : base(Guid.NewGuid())
    {
        Year = year;
        ExternalId = externalId;
        Type = type;
        ImageLocation = imageLocation;
        Name = name;
        Rarity = rarity;
        Series = series;
    }

    /// <summary>
    /// Sets the rarity
    /// </summary>
    /// <param name="rarity">The new rarity</param>
    protected virtual void ChangeRarity(Rarity rarity)
    {
        Rarity = rarity;
    }
}