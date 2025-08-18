using Moq;
using ProvaPub.Dtos;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services.Helpers;
using ProvaPub.Services.PurchasesRules;
using Xunit;
using static ProvaPub.Services.Interfaces.IPurchaseHandler;

namespace ProvaPub.Tests.PurchaseRules
{
    public class OneOrderPerMonthHandlerTests
    {
        private static PurchaseContext Ctx(int customerId, DateTimeOffset nowUtc) =>
        new(new PurchaseRequest(customerId, 10m), nowUtc);

        [Fact]
        public async Task WhenHasOrderThisMonth_FailsWithMonthlyLimitReached()
        {
            var nowUtc = new DateTimeOffset(2025, 8, 17, 12, 34, 00, TimeSpan.Zero);
            var (startUtc, endUtc) = TimeHelper.GetMonthBoundsUtc(nowUtc, TimeHelper.SaoPauloTimeZone);

            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo.Setup(r => r.CustomerHasOrdersOnMonthAsync(
                    7,
                    It.Is<DateTimeOffset>(d => d == startUtc),
                    It.Is<DateTimeOffset>(d => d == endUtc),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new OneOrderPerMonthHandler(repo.Object);
            var res = await handler.HandleAsync(Ctx(7, nowUtc), default);

            Assert.False(res.Allowed);
            Assert.Contains("monthly_limit_reached", res.Reasons);
            repo.VerifyAll();
        }

        [Fact]
        public async Task WhenNoOrderThisMonth_Ok()
        {
            var nowUtc = new DateTimeOffset(2025, 8, 10, 0, 0, 0, TimeSpan.Zero);
            var (startUtc, endUtc) = TimeHelper.GetMonthBoundsUtc(nowUtc, TimeHelper.SaoPauloTimeZone);

            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo.Setup(r => r.CustomerHasOrdersOnMonthAsync(
                    5,
                    It.Is<DateTimeOffset>(d => d == startUtc),
                    It.Is<DateTimeOffset>(d => d == endUtc),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = new OneOrderPerMonthHandler(repo.Object);
            var res = await handler.HandleAsync(Ctx(5, nowUtc), default);

            Assert.True(res.Allowed);
            Assert.Empty(res.Reasons);
            repo.VerifyAll();
        }

        [Fact]
        public async Task PassesCancellationToken_ToRepository()
        {
            var nowUtc = DateTimeOffset.UtcNow;
            var (startUtc, endUtc) = TimeHelper.GetMonthBoundsUtc(nowUtc, TimeHelper.SaoPauloTimeZone);

            var token = new CancellationTokenSource().Token;

            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo.Setup(r => r.CustomerHasOrdersOnMonthAsync(
                    9,
                    It.Is<DateTimeOffset>(d => d == startUtc),
                    It.Is<DateTimeOffset>(d => d == endUtc),
                    It.Is<CancellationToken>(t => t == token)))
                .ReturnsAsync(false);

            var handler = new OneOrderPerMonthHandler(repo.Object);
            var res = await handler.HandleAsync(Ctx(9, nowUtc), token);

            Assert.True(res.Allowed);
            repo.VerifyAll();
        }

        [Fact]
        public void Order_Is_20()
        {
            var handler = new OneOrderPerMonthHandler(Mock.Of<ICustomerRepository>());
            Assert.Equal(20, handler.Order);
        }

    }
}
