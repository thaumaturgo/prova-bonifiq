using Microsoft.EntityFrameworkCore;
using ProvaPub.Dtos;
using ProvaPub.Repository.Interfaces;
using static ProvaPub.Services.Interfaces.IPurchaseHandler;

namespace ProvaPub.Services.PurchasesRules;

public sealed class FirstPurchaseMaxLimitHandler : PurchaseHandlerBase
{
    public override int Order => 30;

    private readonly ICustomerRepository _customerRepository;

    public FirstPurchaseMaxLimitHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    protected override async Task<CanPurchaseResult> ValidateAsync(PurchaseContext c, CancellationToken ct)
    {
        if (c.Request.Value <= 0m) return CanPurchaseResult.Fail("invalid_value");

        var hasAnyOrder = await _customerRepository.CustomerHasOrdersAsync(c.Request.CustomerId, ct);

        if (!hasAnyOrder && c.Request.Value > 100m)
            return CanPurchaseResult.Fail("first_purchase_over_limit");

        return CanPurchaseResult.Ok();
    }
}
