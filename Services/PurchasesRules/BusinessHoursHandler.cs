using ProvaPub.Dtos;
using ProvaPub.Services.Helpers;
using static ProvaPub.Services.Interfaces.IPurchaseHandler;

namespace ProvaPub.Services.PurchasesRules;

public sealed class BusinessHoursHandler : PurchaseHandlerBase
{
 
    public override int Order => 40;
    protected override Task<CanPurchaseResult> ValidateAsync(PurchaseContext c, CancellationToken ct)
    {
        var tz = TimeHelper.SaoPauloTimeZone;
        var nowLocal = TimeZoneInfo.ConvertTime(c.NowUtc, tz);

        var isWorkday = nowLocal.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday);
        var inBusinessHours = nowLocal.Hour >= 8 && nowLocal.Hour <= 18;

        return Task.FromResult(isWorkday && inBusinessHours
            ? CanPurchaseResult.Ok()
            : CanPurchaseResult.Fail("outside_business_hours"));
    }
}
