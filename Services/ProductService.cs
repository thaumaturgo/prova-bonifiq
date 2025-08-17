using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Models.Base;
using ProvaPub.Repository;
using ProvaPub.Services.Extensions;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services
{
    public class ProductService : IProductService
    {
        private readonly TestDbContext _ctx;

        public ProductService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagedResult<Product>> ListProducts(PagedRequest request, CancellationToken ct = default)
        { 
            var result = await  _ctx.Products
                          .AsNoTracking()
                          .OrderBy(p => p.Id)  
                          .ToPagedResultAsync(request, ct);

            return result;
        }
    }

}