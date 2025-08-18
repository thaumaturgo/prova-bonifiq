namespace ProvaPub.Dtos;

public sealed record CanPurchaseResult(bool Allowed, string[] Reasons)
{
    public static CanPurchaseResult Ok() => new(true, Array.Empty<string>());
    public static CanPurchaseResult Fail(params string[] reasons)
        => new(false, reasons?.Where(r => !string.IsNullOrWhiteSpace(r)).ToArray() ?? Array.Empty<string>());

    public static CanPurchaseResult Merge(params CanPurchaseResult[] results)
    {
        var all = results?.Where(r => r is not null && !r.Allowed)
                          .SelectMany(r => r.Reasons)
                          .ToArray() ?? Array.Empty<string>();
        return all.Length == 0 ? Ok() : Fail(all);
    }
}