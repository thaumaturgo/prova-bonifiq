using ProvaPub.Dtos;
using ProvaPub.Models.Base;

namespace ProvaPub.Services.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDto>> ListProducts(PagedRequest request, CancellationToken ct = default);
}
