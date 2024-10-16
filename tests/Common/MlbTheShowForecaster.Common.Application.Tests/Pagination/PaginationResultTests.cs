using com.brettnamba.MlbTheShowForecaster.Common.Application.Pagination;

namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Tests.Pagination;

public class PaginationResultTests
{
    [Fact]
    public void Create_PageInformation_Created()
    {
        // Arrange
        const int page = 2;
        const int pageSize = 5;
        const long totalItems = 20;
        var items = new List<string>() { "A", "B", "C", "D", "E" };

        // Act
        var actual = PaginationResult<string>.Create(page, pageSize, totalItems, items);

        // Assert
        Assert.Equal(2, actual.Page);
        Assert.Equal(5, actual.PageSize);
        Assert.Equal(items, actual.Items);
        Assert.Equal(totalItems, actual.TotalItems);
        Assert.Equal(4, actual.TotalPages);
    }
}