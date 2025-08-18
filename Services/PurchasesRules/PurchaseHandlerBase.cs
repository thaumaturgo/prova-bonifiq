using ProvaPub.Dtos;
using ProvaPub.Services.Interfaces;
using static ProvaPub.Services.Interfaces.IPurchaseHandler;

namespace ProvaPub.Services.PurchasesRules;

public abstract class PurchaseHandlerBase : IPurchaseHandler
{
    private IPurchaseHandler? _next;
    public virtual int Order => 0;

    public IPurchaseHandler SetNext(IPurchaseHandler next) { _next = next; return next; }

    public async Task<CanPurchaseResult> HandleAsync(PurchaseContext ctx, CancellationToken ct = default)
    {
        var res = await ValidateAsync(ctx, ct);
        if (!res.Allowed) return res;
        return _next is null ? CanPurchaseResult.Ok() : await _next.HandleAsync(ctx, ct);
    }

    protected abstract Task<CanPurchaseResult> ValidateAsync(PurchaseContext ctx, CancellationToken ct);
}