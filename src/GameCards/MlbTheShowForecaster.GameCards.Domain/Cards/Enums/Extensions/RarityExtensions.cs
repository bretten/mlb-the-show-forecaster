namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums.Extensions;

/// <summary>
/// Define extensions for <see cref="Rarity"/>
/// </summary>
public static class RarityExtensions
{
    /// <summary>
    /// Checks if the left <see cref="Rarity"/> is greater than the right <see cref="Rarity"/>
    /// </summary>
    /// <param name="l">The left <see cref="Rarity"/></param>
    /// <param name="r">The right <see cref="Rarity"/></param>
    /// <returns>True if the left rarity is greater</returns>
    public static bool GreaterThan(this Rarity l, Rarity r)
    {
        return (int)l > (int)r;
    }

    /// <summary>
    /// Checks if the left <see cref="Rarity"/> is less than the right <see cref="Rarity"/>
    /// </summary>
    /// <param name="l">The left <see cref="Rarity"/></param>
    /// <param name="r">The right <see cref="Rarity"/></param>
    /// <returns>True if the left rarity is less</returns>
    public static bool LessThan(this Rarity l, Rarity r)
    {
        return (int)l < (int)r;
    }
}