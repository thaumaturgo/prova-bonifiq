using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Models.Base;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services.Extensions;

namespace ProvaPub.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly TestDbContext _ctx;
        public ProductRepository(TestDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<PagedResult<Product>> ListPagedProducts(PagedRequest request, CancellationToken ct = default)
        {
            var result = await _ctx.Products
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .ToPagedResultAsync(request, ct);
            return result;
        }
    }
}
