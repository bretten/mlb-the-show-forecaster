using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Common.ValueObjects;

public class MlbIdTests
{
    [Fact]
    public void Create_InvalidId_ThrowsException()
    {
        // Arrange
        var action = () => MlbId.Create(0);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMlbIdException>(actual);
    }

    [Fact]
    public void Create_ValidId_Created()
    {
        // Arrange
        var id = 1;

        // Act
        var actual = MlbId.Create(id);

        // Assert
        Assert.Equal(1, actual.Value);
    }
}