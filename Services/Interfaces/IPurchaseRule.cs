using ProvaPub.Dtos;
using ProvaPub.Repository;

namespace ProvaPub.Services.Interfaces;

public interface IPurchaseRule
{
    Task<CanPurchaseResult> EvaluateAsync(
        TestDbContext ctx,
        PurchaseRequest req,
        DateTimeOffset nowUtc,
        CancellationToken ct);
}
public interface IPurchasePolicy
{
    Task<CanPurchaseResult> CanPurchaseDetailedAsync(PurchaseRequest request, CancellationToken ct = default); 
}
 
public interface IPurchaseHandler
{
    int Order { get; }                         
    IPurchaseHandler SetNext(IPurchaseHandler next);
    Task<CanPurchaseResult> HandleAsync(PurchaseContext ctx, CancellationToken ct = default);

    public sealed record PurchaseContext( PurchaseRequest Request, DateTimeOffset NowUtc); 
}
