using Moq;
using ProvaPub.Dtos;
using ProvaPub.Services;
using ProvaPub.Services.Interfaces;
using Xunit;
using static ProvaPub.Services.Interfaces.IPurchaseHandler;

namespace ProvaPub.Tests;

public class PurchasePolicyChainTests
{
    [Fact]
    public async Task CanPurchase_WhenNoHandlers_ReturnsOk()
    {
        var chain = new PurchasePolicyChain(Array.Empty<IPurchaseHandler>());
        var req = new PurchaseRequest(1, 50m);
        var res = await chain.CanPurchaseDetailedAsync(req);
        Assert.True(res.Allowed);
        Assert.NotNull(res.Reasons);
        Assert.Empty(res.Reasons);
    }

    [Fact]
    public async Task CanPurchase_ExecutesHandlersByOrder_AndAggregatesReasons()
    {
        var calls = new List<string>();

        var h2 = NewHandlerMock(20, (c, _) =>
        {
            calls.Add("h2");
            return Task.FromResult(CanPurchaseResult.Fail("r2"));
        });

        var h1 = NewHandlerMock(10, (c, _) =>
        {
            calls.Add("h1");
            return Task.FromResult(CanPurchaseResult.Ok());
        });

        var h3 = NewHandlerMock(30, (c, _) =>
        {
            calls.Add("h3");
            return Task.FromResult(CanPurchaseResult.Fail("r3a", "r3b"));
        });

        var chain = new PurchasePolicyChain(new[] { h2.Object, h1.Object, h3.Object });
        var res = await chain.CanPurchaseDetailedAsync(new PurchaseRequest(7, 150m));

        Assert.Equal(new[] { "h1", "h2", "h3" }, calls);
        Assert.False(res.Allowed);
        Assert.Contains("r2", res.Reasons);
        Assert.Contains("r3a", res.Reasons);
        Assert.Contains("r3b", res.Reasons);
    }

    [Fact]
    public async Task CanPurchase_PassesSameNowUtc_ToAllHandlers()
    {
        var seen = new List<DateTimeOffset>();

        var h1 = NewHandlerMock(1, (c, _) =>
        {
            seen.Add(c.NowUtc);
            return Task.FromResult(CanPurchaseResult.Ok());
        });

        var h2 = NewHandlerMock(2, (c, _) =>
        {
            seen.Add(c.NowUtc);
            return Task.FromResult(CanPurchaseResult.Ok());
        });

        var chain = new PurchasePolicyChain(new[] { h1.Object, h2.Object });
        var res = await chain.CanPurchaseDetailedAsync(new PurchaseRequest(1, 10m));

        Assert.True(res.Allowed);
        Assert.Equal(2, seen.Count);
        Assert.Equal(seen[0], seen[1]);
    }

    [Fact]
    public async Task CanPurchase_PassesRequest_ToHandlers()
    {
        PurchaseRequest? seenReq = null;

        var h = NewHandlerMock(1, (c, _) =>
        {
            seenReq = c.Request;
            return Task.FromResult(CanPurchaseResult.Ok());
        });

        var chain = new PurchasePolicyChain(new[] { h.Object });
        var req = new PurchaseRequest(42, 99.9m);
        var res = await chain.CanPurchaseDetailedAsync(req);

        Assert.True(res.Allowed);
        Assert.Equal(req, seenReq);
    }

    [Fact]
    public async Task CanPurchase_PassesCancellationToken_ToHandlers()
    {
        var seen = new List<CancellationToken>();

        var h1 = NewHandlerMock(1, (c, t) =>
        {
            seen.Add(t);
            return Task.FromResult(CanPurchaseResult.Ok());
        });

        var h2 = NewHandlerMock(2, (c, t) =>
        {
            seen.Add(t);
            return Task.FromResult(CanPurchaseResult.Ok());
        });

        var chain = new PurchasePolicyChain(new[] { h1.Object, h2.Object });
        using var cts = new CancellationTokenSource();
        var res = await chain.CanPurchaseDetailedAsync(new PurchaseRequest(3, 12m), cts.Token);

        Assert.True(res.Allowed);
        Assert.Equal(2, seen.Count);
        Assert.Equal(cts.Token, seen[0]);
        Assert.Equal(cts.Token, seen[1]);
    }

    [Fact]
    public async Task CanPurchase_AllOkHandlers_ReturnsOkWithEmptyReasons()
    {
        var ok1 = NewHandlerMock(10, (c, _) => Task.FromResult(CanPurchaseResult.Ok()));
        var ok2 = NewHandlerMock(20, (c, _) => Task.FromResult(CanPurchaseResult.Ok()));

        var chain = new PurchasePolicyChain(new[] { ok1.Object, ok2.Object });
        var res = await chain.CanPurchaseDetailedAsync(new PurchaseRequest(5, 25m));

        Assert.True(res.Allowed);
        Assert.NotNull(res.Reasons);
        Assert.Empty(res.Reasons);
    }

    private static Mock<IPurchaseHandler> NewHandlerMock(
        int order,
        Func<PurchaseContext, CancellationToken, Task<CanPurchaseResult>> impl)
    {
        var m = new Mock<IPurchaseHandler>(MockBehavior.Strict);
        m.SetupGet(h => h.Order).Returns(order);
        m.Setup(h => h.HandleAsync(It.IsAny<PurchaseContext>(), It.IsAny<CancellationToken>()))
         .Returns<PurchaseContext, CancellationToken>(impl);
        return m;
    }
}
