﻿using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Core.Tests.SeedWork.TestClasses;

/// <summary>
/// Anemic entity for testing purposes only
/// </summary>
public sealed class TestEntity2 : Entity
{
    private TestEntity2(Guid guid, int integerValue, string stringValue) : base(guid)
    {
        IntegerValue = integerValue;
        StringValue = stringValue;
    }

    public int IntegerValue { get; private set; }

    public string StringValue { get; private set; }

    public static TestEntity2 Create(Guid guid, int integerValue, string stringValue)
    {
        return new TestEntity2(guid, integerValue, stringValue);
    }
}