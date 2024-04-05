namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="ICardCatalog"/> cannot find any active roster MLB player cards
/// </summary>
public sealed class ActiveRosterMlbPlayerCardsNotFoundInCatalogException : Exception
{
    public ActiveRosterMlbPlayerCardsNotFoundInCatalogException(string? message) : base(message)
    {
    }
}