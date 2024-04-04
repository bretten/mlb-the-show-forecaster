namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="IRosterUpdateOrchestrator"/> cannot find a player card from the external source
/// </summary>
public sealed class NoExternalPlayerCardFoundForRosterUpdateException : Exception
{
    public NoExternalPlayerCardFoundForRosterUpdateException(string? message) : base(message)
    {
    }
}