using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Moq;

namespace Carsharing.Tests.Application;

public class BillsServiceTests
{
    private readonly Mock<IBillRepository> _billRepoMock;
    private readonly BillsService _billsService;
    private readonly Mock<IPromocodeRepository> _promoRepoMock;

    public BillsServiceTests()
    {
        _billRepoMock = new Mock<IBillRepository>();
        var bookingRepoMock = new Mock<IBookingRepository>();
        var clientRepoMock = new Mock<IClientRepository>();
        _promoRepoMock = new Mock<IPromocodeRepository>();
        var tripRepoMock = new Mock<ITripRepository>();
        _billsService = new BillsService(
            _billRepoMock.Object,
            _promoRepoMock.Object,
            tripRepoMock.Object,
            bookingRepoMock.Object,
            clientRepoMock.Object);
    }

    [Fact]
    public async Task ApplyPromocode_PromocodeNotFound_ThrowsException()
    {
        const int billId = 1;
        const string code = "INVALID_CODE";

        _promoRepoMock.Setup(x => x.GetByCode(code, It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
            _billsService.ApplyPromocode(billId, code, CancellationToken.None));
        Assert.Equal("Промокод не найден или истек", ex.Message);
    }

    [Fact]
    public async Task ApplyPromocode_BillNotFound_ThrowsException()
    {
        const int billId = 99;
        const string code = "VALID";
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var validPromo = Promocode.Create(1, 20, code, 10, today, today.AddDays(2)).promocode;

        _promoRepoMock.Setup(x => x.GetByCode(code, It.IsAny<CancellationToken>())).ReturnsAsync([validPromo]);

        _billRepoMock.Setup(x => x.GetById(billId, It.IsAny<CancellationToken>())).ReturnsAsync((Bill?)null);

        var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
            _billsService.ApplyPromocode(billId, code, CancellationToken.None));
        Assert.Equal("Счет не найден", ex.Message);
    }

    [Fact]
    public async Task ApplyPromocode_ValidData_CallsUpdate()
    {
        const int billId = 1;
        const string code = "VALID";
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var validPromo = Promocode.Create(10, 20, code, 10, today, today.AddDays(2)).promocode;

        var futureDate = DateTime.Now.AddDays(1);
        var validBill = Bill.Create(billId, 1, null, 13, futureDate, 100, 100).bill;

        _promoRepoMock.Setup(x => x.GetByCode(code, It.IsAny<CancellationToken>())).ReturnsAsync([validPromo]);
        _billRepoMock.Setup(x => x.GetById(billId, It.IsAny<CancellationToken>())).ReturnsAsync(validBill);

        using var cts = new CancellationTokenSource();
        var specificToken = cts.Token;

        await _billsService.ApplyPromocode(billId, code, specificToken);

        _billRepoMock.Verify(x => x.Update(
            billId, null, validPromo.Id, null, null, null, null, specificToken), Times.Once);
    }
}