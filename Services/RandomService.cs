using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services;

public class RandomService : IRandomService
{
    private readonly TestDbContext _ctx;
    public RandomService(TestDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<int?> GetRandom(CancellationToken ct = default)
    {
        const int max = 100;

        var existing = await _ctx.Numbers
            .AsNoTracking()
            .Select(x => x.Number)
            .ToListAsync(ct);

        if (existing.Count >= max) return null;

        var missing = Enumerable.Range(0, max)
            .Except(existing)
            .OrderBy(_ => Random.Shared.Next())
            .First();

        _ctx.Numbers.Add(new RandomNumber { Number = missing });
        await _ctx.SaveChangesAsync(ct);
        return missing;
    } 
}
