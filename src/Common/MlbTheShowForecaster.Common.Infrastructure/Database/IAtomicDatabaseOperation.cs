using System.Data.Common;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;

/// <summary>
/// Represents an atomic database operation, such as a transaction
/// </summary>
public interface IAtomicDatabaseOperation : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the current, active database connection for the operation
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The current, active database connection for the operation</returns>
    Task<DbConnection> GetCurrentActiveConnection(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current, active database transaction for the operation
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The current, active database transaction for the operation</returns>
    Task<DbTransaction> GetCurrentActiveTransaction(CancellationToken cancellationToken = default);
}