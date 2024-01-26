using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.ValueObjects;

public class SeasonYearTests
{
    [Fact]
    public void Create_InvalidYear_ThrowsException()
    {
        // Arrange
        const ushort year = 1800;
        var action = () => SeasonYear.Create(year);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMlbSeasonYearException>(actual);
    }

    [Fact]
    public void Create_ValidYear_Created()
    {
        // Arrange
        const ushort year = 2024;

        // Act
        var actual = SeasonYear.Create(year);

        // Assert
        Assert.Equal(2024, actual.Value);
    }
}