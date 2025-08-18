using ProvaPub.Dtos;
using ProvaPub.Models.Base;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services.Extensions;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<ProductDto>> ListProducts(PagedRequest request, CancellationToken ct = default)
    {
        var page = await _productRepository.ListPagedProducts(request, ct);
        return page.SelectItems(p => new ProductDto(p.Id, p.Name));
    }
}