using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFramework.TestClasses;

public class TestEntity : Entity
{
    private TestEntity(int integerValue, string stringValue) : base(new Guid())
    {
        IntegerValue = integerValue;
        StringValue = stringValue;
    }

    public int IntegerValue { get; private set; }

    public string StringValue { get; private set; }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        RaiseDomainEvent(domainEvent);
    }

    public static TestEntity Create(int integerValue, string stringValue)
    {
        return new TestEntity(integerValue, stringValue);
    }
}