using ProvaPub.Repository;
using ProvaPub.Services.Interfaces;
using ProvaPub.ViewModels.Parte3;

namespace ProvaPub.Services;

public class OrderService : IOrderService
{
    private readonly TestDbContext _ctx;
    private readonly IPaymentStrategyResolver _resolver;

    public OrderService(TestDbContext ctx, IPaymentStrategyResolver resolver)
    {
        _ctx = ctx;
        _resolver = resolver;
    }

    public async Task<OrderDto> PayOrder(PayOrderDto dto, CancellationToken ct = default)
    {
        var strategy = _resolver.Resolve(dto.PaymentMethod);

        var order = await strategy.PayAsync(dto.PaymentValue, dto.CustomerId);

        await _ctx.Orders.AddAsync(order, ct);

        await _ctx.SaveChangesAsync(ct);

        return OrderDto.FromEntity(order);
    }

}
