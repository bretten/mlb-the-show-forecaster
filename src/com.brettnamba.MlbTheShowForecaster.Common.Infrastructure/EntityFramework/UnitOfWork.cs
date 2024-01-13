using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFramework;

/// <summary>
/// An EF implementation of unit of work that forces any changes to the DB context (since the last call to commit)
/// to be encapsulated as a single, logical unit of work by only saving changes when commit is invoked
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public sealed class UnitOfWork<TDbContext> : IUnitOfWork, IDisposable, IAsyncDisposable where TDbContext : DbContext
{
    /// <summary>
    /// The DB context
    /// </summary>
    private readonly TDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">The DB context</param>
    public UnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Saves changes to the database
    ///
    /// <para>Commit is meant to be invoked after all changes that represent a unit of work are made on the
    /// <see cref="DbContext"/> so that they are encapsulated in a logical transaction</para>
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes of the DB context
    /// </summary>
    public void Dispose()
    {
        _dbContext.Dispose();
    }

    /// <summary>
    /// Disposes of the DB context
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}