using ProvaPub.Models.Base;

namespace ProvaPub.Models.Base;

public sealed record PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public int TotalCount { get; init; }
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
     
    public static PagedResult<T> Create(IEnumerable<T> items, int totalCount, PagedRequest request)
        => new()
        {
            Items = items?.ToList() ?? new List<T>(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
}
