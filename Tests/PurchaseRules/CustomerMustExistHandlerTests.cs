using Moq;
using ProvaPub.Dtos;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services.PurchasesRules;
using Xunit;
using static ProvaPub.Services.Interfaces.IPurchaseHandler;

namespace ProvaPub.Tests.PurchaseRules
{
    public class CustomerMustExistHandlerTests
    {
        private static PurchaseContext Ctx(int customerId, decimal value = 10m) =>
     new(  new PurchaseRequest(customerId, value), DateTimeOffset.UtcNow);


        [Fact]
        public async Task InvalidCustomerId_Fails_WithoutRepoCall()
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            var handler = new CustomerMustExistHandler(repo.Object);

            var res = await handler.HandleAsync(Ctx(0), CancellationToken.None);

            Assert.False(res.Allowed);
            Assert.Contains("invalid_customer_id", res.Reasons);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ExistingCustomer_Ok()
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo.Setup(r => r.ExistsAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var handler = new CustomerMustExistHandler(repo.Object);
            var res = await handler.HandleAsync(Ctx(5), CancellationToken.None);

            Assert.True(res.Allowed);
            Assert.Empty(res.Reasons);
            repo.VerifyAll();
        }

        [Fact]
        public async Task NotExistingCustomer_FailsWithCustomerNotFound()
        {
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo.Setup(r => r.ExistsAsync(9, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var handler = new CustomerMustExistHandler(repo.Object);
            var res = await handler.HandleAsync(Ctx(9), CancellationToken.None);

            Assert.False(res.Allowed);
            Assert.Contains("customer_not_found", res.Reasons);
            repo.VerifyAll();
        }

        [Fact]
        public async Task PassesCancellationToken_ToRepository()
        {
            var token = new CancellationTokenSource().Token;
            var repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
            repo.Setup(r => r.ExistsAsync(7, It.Is<CancellationToken>(t => t == token))).ReturnsAsync(true);

            var handler = new CustomerMustExistHandler(repo.Object);
            var res = await handler.HandleAsync(Ctx(7), token);

            Assert.True(res.Allowed);
            repo.VerifyAll();
        }

        [Fact]
        public void Order_Is_10()
        {
            var handler = new CustomerMustExistHandler(Mock.Of<ICustomerRepository>());
            Assert.Equal(10, handler.Order);
        }

    }
}
