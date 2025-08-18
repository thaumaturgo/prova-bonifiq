using ProvaPub.Dtos;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services.Helpers;
using static ProvaPub.Services.Interfaces.IPurchaseHandler;

namespace ProvaPub.Services.PurchasesRules;

public sealed class OneOrderPerMonthHandler : PurchaseHandlerBase
{
    public override int Order => 20;

    private readonly ICustomerRepository _customerRepository;
    public OneOrderPerMonthHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    protected override async Task<CanPurchaseResult> ValidateAsync(PurchaseContext c, CancellationToken ct)
    {
        var timeZone = TimeHelper.SaoPauloTimeZone;
        var (startUtc, endUtc) = TimeHelper.GetMonthBoundsUtc(c.NowUtc, timeZone);

        var hasOrderThisMonth = await _customerRepository
            .CustomerHasOrdersOnMonthAsync(c.Request.CustomerId, startUtc, endUtc, ct);

        return hasOrderThisMonth ? CanPurchaseResult.Fail("monthly_limit_reached") : CanPurchaseResult.Ok();
    }
}
