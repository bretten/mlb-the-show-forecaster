using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.SeedWork.TestClasses;

/// <summary>
/// For testing
/// </summary>
public sealed class TestValueObject1 : ValueObject
{
    public int IntegerValue { get; }

    public string StringValue { get; }

    private TestValueObject1(int integerValue, string stringValue)
    {
        IntegerValue = integerValue;
        StringValue = stringValue;
    }

    protected override IEnumerable<object?> GetNestedValues()
    {
        yield return IntegerValue;
        yield return StringValue;
    }

    public static TestValueObject1 Create(int integerValue, string stringValue)
    {
        return new TestValueObject1(integerValue, stringValue);
    }
}