namespace ProvaPub.Services.Interfaces;

public interface IRandomService
{
    Task<int?> GetRandom(CancellationToken ct = default);
}
