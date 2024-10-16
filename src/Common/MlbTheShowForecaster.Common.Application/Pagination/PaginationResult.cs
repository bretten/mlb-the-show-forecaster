namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Pagination;

/// <summary>
/// Represents a paginated result of items of type <see cref="T"/>
/// </summary>
public sealed class PaginationResult<T>
{
    /// <summary>
    /// The current page
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// The number of items on the page
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// The total number of items across all pages
    /// </summary>
    public long TotalItems { get; }

    /// <summary>
    /// The total number of pages
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

    /// <summary>
    /// The items on the current page
    /// </summary>
    public IEnumerable<T> Items { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="page">The current page</param>
    /// <param name="pageSize">The number of items on the page</param>
    /// <param name="totalItems">The total number of items across all pages</param>
    /// <param name="items">The items on the current page</param>
    private PaginationResult(int page, int pageSize, long totalItems, IEnumerable<T> items)
    {
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
        Items = items;
    }

    /// <summary>
    /// Creates <see cref="PaginationResult{T}"/>
    /// </summary>
    /// <param name="page">The current page</param>
    /// <param name="pageSize">The number of items on the page</param>
    /// <param name="totalItems">The total number of items across all pages</param>
    /// <param name="items">The items on the current page</param>
    /// <returns><see cref="PaginationResult{T}"/></returns>
    public static PaginationResult<T> Create(int page, int pageSize, long totalItems, IEnumerable<T> items)
    {
        return new PaginationResult<T>(page: page, pageSize: pageSize, totalItems: totalItems, items);
    }
}