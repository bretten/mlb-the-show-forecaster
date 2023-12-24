using System.ComponentModel;
using com.brettnamba.MlbTheShowForecaster.Common.Converters.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Common.Tests.Converters.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Common.Tests.Converters;

public class EnumDisplayNameConverterTests
{
    [Fact]
    public void ConvertFrom_StringThatMatchesDisplayAttribute_ReturnsCorrespondingEnumMember()
    {
        // Arrange
        var converter = TypeDescriptor.GetConverter(typeof(TestDisplayNameEnum));
        var enumDisplayName = "Member B";

        // Act
        var actual = converter.ConvertFrom(enumDisplayName);

        // Assert
        Assert.Equal(actual, TestDisplayNameEnum.MemberB);
    }

    [Fact]
    public void ConvertFrom_StringThatHasNoMatchingDisplayAttribute_ThrowsException()
    {
        // Arrange
        var converter = TypeDescriptor.GetConverter(typeof(TestDisplayNameEnum));
        var enumDisplayName = "Member D";
        var action = () => converter.ConvertFrom(enumDisplayName);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<CannotConvertByDisplayNameException>(actual);
    }

    [Fact]
    public void ConvertFrom_NonString_ThrowsException()
    {
        // Arrange
        var converter = TypeDescriptor.GetConverter(typeof(TestDisplayNameEnum));
        var nonString = 1;
        var action = () => converter.ConvertFrom(nonString);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidEnumDisplayNameConvertFromArgumentException>(actual);
    }
}