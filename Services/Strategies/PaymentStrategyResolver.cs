using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services.Strategies;

public sealed class PaymentStrategyResolver : IPaymentStrategyResolver
{
    private readonly IReadOnlyDictionary<string, IPaymentStrategy> _map;

    public PaymentStrategyResolver(IEnumerable<IPaymentStrategy> strategies)
    {
        _map = strategies.ToDictionary(s => s.Method, s => s, StringComparer.OrdinalIgnoreCase);
    }

    public IPaymentStrategy Resolve(string method)
    {
        if (string.IsNullOrWhiteSpace(method))
            throw new ArgumentNullException(nameof(method));

        if (_map.TryGetValue(method, out var strat))
            return strat;

        throw new NotSupportedException($"Payment method '{method}' is not supported.");
    }
}