using ProvaPub.Models;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services.Strategies;

public sealed class PaypalPaymentStrategy : IPaymentStrategy
{
    public string Method => "paypal";

    public Task<Order> PayAsync(decimal value, int customerId, CancellationToken ct = default)
    {
        var order = new Order
        {
            CustomerId = customerId,
            Value = value,
        };
        return Task.FromResult(order);
    }
}
