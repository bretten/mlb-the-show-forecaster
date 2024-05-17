namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork.Exceptions;

/// <summary>
/// Thrown when <see cref="IUnitOfWork{T}"/> cannot find the contributor of a specified type to contribute to
/// the unit of work
/// </summary>
public sealed class UnitOfWorkContributorNotFoundException : Exception
{
    public UnitOfWorkContributorNotFoundException(string? message) : base(message)
    {
    }
}