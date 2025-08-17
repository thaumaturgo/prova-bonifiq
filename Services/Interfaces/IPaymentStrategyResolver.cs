namespace ProvaPub.Services.Interfaces;

public interface IPaymentStrategyResolver
{
    IPaymentStrategy Resolve(string method);
}