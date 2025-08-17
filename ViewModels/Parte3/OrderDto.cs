using ProvaPub.Models;

namespace ProvaPub.ViewModels.Parte3;

public sealed record OrderDto(
 int CustomerId,
 decimal PaymentValue,
 DateTimeOffset OrderDateUtc  
)
{
  
    public DateTimeOffset OrderDateLocal => OrderDateUtc.ToLocalTime();


    public static OrderDto FromEntity(Order order)
        => new(
            CustomerId: order.CustomerId,
            PaymentValue: order.Value,
            OrderDateUtc: order.OrderDate
        );
     
}
