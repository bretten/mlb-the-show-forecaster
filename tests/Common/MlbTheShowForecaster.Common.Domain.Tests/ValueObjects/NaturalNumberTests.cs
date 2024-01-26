using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects;

public class NaturalNumberTests
{
    [Fact]
    public void Create_UnsignedInteger_Created()
    {
        // Arrange
        const uint value = 100;

        // Act
        var actual = NaturalNumber.Create(value);

        // Assert
        Assert.Equal(100U, actual.Value);
    }

    [Fact]
    public void Create_InvalidSignedInteger_ThrowsException()
    {
        // Arrange
        const int value = -1;
        var action = () => NaturalNumber.Create(value);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<NaturalNumberCannotBeLessThanZeroException>(actual);
    }

    [Fact]
    public void Create_SignedInteger_Created()
    {
        // Arrange
        const int value = 100;

        // Act
        var actual = NaturalNumber.Create(value);

        // Assert
        Assert.Equal(100U, actual.Value);
    }
}