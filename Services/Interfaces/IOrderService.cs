using ProvaPub.Dtos;
using ProvaPub.Models;

namespace ProvaPub.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> PayOrder(PayOrderDto dto, CancellationToken ct = default);
    }
}
