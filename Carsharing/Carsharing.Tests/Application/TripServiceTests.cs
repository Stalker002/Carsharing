using Carsharing.Application.Abstractions;
using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Moq;
using Shared.Contracts.Trip;
using Shared.Enums;

namespace Carsharing.Tests.Application;

public class TripServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepoMock;
    private readonly Mock<ICarsService> _carsServiceMock;
    private readonly Mock<IClientRepository> _clientRepoMock;
    private readonly Mock<ITripRepository> _tripRepoMock;
    private readonly TripService _tripService;

    public TripServiceTests()
    {
        _tripRepoMock = new Mock<ITripRepository>();

        var mockDetail = new Mock<ITripDetailRepository>();
        _clientRepoMock = new Mock<IClientRepository>();
        _carsServiceMock = new Mock<ICarsService>();
        _bookingRepoMock = new Mock<IBookingRepository>();
        var mockBill = new Mock<IBillRepository>();

        _tripService = new TripService(
            _tripRepoMock.Object,
            mockDetail.Object,
            _clientRepoMock.Object,
            null!,
            _carsServiceMock.Object,
            _bookingRepoMock.Object,
            mockBill.Object);
    }

    [Fact]
    public async Task CancelTripAsync_CallsRepositoryAndReturnsTrue()
    {
        const int tripIdToCancel = 5;

        _tripRepoMock.Setup(x => x.CancelTripAsync(tripIdToCancel, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        using var cts = new CancellationTokenSource();
        var specificToken = cts.Token;

        var result = await _tripService.CancelTripAsync(tripIdToCancel, specificToken);

        Assert.True(result);

        _tripRepoMock.Verify(x => x.CancelTripAsync(tripIdToCancel, specificToken), Times.Once);
    }

    [Fact]
    public async Task UpdateTripLocationAsync_ValidActiveTrip_UpdatesCarLocation()
    {
        const int userId = 5;
        const int clientId = 15;
        const int tripId = 25;
        const int bookingId = 35;
        const int carId = 45;

        var (trip, tripError) = Trip.Create(
            tripId,
            bookingId,
            (int)TripStatusEnum.EnRoute,
            "per_minute",
            DateTime.UtcNow.AddMinutes(-10),
            null,
            null,
            null);
        Assert.True(string.IsNullOrEmpty(tripError));

        var (client, clientError) = Client.Create(
            clientId,
            userId,
            "Trip",
            "Client",
            "+375291112233",
            "trip.client@test.com");
        Assert.True(string.IsNullOrEmpty(clientError));

        _clientRepoMock.Setup(x => x.GetClientByUserId(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([client]);
        _tripRepoMock.Setup(x => x.GetById(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([trip]);
        _bookingRepoMock.Setup(x => x.GetById(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([Booking.Create(
                bookingId,
                (int)BookingStatusEnum.Active,
                carId,
                clientId,
                DateTime.UtcNow.AddMinutes(-20),
                DateTime.UtcNow.AddHours(1)).booking]);

        var request = new UpdateTripLocationRequest("Minsk, Pobediteley Ave", 53.90454, 27.56152);

        await _tripService.UpdateTripLocationAsync(userId, tripId, request, CancellationToken.None);

        _carsServiceMock.Verify(x => x.UpdateCarLocationAsync(
            carId,
            request.Location,
            request.CarLatitude,
            request.CarLongitude,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTripLocationAsync_ForeignTrip_ThrowsUnauthorized()
    {
        const int userId = 6;
        const int currentClientId = 16;
        const int foreignClientId = 17;
        const int tripId = 26;
        const int bookingId = 36;

        var (trip, tripError) = Trip.Create(
            tripId,
            bookingId,
            (int)TripStatusEnum.EnRoute,
            "per_minute",
            DateTime.UtcNow.AddMinutes(-10),
            null,
            null,
            null);
        Assert.True(string.IsNullOrEmpty(tripError));

        var (client, clientError) = Client.Create(
            currentClientId,
            userId,
            "Trip",
            "Client",
            "+375291112234",
            "trip.owner@test.com");
        Assert.True(string.IsNullOrEmpty(clientError));

        _clientRepoMock.Setup(x => x.GetClientByUserId(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([client]);
        _tripRepoMock.Setup(x => x.GetById(tripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([trip]);
        _bookingRepoMock.Setup(x => x.GetById(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([Booking.Create(
                bookingId,
                (int)BookingStatusEnum.Active,
                46,
                foreignClientId,
                DateTime.UtcNow.AddMinutes(-20),
                DateTime.UtcNow.AddHours(1)).booking]);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _tripService.UpdateTripLocationAsync(
            userId,
            tripId,
            new UpdateTripLocationRequest("Minsk", 53.9, 27.56),
            CancellationToken.None));

        _carsServiceMock.Verify(x => x.UpdateCarLocationAsync(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<double>(),
            It.IsAny<double>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
}
