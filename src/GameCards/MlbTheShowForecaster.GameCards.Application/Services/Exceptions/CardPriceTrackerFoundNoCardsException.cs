using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="ICardPriceTracker"/> found no <see cref="PlayerCard"/>s
/// </summary>
public sealed class CardPriceTrackerFoundNoCardsException : Exception
{
    public CardPriceTrackerFoundNoCardsException(string? message) : base(message)
    {
    }
}