using ProvaPub.Dtos;
using ProvaPub.Models;
using ProvaPub.Models.Base;

namespace ProvaPub.Services.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<CustomerDto>> ListCustomers(PagedRequest request, CancellationToken ct = default);
    Task<CanPurchaseResult> CanPurchase(PurchaseRequest request, CancellationToken ct = default);
}