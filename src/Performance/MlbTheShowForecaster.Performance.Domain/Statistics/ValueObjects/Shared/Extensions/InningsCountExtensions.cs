using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared.Extensions;

/// <summary>
/// Extensions for <see cref="InningsCount"/>
/// </summary>
public static class InningsCountExtensions
{
    /// <summary>
    /// Sums a collection of <see cref="InningsCount"/>
    /// </summary>
    /// <param name="inningsCounts">A collection of <see cref="InningsCount"/></param>
    /// <returns>An aggregated <see cref="InningsCount"/></returns>
    public static InningsCount SumInnings(this IEnumerable<InningsCount> inningsCounts)
    {
        return inningsCounts.Aggregate((total, next) =>
        {
            var fullInnings = total.FullInnings.Value + next.FullInnings.Value;
            var additionalOuts = total.AdditionalOuts.Value + next.AdditionalOuts.Value;

            return InningsCount.Create(NaturalNumber.Create(fullInnings), NaturalNumber.Create(additionalOuts));
        });
    }
}