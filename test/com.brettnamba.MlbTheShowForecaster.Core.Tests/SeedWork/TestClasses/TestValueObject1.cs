using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Core.Tests.SeedWork.TestClasses;

/// <summary>
/// For testing
/// </summary>
public sealed class TestValueObject1 : ValueObject
{
    public int IntegerValue { get; private set; }

    public string StringValue { get; private set; }

    private TestValueObject1(int integerValue, string stringValue)
    {
        IntegerValue = integerValue;
        StringValue = stringValue;
    }

    public override IEnumerable<object> GetNestedValues()
    {
        yield return IntegerValue;
        yield return StringValue;
    }

    public static TestValueObject1 Create(int integerValue, string stringValue)
    {
        return new TestValueObject1(integerValue, stringValue);
    }
}