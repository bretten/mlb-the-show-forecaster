using com.brettnamba.MlbTheShowForecaster.Core.Tests.SeedWork.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Core.Tests.SeedWork;

public class EntityTests
{
    [Fact]
    public void Equals_NullComparison_ReturnsFalse()
    {
        // Assemble
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var entity = TestEntity1.Create(guid, 1, "a");
        TestEntity1? nullEntity = null;

        // Act
        var actual = entity.Equals(nullEntity);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // Assemble
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var entity1 = TestEntity1.Create(guid, 1, "a");
        var entity2 = TestEntity2.Create(guid, 1, "a");

        // Act
        var actual = entity1.Equals(entity2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_EntityWithSameId_ReturnsTrue()
    {
        // Assemble
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var entity = TestEntity1.Create(guid, 1, "a");
        var mutatedEntity = TestEntity1.Create(guid, 2, "b");

        // Act
        var actual = entity.Equals(mutatedEntity);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_EntityWithDifferentId_ReturnsFalse()
    {
        // Assemble
        var guid1 = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var guid2 = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var entity1 = TestEntity1.Create(guid1, 1, "a");
        var entity2 = TestEntity1.Create(guid2, 1, "a");

        // Act
        var actual = entity1.Equals(entity2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_BoxedNullComparison_ReturnsFalse()
    {
        // Assemble
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var entity = TestEntity1.Create(guid, 1, "a");
        object? nullEntity = null;

        // Act
        var actual = entity.Equals(nullEntity);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_BoxedDifferentType_ReturnsFalse()
    {
        // Assemble
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var entity1 = TestEntity1.Create(guid, 1, "a");
        object entity2 = TestEntity2.Create(guid, 1, "a");

        // Act
        var actual = entity1.Equals(entity2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Equals_BoxedEntityWithSameId_ReturnsTrue()
    {
        // Assemble
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var entity = TestEntity1.Create(guid, 1, "a");
        object mutatedEntity = TestEntity1.Create(guid, 2, "b");

        // Act
        var actual = entity.Equals(mutatedEntity);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_BoxedEntityWithDifferentId_ReturnsFalse()
    {
        // Assemble
        var guid1 = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var guid2 = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var entity1 = TestEntity1.Create(guid1, 1, "a");
        object entity2 = TestEntity1.Create(guid2, 1, "a");

        // Act
        var actual = entity1.Equals(entity2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void EqualsOperator_EntityWithSameId_ReturnsTrue()
    {
        // Assemble
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var entity = TestEntity1.Create(guid, 1, "a");
        var mutatedEntity = TestEntity1.Create(guid, 2, "b");

        // Act
        var actual = entity == mutatedEntity;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void EqualsOperator_EntityWithDifferentId_ReturnsFalse()
    {
        // Assemble
        var guid1 = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var guid2 = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var entity1 = TestEntity1.Create(guid1, 1, "a");
        var entity2 = TestEntity1.Create(guid2, 1, "a");

        // Act
        var actual = entity1 == entity2;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void NotEqualsOperator_EntityWithSameId_ReturnsFalse()
    {
        // Assemble
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var entity = TestEntity1.Create(guid, 1, "a");
        var mutatedEntity = TestEntity1.Create(guid, 2, "b");

        // Act
        var actual = entity != mutatedEntity;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void NotEqualsOperator_EntityWithDifferentId_ReturnsTrue()
    {
        // Assemble
        var guid1 = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var guid2 = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var entity1 = TestEntity1.Create(guid1, 1, "a");
        var entity2 = TestEntity1.Create(guid2, 1, "a");

        // Act
        var actual = entity1 != entity2;

        // Assert
        Assert.True(actual);
    }
}