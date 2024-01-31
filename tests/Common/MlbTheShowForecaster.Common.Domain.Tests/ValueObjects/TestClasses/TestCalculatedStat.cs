using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects.TestClasses;

/// <summary>
/// Test implementation of <see cref="CalculatedStat"/>
/// </summary>
public sealed class TestCalculatedStat : CalculatedStat
{
    public int Component1 { get; }

    public int Component2 { get; }

    public int Component3 { get; }

    private TestCalculatedStat(int component1, int component2, int component3)
    {
        Component1 = component1;
        Component2 = component2;
        Component3 = component3;
    }

    public static TestCalculatedStat Create(int component1, int component2, int component3)
    {
        return new TestCalculatedStat(component1, component2, component3);
    }

    protected override int FractionalDigitCount => 2;

    protected override decimal Calculate()
    {
        return ((decimal)Component1 / Component2) + Component3;
    }
}