namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

/// <summary>
/// Represents a set of mutations on the system that depend on each other and must be committed together.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Commits all of the mutations that depend on each other
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>The completed Task</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);
}