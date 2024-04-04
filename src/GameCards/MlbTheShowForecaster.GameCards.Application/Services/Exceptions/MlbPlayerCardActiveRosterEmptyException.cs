namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="ICardCatalog"/> cannot find or finds an empty MLB player card active roster
/// </summary>
public sealed class MlbPlayerCardActiveRosterEmptyException : Exception
{
    public MlbPlayerCardActiveRosterEmptyException(string? message) : base(message)
    {
    }
}