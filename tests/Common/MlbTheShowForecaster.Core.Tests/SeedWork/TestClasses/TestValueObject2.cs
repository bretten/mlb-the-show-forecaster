using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Core.Tests.SeedWork.TestClasses;

/// <summary>
/// For testing
/// </summary>
public sealed class TestValueObject2 : ValueObject
{
    public int IntegerValue { get; }

    public string StringValue { get; }

    private TestValueObject2(int integerValue, string stringValue)
    {
        IntegerValue = integerValue;
        StringValue = stringValue;
    }

    protected override IEnumerable<object?> GetNestedValues()
    {
        yield return IntegerValue;
        yield return StringValue;
    }

    public static TestValueObject2 Create(int integerValue, string stringValue)
    {
        return new TestValueObject2(integerValue, stringValue);
    }
}