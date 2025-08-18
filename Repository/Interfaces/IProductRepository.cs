using ProvaPub.Models;
using ProvaPub.Models.Base;

namespace ProvaPub.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> ListPagedProducts(PagedRequest request, CancellationToken ct = default);
    }
}
