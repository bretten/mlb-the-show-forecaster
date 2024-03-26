using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Tests;

public class MlbTheShowApiFactoryTests
{
    [Theory]
    [InlineData(2021, Constants.BaseUrl2021)]
    [InlineData(2022, Constants.BaseUrl2022)]
    [InlineData(2023, Constants.BaseUrl2023)]
    [InlineData(2024, Constants.BaseUrl2024)]
    public void GetClient_Year_ReturnsClientForSpecifiedYear(int year, string expectedBaseUrl)
    {
        // Arrange
        var factory = new MlbTheShowApiFactory();

        // Act
        var actual = factory.GetClient((Year)year);

        // Assert
        var actualUnderlyingHttpClient = actual.GetType().GetMethod("get_Client")!.Invoke(actual, null) as HttpClient;
        var actualBaseUrl = actualUnderlyingHttpClient!.BaseAddress!.OriginalString;
        Assert.Equal(expectedBaseUrl, actualBaseUrl);
    }

    [Fact]
    public void GetClient_UnsupportedYear_ThrowsException()
    {
        // Arrange
        var factory = new MlbTheShowApiFactory();
        var action = () => factory.GetClient((Year)2020);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<UnsupportedMlbTheShowYearException>(actual);
    }
}