namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Mapping.AutoMapper.TestClasses;

public sealed class ObjectTypeA
{
    public ObjectTypeA(int anInt, string aString)
    {
        AnInt = anInt;
        AString = aString;
    }

    public int AnInt { get; private set; }

    public string AString { get; private set; }
}