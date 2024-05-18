using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;

/// <summary>
/// Implementation of <see cref="IUnitOfWork{T}"/> using <see cref="System.Data.Common"/> database abstractions.
/// It creates and scopes a database transaction to an instance of <see cref="DbUnitOfWork{T}"/>. <see cref="CommitAsync"/>
/// will commit the nested database transaction and persist any mutations performed by contributors to the unit of work
/// </summary>
/// <typeparam name="T">The type of work that is being committed</typeparam>
public sealed class DbUnitOfWork<T> : ScopedUnitOfWork<T> where T : IUnitOfWorkType
{
    /// <summary>
    /// The current, active database transaction
    /// </summary>
    private readonly IAtomicDatabaseOperation _atomicDatabaseOperation;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serviceScopeFactory">Factory for the service scope used to resolve services that contribute to this <see cref="DbUnitOfWork{T}"/></param>
    public DbUnitOfWork(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
    {
        _atomicDatabaseOperation = Scope.ServiceProvider.GetRequiredService<IAtomicDatabaseOperation>();
    }

    /// <summary>
    /// Commits the database transaction so that all changes are persisted
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public override async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        var activeTransaction = await _atomicDatabaseOperation.GetCurrentActiveTransaction(cancellationToken);
        await activeTransaction.CommitAsync(cancellationToken);
        await DisposeAsync();
    }

    /// <summary>
    /// Disposes of the connection
    /// </summary>
    public override void Dispose()
    {
        _atomicDatabaseOperation.Dispose();
        base.Dispose();
    }

    /// <summary>
    /// Disposes of the connection
    /// </summary>
    public override async ValueTask DisposeAsync()
    {
        await _atomicDatabaseOperation.DisposeAsync();
        await base.DisposeAsync();
    }
}