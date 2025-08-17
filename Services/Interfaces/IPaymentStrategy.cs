using ProvaPub.Models;

namespace ProvaPub.Services.Interfaces;

public interface IPaymentStrategy
{
    string Method { get; }
    Task<Order> PayAsync(decimal value, int customerId, CancellationToken ct = default);
}
