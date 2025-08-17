using ProvaPub.Models;
using ProvaPub.Models.Base;

namespace ProvaPub.Services.Interfaces;

public interface IProductService
{
    Task<PagedResult<Product>> ListProducts(PagedRequest request, CancellationToken ct = default);
}
