namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.Exceptions;

/// <summary>
/// Thrown when an invalid decimal value is provided to <see cref="InningsPitched"/>
/// </summary>
public sealed class InvalidInningsPitchedDecimalException : Exception
{
    public InvalidInningsPitchedDecimalException(string? message) : base(message)
    {
    }
}