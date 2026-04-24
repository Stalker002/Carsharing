using Carsharing.Application.Abstractions;
using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Moq;

namespace Carsharing.Tests.Application;

public class TripDetailsServiceTests
{
    private readonly Mock<IBillingLifecycleService> _billingLifecycleMock;
    private readonly Mock<IInsuranceRepository> _insuranceRepoMock;
    private readonly TripDetailsService _service;
    private readonly Mock<ITripDetailRepository> _tripDetailRepoMock;

    public TripDetailsServiceTests()
    {
        _tripDetailRepoMock = new Mock<ITripDetailRepository>();
        _insuranceRepoMock = new Mock<IInsuranceRepository>();
        _billingLifecycleMock = new Mock<IBillingLifecycleService>();

        _service = new TripDetailsService(
            _tripDetailRepoMock.Object,
            _insuranceRepoMock.Object,
            _billingLifecycleMock.Object);
    }

    [Fact]
    public async Task CreateTripDetail_CarNotFound_ThrowsNotFoundException()
    {
        var tripDetail = TripDetail.Create(1, 10, "Point A", "Point B", false, 0, 0).tripDetail;

        _tripDetailRepoMock.Setup(x => x.GetCarIdByTripId(tripDetail.TripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        using var cts = new CancellationTokenSource();
        var specificToken = cts.Token;

        var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.CreateTripDetail(tripDetail, specificToken));
        Assert.Contains("или связанный автомобиль не найдены", ex.Message);

        _tripDetailRepoMock.Verify(x => x.Create(It.IsAny<TripDetail>(), specificToken), Times.Never);
    }

    [Fact]
    public async Task CreateTripDetail_WithInsuranceButNoActivePolicy_ThrowsConflictException()
    {
        const int carId = 5;
        var tripDetail = TripDetail.Create(1, 10, "Point A", "Point B", true, 0, 0).tripDetail;

        _tripDetailRepoMock.Setup(x => x.GetCarIdByTripId(tripDetail.TripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(carId);
        _billingLifecycleMock.Setup(x => x.NormalizeTripDetailAsync(tripDetail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tripDetail);

        _insuranceRepoMock.Setup(x => x.GetActiveByCarId(carId, It.IsAny<CancellationToken>())).ReturnsAsync([]);

        using var cts = new CancellationTokenSource();
        var specificToken = cts.Token;

        var ex = await Assert.ThrowsAsync<ConflictException>(() =>
            _service.CreateTripDetail(tripDetail, specificToken));
        Assert.Equal("Нельзя начать поездку с опцией страховки: у автомобиля нет активного полиса", ex.Message);

        _tripDetailRepoMock.Verify(x => x.Create(It.IsAny<TripDetail>(), specificToken), Times.Never);
    }

    [Fact]
    public async Task CreateTripDetail_ValidData_CallsRepositoryCreate()
    {
        const int carId = 5;
        var tripDetail = TripDetail.Create(1, 10, "Point A", "Point B", false, 0, 0).tripDetail;

        _tripDetailRepoMock.Setup(x => x.GetCarIdByTripId(tripDetail.TripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(carId);
        _billingLifecycleMock.Setup(x => x.NormalizeTripDetailAsync(tripDetail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tripDetail);

        _tripDetailRepoMock.Setup(x => x.Create(tripDetail, It.IsAny<CancellationToken>())).ReturnsAsync(99);

        using var cts = new CancellationTokenSource();
        var specificToken = cts.Token;

        var resultId = await _service.CreateTripDetail(tripDetail, specificToken);

        Assert.Equal(99, resultId);

        _tripDetailRepoMock.Verify(x => x.Create(tripDetail, specificToken), Times.Once);
        _billingLifecycleMock.Verify(x => x.SyncCarFuelLevelAsync(tripDetail.TripId, specificToken), Times.Once);

        _insuranceRepoMock.Verify(x => x.GetActiveByCarId(It.IsAny<int>(), specificToken), Times.Never);
    }
}
