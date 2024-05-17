using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFrameworkCore;

/// <summary>
/// An <see cref="Microsoft.EntityFrameworkCore"/> implementation <see cref="IUnitOfWork{T}"/> that creates and scopes a
/// <see cref="DbContext"/> to an instance of <see cref="UnitOfWork{T}"/>. <see cref="CommitAsync"/> will invoke the
/// <see cref="DbContext"/>'s SaveChanges and persist any mutations performed
/// by contributors to the unit of work
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
    /// Service scope used to resolve services that contribute to this <see cref="UnitOfWork{T}"/>
    /// </summary>
    private readonly IServiceScope _scope;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="domainEventDispatcher">Publishes all domain events that were raised by any <see cref="Entity"/> that was changed</param>
    /// <param name="serviceScopeFactory">Factory for the service scope used to resolve services that contribute to this <see cref="UnitOfWork{T}"/></param>
    public UnitOfWork(IDomainEventDispatcher domainEventDispatcher, IServiceScopeFactory serviceScopeFactory)
    {
        _domainEventDispatcher = domainEventDispatcher;
        _scope = serviceScopeFactory.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<TDbContext>();
    }

    /// <summary>
    /// Gets a contributor to the unit of work of the specified type <see cref="TContributor"/>
    /// </summary>
    /// <typeparam name="TContributor">The type of contributor to get</typeparam>
    /// <returns>The contributor to the unit of work</returns>
    /// <exception cref="UnitOfWorkContributorNotFoundException">Thrown if the contributor could not be found</exception>
    public TContributor GetContributor<TContributor>() where TContributor : notnull
    {
        return _scope.ServiceProvider.GetService<TContributor>() ??
               throw new UnitOfWorkContributorNotFoundException(
                   $"Contributor of type {nameof(TContributor)} not found");
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
        await DisposeAsync();
    }

    /// <summary>
    /// Disposes of the DB context
    /// </summary>
    public void Dispose()
    {
        _dbContext.Dispose();
        _scope.Dispose();
    }

    /// <summary>
    /// Disposes of the DB context
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        _scope.Dispose();
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