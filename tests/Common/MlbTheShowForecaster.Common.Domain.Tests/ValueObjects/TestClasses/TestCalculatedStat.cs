using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects.TestClasses;

/// <summary>
/// Test implementation of <see cref="CalculatedStat"/>
/// </summary>
public sealed class TestCalculatedStat : CalculatedStat
{
    private TestCalculatedStat(decimal value) : base(value)
    {
    }

    public static TestCalculatedStat Create(int variable1, int variable2, int variable3)
    {
        return new TestCalculatedStat(((decimal)variable1 / variable2) + variable3);
    }

    public static TestCalculatedStat Create(decimal rawValue)
    {
        return new TestCalculatedStat(rawValue);
    }
}