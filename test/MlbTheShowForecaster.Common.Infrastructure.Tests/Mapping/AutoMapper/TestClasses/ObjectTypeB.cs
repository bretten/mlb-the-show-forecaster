namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Mapping.AutoMapper.TestClasses;

public sealed class ObjectTypeB
{
    public ObjectTypeB(int integerValue, string stringValue)
    {
        IntegerValue = integerValue;
        StringValue = stringValue;
    }

    public int IntegerValue { get; private set; }

    public string StringValue { get; private set; }
}