using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;

/// <summary>
/// Represents the criteria that an individual stat will be assessed by in <see cref="MinMaxNormalizationPerformanceAssessor"/>
/// </summary>
public abstract record MinMaxStatCriteria
{
    /// <summary>
    /// The name of the stat
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Weight that determines the degree of impact on the assessment score
    /// </summary>
    public decimal Weight { get; }

    /// <summary>
    /// True if better performance in this stat means a lower value
    /// </summary>
    public bool IsLowerValueBetter { get; }

    /// <summary>
    /// For min-max normalization, the minimum value of the stat. Will be based off of historical MLB stats
    /// </summary>
    public decimal Min { get; }

    /// <summary>
    /// For min-max normalization, the maximum value of the stat. Will be based off of historical MLB stats
    /// </summary>
    public decimal Max { get; }

    /// <summary>
    /// The result of <see cref="Max"/> minus <see cref="Min"/>
    /// </summary>
    public decimal MaxMinusMin => Max - Min;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name">The name of the stat</param>
    /// <param name="weight">Weight that determines the degree of impact on the assessment score</param>
    /// <param name="isLowerValueBetter">True if better performance in this stat means a lower value</param>
    /// <param name="min">For min-max normalization, the minimum value of the stat. Will be based off of historical MLB stats</param>
    /// <param name="max">For min-max normalization, the maximum value of the stat. Will be based off of historical MLB stats</param>
    /// <exception cref="InvalidMinMaxStatCriteriaException">Thrown if the weights or min/max values are invalid</exception>
    protected MinMaxStatCriteria(string name, decimal weight, bool isLowerValueBetter, decimal min, decimal max)
    {
        if (weight < 0 || weight > 1)
        {
            throw new InvalidMinMaxStatCriteriaException("Weight must be between 0 and 1");
        }

        if (min >= max)
        {
            throw new InvalidMinMaxStatCriteriaException("Min cannot be larger than max");
        }

        Name = name;
        Weight = weight;
        IsLowerValueBetter = isLowerValueBetter;
        Min = min;
        Max = max;
    }
}