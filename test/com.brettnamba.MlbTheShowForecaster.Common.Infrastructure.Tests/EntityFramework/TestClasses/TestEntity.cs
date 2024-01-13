namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFramework.TestClasses;

public class TestEntity
{
    private TestEntity(int integerValue, string stringValue)
    {
        IntegerValue = integerValue;
        StringValue = stringValue;
    }

    public int IntegerValue { get; private set; }

    public string StringValue { get; private set; }

    public static TestEntity Create(int integerValue, string stringValue)
    {
        return new TestEntity(integerValue, stringValue);
    }
}