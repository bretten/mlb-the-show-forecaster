using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos;

public class AttributeChangeTests
{
    [Fact]
    public void Increased_PositiveChangeAmount_ReturnsTrue()
    {
        // Arrange
        var attributeChange = Faker.FakeAttributeChange(newValue: 10, changeAmount: 1);

        // Act
        var actual = attributeChange.Increased;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Increased_NegativeChangeAmount_ReturnsFalse()
    {
        // Arrange
        var attributeChange = Faker.FakeAttributeChange(newValue: 10, changeAmount: -1);

        // Act
        var actual = attributeChange.Increased;

        // Assert
        Assert.False(actual);
    }
}