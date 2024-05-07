using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFrameworkCore;

/// <summary>
/// An EF implementation of unit of work that forces any changes to the DB context (since the last call to commit)
/// to be encapsulated as a single, logical unit of work by only saving changes when commit is invoked
/// </summary>
/// <typeparam name="TDbContext">The type of work that is being committed. In this case, the work is for a <see cref="DbContext"/></typeparam>
public sealed class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext>, IDisposable, IAsyncDisposable
    where TDbContext : DbContext, IUnitOfWorkType
{
    /// <summary>
    /// The DB context
    /// </summary>
    private readonly TDbContext _dbContext;

    /// <summary>
    /// Publishes all domain events that were raised by any <see cref="Entity"/> that was changed
    /// </summary>
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">The DB context</param>
    /// <param name="domainEventDispatcher">Publishes all domain events that were raised by any <see cref="Entity"/> that was changed</param>
    public UnitOfWork(TDbContext dbContext, IDomainEventDispatcher domainEventDispatcher)
    {
        _dbContext = dbContext;
        _domainEventDispatcher = domainEventDispatcher;
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
        PublishDomainEvents();

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

    /// <summary>
    /// Checks for any Entities that were modified in the DB context and publishes their domain events
    /// </summary>
    private void PublishDomainEvents()
    {
        var domainEventsToPublish = new List<IDomainEvent>();

        // Look for any Entities that were modified and get their domain events
        _dbContext.ChangeTracker
            .Entries<Entity>()
            .Select(x => x.Entity)
            .ToList()
            .ForEach(x =>
            {
                domainEventsToPublish.AddRange(x.DomainEvents);
                x.ClearDomainEvents();
            });

        _domainEventDispatcher.Dispatch(domainEventsToPublish);
    }
}