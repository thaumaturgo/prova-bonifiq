using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Models.Base;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services.Extensions;

namespace ProvaPub.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly TestDbContext _ctx;

    public CustomerRepository(TestDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<bool> ExistsAsync(int customerId, CancellationToken ct = default)
    {
        return await _ctx.Customers
                         .AsNoTracking()
                         .AnyAsync(c => c.Id == customerId, ct);
    }

    public async Task<bool> CustomerHasOrdersAsync(int customerId, CancellationToken ct = default) =>
     await _ctx.Orders.AsNoTracking().AnyAsync(o => o.CustomerId == customerId, ct);


    public async Task<PagedResult<Customer>> ListPagedCustomers(PagedRequest request, CancellationToken ct = default)
    {
        var result = await _ctx.Customers
                         .AsNoTracking()
                         .OrderBy(p => p.Id)
                         .ToPagedResultAsync(request, ct);

        return result;
    }

    public async Task<bool> CustomerHasOrdersOnMonthAsync(
        int customerId, DateTimeOffset startUtc, DateTimeOffset endUtc, CancellationToken ct = default)
    {
         return await _ctx.Orders.AsNoTracking()
               .AnyAsync(o => o.CustomerId == customerId &&
                               o.OrderDate >= startUtc && o.OrderDate < endUtc, ct);
    }
}
