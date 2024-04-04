namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="IRosterUpdateOrchestrator"/> is interrupted due to an inner exception and cannot continue
/// </summary>
public sealed class RosterUpdateOrchestratorInterruptedException : AggregateException
{
    public RosterUpdateOrchestratorInterruptedException(string? message, IEnumerable<Exception> innerExceptions) : base(
        message, innerExceptions)
    {
    }
}