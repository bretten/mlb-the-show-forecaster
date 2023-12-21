using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Core.Tests.SeedWork.TestClasses;

/// <summary>
/// Anemic entity for testing purposes only
/// </summary>
public sealed class TestEntity1 : Entity
{
    private TestEntity1(Guid guid, int integerValue, string stringValue) : base(guid)
    {
        IntegerValue = integerValue;
        StringValue = stringValue;
    }

    public int IntegerValue { get; private set; }

    public string StringValue { get; private set; }

    public static TestEntity1 Create(Guid guid, int integerValue, string stringValue)
    {
        return new TestEntity1(guid, integerValue, stringValue);
    }
}