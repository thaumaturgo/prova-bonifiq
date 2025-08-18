using Moq;
using ProvaPub.Dtos;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services.PurchasesRules;
using Xunit;
using static ProvaPub.Services.Interfaces.IPurchaseHandler;

namespace ProvaPub.Tests.PurchaseRules
{
    public class FirstPurchaseMaxLimitHandlerTests
    {
        private static PurchaseContext Ctx(int customerId, decimal value) =>
        new(new PurchaseRequest(customerId, value), DateTimeOffset.UtcNow);
        [Fact]
        public async Task InvalidValue_Fails_WithoutRepoCall()
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            var handler = new FirstPurchaseMaxLimitHandler(repo.Object);

            var res = await handler.HandleAsync(Ctx(1, 0m), default);

            Assert.False(res.Allowed);
            Assert.Contains("invalid_value", res.Reasons);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task FirstPurchase_ValueUpTo100_Ok()
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo.Setup(r => r.CustomerHasOrdersAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var handler = new FirstPurchaseMaxLimitHandler(repo.Object);
            var res = await handler.HandleAsync(Ctx(2, 100m), default);

            Assert.True(res.Allowed);
            Assert.Empty(res.Reasons);
            repo.VerifyAll();
        }

        [Fact]
        public async Task FirstPurchase_Over100_Fails()
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo.Setup(r => r.CustomerHasOrdersAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var handler = new FirstPurchaseMaxLimitHandler(repo.Object);
            var res = await handler.HandleAsync(Ctx(3, 100.01m), default);

            Assert.False(res.Allowed);
            Assert.Contains("first_purchase_over_limit", res.Reasons);
            repo.VerifyAll();
        }

        [Fact]
        public async Task NotFirstPurchase_Over100_Ok()
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo.Setup(r => r.CustomerHasOrdersAsync(4, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var handler = new FirstPurchaseMaxLimitHandler(repo.Object);
            var res = await handler.HandleAsync(Ctx(4, 500m), default);

            Assert.True(res.Allowed);
            Assert.Empty(res.Reasons);
            repo.VerifyAll();
        }

        [Fact]
        public async Task PassesCancellationToken_ToRepository()
        {
            var token = new CancellationTokenSource().Token;
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo.Setup(r => r.CustomerHasOrdersAsync(5, It.Is<CancellationToken>(t => t == token))).ReturnsAsync(true);

            var handler = new FirstPurchaseMaxLimitHandler(repo.Object);
            var res = await handler.HandleAsync(Ctx(5, 10m), token);

            Assert.True(res.Allowed);
            repo.VerifyAll();
        }

        [Fact]
        public void Order_Is_30()
        {
            var handler = new FirstPurchaseMaxLimitHandler(Mock.Of<ICustomerRepository>());
            Assert.Equal(30, handler.Order);
        }
    }
}
