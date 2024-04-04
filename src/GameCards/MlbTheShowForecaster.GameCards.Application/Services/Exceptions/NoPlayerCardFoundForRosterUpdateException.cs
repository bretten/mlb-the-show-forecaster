using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;

/// <summary>
/// Thrown when there is no <see cref="PlayerCard"/> in the domain yet a roster update was found for it in the external
/// source
/// </summary>
public sealed class NoPlayerCardFoundForRosterUpdateException : Exception
{
    public NoPlayerCardFoundForRosterUpdateException(string? message) : base(message)
    {
    }
}