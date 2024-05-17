using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;

/// <summary>
/// Npgsql implementation of <see cref="IUnitOfWork{T}"/> that creates and scopes a database transaction to the <see cref="IUnitOfWork{T}"/>.
/// <see cref="CommitAsync"/> will commit the nested database transaction and persist any mutations performed by contributors
/// to the unit of work
/// </summary>
/// <typeparam name="T">The type of work that is being committed</typeparam>
public sealed class DbUnitOfWork<T> : IUnitOfWork<T>, IDisposable, IAsyncDisposable where T : IUnitOfWorkType
{
    /// <summary>
    /// The current, active database transaction
    /// </summary>
    private readonly IAtomicDatabaseOperation _atomicDatabaseOperation;

    /// <summary>
    /// Service scope used to resolve services that contribute to this <see cref="DbUnitOfWork{T}"/>
    /// </summary>
    private readonly IServiceScope _scope;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serviceScopeFactory">Factory for the service scope used to resolve services that contribute to this <see cref="DbUnitOfWork{T}"/></param>
    public DbUnitOfWork(IServiceScopeFactory serviceScopeFactory)
    {
        _scope = serviceScopeFactory.CreateScope();
        _atomicDatabaseOperation = _scope.ServiceProvider.GetRequiredService<IAtomicDatabaseOperation>();
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
    /// Commits the database transaction so that all changes are persisted
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        var activeTransaction = await _atomicDatabaseOperation.GetCurrentActiveTransaction();
        await activeTransaction.CommitAsync(cancellationToken);
        await DisposeAsync();
    }

    /// <summary>
    /// Disposes of the connection
    /// </summary>
    public void Dispose()
    {
        _scope.Dispose();
        _atomicDatabaseOperation.Dispose();
    }

    /// <summary>
    /// Disposes of the connection
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        _scope.Dispose();
        await _atomicDatabaseOperation.DisposeAsync();
    }
}