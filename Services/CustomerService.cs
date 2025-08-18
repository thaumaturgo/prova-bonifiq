using ProvaPub.Dtos;
using ProvaPub.Models.Base;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services.Extensions;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services;

public class CustomerService : ICustomerService
{ 
    private readonly IPurchasePolicy _policy; 
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(IPurchasePolicy policy, ICustomerRepository customerRepository)
    {
        _policy = policy;
        _customerRepository = customerRepository;
    }

    public async Task<PagedResult<CustomerDto>> ListCustomers(PagedRequest request, CancellationToken ct = default)
    {
        var page = await _customerRepository.ListPagedCustomers(request, ct);
        return page.SelectItems(c => new CustomerDto(
            c.Id,
            c.Name 
        ));
    }

    public async Task<CanPurchaseResult> CanPurchase(PurchaseRequest request, CancellationToken ct = default) => await _policy.CanPurchaseDetailedAsync(request, ct);

}
