using ProvaPub.Dtos;
using ProvaPub.Repository;
using ProvaPub.Services.Interfaces;
using static ProvaPub.Services.Interfaces.IPurchaseHandler;

namespace ProvaPub.Services;

public sealed class PurchasePolicyChain : IPurchasePolicy
{
    private readonly DateTimeOffset _clock = DateTimeOffset.UtcNow;
    private readonly IEnumerable<IPurchaseHandler> _handlers;
    public PurchasePolicyChain(IEnumerable<IPurchaseHandler> handlers)
    {
        _handlers = handlers;
    }

    public async Task<CanPurchaseResult> CanPurchaseDetailedAsync(PurchaseRequest request, CancellationToken ct = default)
    {
        var ctx = new PurchaseContext(request, _clock);
        var results = new List<CanPurchaseResult>();

        foreach (var h in _handlers.OrderBy(x => x.Order))
            results.Add(await h.HandleAsync(ctx, ct));

        return CanPurchaseResult.Merge(results.ToArray());
    }
}

