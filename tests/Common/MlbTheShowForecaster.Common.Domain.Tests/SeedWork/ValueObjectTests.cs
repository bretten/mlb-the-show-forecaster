using com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.SeedWork.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.SeedWork;

public class ValueObjectTests
{
    [Fact]
    public void Equals_NullComparison_ReturnsFalse()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        TestValueObject1? vo2 = null;

        // Act
        var actual = vo1.Equals(vo2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        var vo2 = TestValueObject2.Create(1, "a");

        // Act
        var actual = vo1.Equals(vo2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        var vo2 = TestValueObject1.Create(1, "a");

        // Act
        var actual = vo1.Equals(vo2);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        var vo2 = TestValueObject1.Create(2, "b");

        // Act
        var actual = vo1.Equals(vo2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_BoxedNullComparison_ReturnsFalse()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        object? vo2 = null;

        // Act
        var actual = vo1.Equals(vo2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_BoxedDifferentType_ReturnsFalse()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        object vo2 = TestValueObject2.Create(1, "a");

        // Act
        var actual = vo1.Equals(vo2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_BoxedSameValues_ReturnsTrue()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        object vo2 = TestValueObject1.Create(1, "a");

        // Act
        var actual = vo1.Equals(vo2);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_BoxedDifferentValues_ReturnsFalse()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        object vo2 = TestValueObject1.Create(2, "b");

        // Act
        var actual = vo1.Equals(vo2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void EqualsOperator_SameValues_ReturnsTrue()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        var vo2 = TestValueObject1.Create(1, "a");

        // Act
        var actual = vo1 == vo2;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void EqualsOperator_DifferentValues_ReturnsFalse()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        var vo2 = TestValueObject1.Create(2, "b");

        // Act
        var actual = vo1 == vo2;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void NotEqualsOperator_SameValues_ReturnsFalse()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        var vo2 = TestValueObject1.Create(1, "a");

        // Act
        var actual = vo1 != vo2;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void NotEqualsOperator_DifferentValues_ReturnsTrue()
    {
        // Assemble
        var vo1 = TestValueObject1.Create(1, "a");
        var vo2 = TestValueObject1.Create(2, "b");

        // Act
        var actual = vo1 != vo2;

        // Assert
        Assert.True(actual);
    }
}