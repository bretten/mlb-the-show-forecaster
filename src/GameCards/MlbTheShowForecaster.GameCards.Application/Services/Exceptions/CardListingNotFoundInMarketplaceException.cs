namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="ICardMarketplace"/> cannot find the specified listing
/// </summary>
public sealed class CardListingNotFoundInMarketplaceException : Exception
{
    public CardListingNotFoundInMarketplaceException(string? message) : base(message)
    {
    }
}