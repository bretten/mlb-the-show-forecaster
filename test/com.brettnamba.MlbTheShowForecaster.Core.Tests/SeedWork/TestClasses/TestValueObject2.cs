using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Core.Tests.SeedWork.TestClasses;

/// <summary>
/// For testing
/// </summary>
public sealed class TestValueObject2 : ValueObject
{
    public int IntegerValue { get; private set; }

    public string StringValue { get; private set; }

    private TestValueObject2(int integerValue, string stringValue)
    {
        IntegerValue = integerValue;
        StringValue = stringValue;
    }

    public override IEnumerable<object> GetNestedValues()
    {
        yield return IntegerValue;
        yield return StringValue;
    }

    public static TestValueObject2 Create(int integerValue, string stringValue)
    {
        return new TestValueObject2(integerValue, stringValue);
    }
}