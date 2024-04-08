namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

/// <summary>
/// Thrown when <see cref="MlbTheShowRosterUpdateMapper"/> encounters an attribute amount that is not convertable to
/// a numerical form
/// </summary>
public sealed class InvalidTheShowAttributeAmountException : Exception
{
    public InvalidTheShowAttributeAmountException(string? message) : base(message)
    {
    }
}