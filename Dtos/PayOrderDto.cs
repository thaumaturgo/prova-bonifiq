namespace ProvaPub.Dtos
{
    public sealed record PayOrderDto(string PaymentMethod, decimal PaymentValue, int CustomerId);
}
