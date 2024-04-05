namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="ICardCatalog"/> cannot find the specified card
/// </summary>
public sealed class MlbPlayerCardNotFoundInCatalogException : Exception
{
    public MlbPlayerCardNotFoundInCatalogException(string? message) : base(message)
    {
    }
}