using ProvaPub.Dtos;
using ProvaPub.Repository.Interfaces;
using static ProvaPub.Services.Interfaces.IPurchaseHandler;

namespace ProvaPub.Services.PurchasesRules;
public sealed class CustomerMustExistHandler : PurchaseHandlerBase
{
    public override int Order => 10;

    private readonly ICustomerRepository _customerRepository;

    public CustomerMustExistHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    protected override async Task<CanPurchaseResult> ValidateAsync(PurchaseContext c, CancellationToken ct)
    {
        if (c.Request.CustomerId <= 0) return CanPurchaseResult.Fail("invalid_customer_id");
        var exists = await _customerRepository.ExistsAsync(c.Request.CustomerId, ct);   
        return exists ? CanPurchaseResult.Ok() : CanPurchaseResult.Fail("customer_not_found");
    }
}
