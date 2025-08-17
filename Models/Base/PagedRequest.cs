namespace ProvaPub.Models.Base;
public sealed record PagedRequest(int Page = 1, int PageSize = 20)
{
    public const int MaxPageSize = 200;
    public PagedRequest Normalize() => this with
    {
        Page = Page < 1 ? 1 : Page,
        PageSize = PageSize < 1 ? 20 : (PageSize > MaxPageSize ? MaxPageSize : PageSize)
    };
}