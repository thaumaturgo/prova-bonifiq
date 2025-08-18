using Microsoft.EntityFrameworkCore;
using ProvaPub.Models.Base;
using System.Linq.Expressions;

namespace ProvaPub.Services.Extensions;

public static class PagingExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PagedRequest request,
        CancellationToken ct = default)
    {
        var r = (request ?? new PagedRequest()).Normalize();

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((r.Page - 1) * r.PageSize)
            .Take(r.PageSize)
            .ToListAsync(ct);

        return PagedResult<T>.Create(items, totalCount, r);
    }

    public static PagedResult<TOut> SelectItems<TIn, TOut>(
         this PagedResult<TIn> source,
         Func<TIn, TOut> selector)
         => new PagedResult<TOut>
         {
             Items = (source.Items ?? Array.Empty<TIn>()).Select(selector).ToList(),
             Page = source.Page,
             PageSize = source.PageSize,
             TotalCount = source.TotalCount
         };
}
