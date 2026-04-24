using Carsharing.Application.Abstractions;
using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Moq;

namespace Carsharing.Tests.Application;

public class PaymentServiceTests
{
    private readonly Mock<IBillingLifecycleService> _billingLifecycleMock = new();
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock = new();
    private readonly PaymentService _service;

    public PaymentServiceTests()
    {
        _service = new PaymentService(
            _paymentRepositoryMock.Object,
            Mock.Of<IBillRepository>(),
            Mock.Of<ITripRepository>(),
            Mock.Of<IBookingRepository>(),
            Mock.Of<IClientRepository>(),
            _billingLifecycleMock.Object);
    }

    [Fact]
    public async Task CreatePayment_ValidPayment_RecalculatesBillAfterCreate()
    {
        var payment = Payment.Create(0, 10, 25m, "Картой", DateTime.UtcNow).payment;
        _paymentRepositoryMock.Setup(x => x.Create(payment, It.IsAny<CancellationToken>())).ReturnsAsync(77);

        var result = await _service.CreatePayment(payment, CancellationToken.None);

        Assert.Equal(77, result);
        _billingLifecycleMock.Verify(x => x.EnsureNoOverpaymentOnCreateAsync(payment, It.IsAny<CancellationToken>()),
            Times.Once);
        _billingLifecycleMock.Verify(x => x.RecalculateBillAsync(payment.BillId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeletePayment_PaymentMissing_ThrowsNotFound()
    {
        _paymentRepositoryMock.Setup(x => x.GetById(5, It.IsAny<CancellationToken>())).ReturnsAsync([]);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeletePayment(5, CancellationToken.None));
    }
}
