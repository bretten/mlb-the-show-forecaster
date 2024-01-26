namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

/// <summary>
/// Thrown when an invalid year is provided to <see cref="SeasonYear"/>
/// </summary>
public sealed class InvalidMlbSeasonYearException : Exception
{
    public InvalidMlbSeasonYearException(string? message) : base(message)
    {
    }
}