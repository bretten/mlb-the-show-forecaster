using Npgsql;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore.Exceptions;

/// <summary>
/// Thrown when <see cref="HybridNpgsqlEntityFrameworkCoreListingRepository"/> is provided an invalid <see cref="NpgsqlDataSource"/>
/// </summary>
public sealed class InvalidNpgsqlDataSourceForListingRepositoryException : Exception
{
    public InvalidNpgsqlDataSourceForListingRepositoryException(string? message) : base(message)
    {
    }
}