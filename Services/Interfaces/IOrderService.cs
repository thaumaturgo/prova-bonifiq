using ProvaPub.Models;
using ProvaPub.ViewModels.Parte3;

namespace ProvaPub.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> PayOrder(PayOrderDto dto, CancellationToken ct = default);
    }
}
