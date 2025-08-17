using ProvaPub.Models;
using ProvaPub.Models.Base;

namespace ProvaPub.Services.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<Customer>> ListCustomers(PagedRequest request, CancellationToken ct = default);
    Task<bool> CanPurchase(int customerId, decimal purchaseValue, CancellationToken ct = default);
}