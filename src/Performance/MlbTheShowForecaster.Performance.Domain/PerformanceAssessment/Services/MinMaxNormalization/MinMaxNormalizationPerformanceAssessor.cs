using System.Linq.Expressions;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Contracts;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;

/// <summary>
/// Uses min-max normalization to bring different stats to the same scale, so they can assessed together
/// <para>
/// The min and max for each stat are determined using historical MLB stats. The normalized stat value is calculated with:
/// </para>
/// <para>
/// normalized = (x - xMin) / (xMax - xMin) where x is the value of the stat
/// </para>
/// <para>Weights are then applied to each normalized stat to determine the degree to which they affect the score</para>
/// </summary>
public sealed class MinMaxNormalizationPerformanceAssessor : IPerformanceAssessor
{
    /// <summary>
    /// The normalization criteria
    /// </summary>
    private readonly MinMaxNormalizationCriteria _normalizationCriteria;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="normalizationCriteria">The normalization criteria</param>
    public MinMaxNormalizationPerformanceAssessor(MinMaxNormalizationCriteria normalizationCriteria)
    {
        _normalizationCriteria = normalizationCriteria;
    }

    /// <summary>
    /// Assesses <see cref="BattingStats"/> using min-max normalization
    /// </summary>
    /// <param name="stats"><see cref="BattingStats"/></param>
    /// <returns><see cref="PerformanceScore"/></returns>
    public PerformanceScore AssessBatting(BattingStats stats)
    {
        return CalculateScore(stats, _normalizationCriteria.BattingCriteria);
    }

    /// <summary>
    /// Assesses <see cref="PitchingStats"/> using min-max normalization
    /// </summary>
    /// <param name="stats"><see cref="PitchingStats"/></param>
    /// <returns><see cref="PerformanceScore"/></returns>
    public PerformanceScore AssessPitching(PitchingStats stats)
    {
        if (stats.NumberOfPitches.Value == 0)
        {
            return PerformanceScore.Zero();
        }

        return CalculateScore(stats, _normalizationCriteria.PitchingCriteria);
    }

    /// <summary>
    /// Assesses <see cref="FieldingStats"/> using min-max normalization
    /// </summary>
    /// <param name="stats"><see cref="FieldingStats"/></param>
    /// <returns><see cref="PerformanceScore"/></returns>
    public PerformanceScore AssessFielding(FieldingStats stats)
    {
        return CalculateScore(stats, _normalizationCriteria.FieldingCriteria);
    }

    /// <inheritdoc />
    public PerformanceScoreComparison Compare(PerformanceScore oldScore, PerformanceScore newScore)
    {
        return PerformanceScoreComparison.Create(oldScore, newScore,
            _normalizationCriteria.ScorePercentageChangeThreshold);
    }

    /// <summary>
    /// Calculates a score for a set of stats <see cref="T"/>
    /// </summary>
    /// <param name="stats">A set of stats</param>
    /// <param name="statsToAssess">Which individual stats from the set to assess</param>
    /// <typeparam name="T">The type of stats being assessed</typeparam>
    /// <returns>A normalized performance score</returns>
    /// <exception cref="UnexpectedMinMaxStatTypeException">Thrown when the type of stat is unknown</exception>
    private static PerformanceScore CalculateScore<T>(T stats, IEnumerable<MinMaxStatCriteria> statsToAssess)
    {
        var score = 0m;

        // For each stat that was configured to be assessed, calculate a score based on its min-max value and its weight
        foreach (var statToAssess in statsToAssess)
        {
            // The stats to be assessed are configurable and not known at runtime, so dynamically get their value
            var value = GetStatValue<T, object>(stats, statToAssess.Name);

            // Determine the underling stat value based on type
            var actualValue = value switch
            {
                NaturalNumber n => n.Value,
                InningsCount n => n.Value,
                IStat n => n.Value,
                _ => throw new UnexpectedMinMaxStatTypeException(
                    $"Stat of type {value.GetType().Name} cannot be scored")
            };

            // Normalize the stat value
            var normalizedValue = Normalize(statToAssess, actualValue);

            // Add the weighted score to the total
            score += (statToAssess.Weight * normalizedValue);
        }

        return PerformanceScore.Create(score);
    }

    /// <summary>
    /// Performs min-max normalization on the specified stat's value
    /// </summary>
    /// <param name="stat">The stat to normalize</param>
    /// <param name="value">The value of the stat</param>
    /// <returns>The normalized stat</returns>
    private static decimal Normalize(MinMaxStatCriteria stat, decimal value)
    {
        // If a lower value for the stat means better performance, such as ERA, then it needs to be inverted
        if (stat.IsLowerValueBetter)
        {
            value = stat.Max - value;
        }

        // Min-max normalization
        var normalized = (value - stat.Min) / stat.MaxMinusMin;

        // If the stat exceeds the min or max, cap it at 0 or 1 respectively
        return normalized switch
        {
            > 1 => 1,
            < 0 => 0,
            _ => normalized
        };
    }

    /// <summary>
    /// Gets an individual stat value from a set of stats in <see cref="T"/>. The stats to be assessed are configurable
    /// and not known at runtime, so this uses an <see cref="Expression{TDelegate}"/> to dynamically get the stat
    /// value from the set of stats <see cref="T"/>
    /// </summary>
    /// <param name="stats">The set of stat values</param>
    /// <param name="statName">The name of the individual stat to get the value for</param>
    /// <typeparam name="T">The type of stat set</typeparam>
    /// <typeparam name="TResult">The type of the stat value</typeparam>
    /// <returns>The stat value</returns>
    private static TResult GetStatValue<T, TResult>(T stats, string statName)
    {
        var parameter = Expression.Parameter(typeof(T), "stat");
        var property = Expression.Property(parameter, statName);
        var convert = Expression.Convert(property, typeof(TResult));
        var lambda = Expression.Lambda<Func<T, TResult>>(convert, parameter);
        var func = lambda.Compile();
        return func(stats);
    }
}