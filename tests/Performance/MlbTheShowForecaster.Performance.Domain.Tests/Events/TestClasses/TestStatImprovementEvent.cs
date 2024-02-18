using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Events.TestClasses;

/// <summary>
/// A test stat improvement event
/// </summary>
public sealed record TestStatImprovementEvent(StatSnapshot CurrentStat, StatSnapshot PreviousStat)
    : StatImprovementEvent(CurrentStat, PreviousStat)
{
}