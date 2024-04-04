namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="ICardCatalog"/> cannot find the specified card
/// </summary>
public sealed class MlbPlayerCardNotFoundException : Exception
{
    public MlbPlayerCardNotFoundException(string? message) : base(message)
    {
    }
}