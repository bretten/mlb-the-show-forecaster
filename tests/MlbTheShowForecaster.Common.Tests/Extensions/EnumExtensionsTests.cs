using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Common.Tests.Extensions.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Common.Tests.Extensions;

public class EnumExtensionsTests
{
    [Fact]
    public void GetDisplayName_EnumWithoutDisplayAttribute_ThrowsException()
    {
        // Arrange
        var enumMember = TestGetDisplayNameEnum.EnumWithoutDisplayAttribute;
        var action = () => enumMember.GetDisplayName();

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<EnumDisplayNameNotFoundException>(actual);
    }

    [Fact]
    public void GetDisplayName_EnumWithDisplayAttribute_ThrowsException()
    {
        // Arrange
        var enumMember = TestGetDisplayNameEnum.EnumWithDisplayAttribute;
        var action = () => enumMember.GetDisplayName();

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<UndefinedEnumDisplayNameException>(actual);
    }

    [Fact]
    public void GetDisplayName_EnumWithDisplayAttributeAndName_ReturnsName()
    {
        // Arrange
        var enumMember = TestGetDisplayNameEnum.EnumWithDisplayAttributeAndName;

        // Act
        var actual = enumMember.GetDisplayName();

        // Assert
        Assert.Equal("Has DisplayAttribute and name", actual);
    }
}