using Microsoft.EntityFrameworkCore;
using ProvaPub.Models.Base;

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
}
