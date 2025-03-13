using System.Data.Common;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;

/// <summary>
/// Represents an atomic database operation using <see cref="System.Data.Common"/> database abstractions
/// </summary>
public sealed class DbAtomicDatabaseOperation : IAtomicDatabaseOperation
{
    /// <summary>
    /// Data source for getting connections
    /// </summary>
    private readonly DbDataSource _dataSource;

    /// <summary>
    /// The current, active database connection
    /// </summary>
    private DbConnection? _dbConnection;

    /// <summary>
    /// The current, active database transaction
    /// </summary>
    private DbTransaction? _dbTransaction;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dataSource">Data source for getting connections</param>
    public DbAtomicDatabaseOperation(DbDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    /// <inheritdoc />
    public async Task<DbConnection> GetCurrentActiveConnection(CancellationToken cancellationToken = default)
    {
        if (_dbConnection == null)
        {
            _dbConnection = await _dataSource.OpenConnectionAsync(cancellationToken);
        }

        return _dbConnection;
    }

    /// <inheritdoc />
    public async Task<DbTransaction> GetCurrentActiveTransaction(CancellationToken cancellationToken = default)
    {
        var connection = await GetCurrentActiveConnection(cancellationToken);
        if (_dbTransaction == null)
        {
            _dbTransaction = await connection.BeginTransactionAsync(cancellationToken);
        }

        return _dbTransaction;
    }

    /// <summary>
    /// Disposes of all database resources
    /// </summary>
    public void Dispose()
    {
        _dbTransaction?.Dispose();
        _dbConnection?.Dispose();
    }

    /// <summary>
    /// Disposes of all database resources
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_dbTransaction != null)
        {
            await _dbTransaction.DisposeAsync();
        }

        if (_dbConnection != null)
        {
            await _dbConnection.DisposeAsync();
        }
    }
}