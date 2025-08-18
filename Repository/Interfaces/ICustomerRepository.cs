using ProvaPub.Models;
using ProvaPub.Models.Base;

namespace ProvaPub.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<bool> ExistsAsync(int customerId, CancellationToken ct = default);
        Task<PagedResult<Customer>> ListPagedCustomers(PagedRequest request, CancellationToken ct = default);

        Task<bool> CustomerHasOrdersAsync(int customerId, CancellationToken ct = default);
        Task<bool> CustomerHasOrdersOnMonthAsync(int customerId, DateTimeOffset startUtc, DateTimeOffset endUtc, CancellationToken ct = default);
    }
}
