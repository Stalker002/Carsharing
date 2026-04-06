using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Moq;
using Shared.Enums;

namespace Carsharing.Tests.Application;

public class BookingsServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly BookingsService _bookingsService;

    public BookingsServiceTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _carRepositoryMock = new Mock<ICarRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _bookingsService = new BookingsService(
            _bookingRepositoryMock.Object,
            _clientRepositoryMock.Object,
            _carRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateBooking_WhenCarIsAvailable_ReservesCarAndCommitsTransaction()
    {
        const int userId = 100;
        const int clientId = 10;
        var startTime = DateTime.UtcNow;
        var endTime = DateTime.UtcNow.AddHours(1);

        var client = Client.Create(clientId, userId, "Test", "User", "+375291234567", "test@example.com").client!;
        _clientRepositoryMock
            .Setup(x => x.GetClientByUserId(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([client]);

        _carRepositoryMock
            .Setup(x => x.TryUpdateStatus(5, (int)CarStatusEnum.Available, (int)CarStatusEnum.Reserved, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _bookingRepositoryMock
            .Setup(x => x.Create(It.Is<Booking>(b =>
                b.StatusId == (int)BookingStatusEnum.Active &&
                b.CarId == 5 &&
                b.ClientId == clientId &&
                b.StartTime == startTime &&
                b.EndTime == endTime), It.IsAny<CancellationToken>()))
            .ReturnsAsync(55);

        using var cts = new CancellationTokenSource();
        CancellationToken specificToken = cts.Token;

        var bookingId = await _bookingsService.CreateBooking(userId, (int)BookingStatusEnum.Active, 5, startTime, endTime, specificToken);

        Assert.Equal(55, bookingId);
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(specificToken), Times.Once);
        _carRepositoryMock.Verify(
            x => x.TryUpdateStatus(5, (int)CarStatusEnum.Available, (int)CarStatusEnum.Reserved, specificToken),
            Times.Once);
        _bookingRepositoryMock.Verify(x => x.Create(It.Is<Booking>(b =>
            b.StatusId == (int)BookingStatusEnum.Active &&
            b.CarId == 5 &&
            b.ClientId == clientId &&
            b.StartTime == startTime &&
            b.EndTime == endTime), specificToken), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(specificToken), Times.Once);
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(specificToken), Times.Never);
    }

    [Fact]
    public async Task CreateBooking_WhenCarStatusUpdateFailsAndCarExists_ThrowsConflictException()
    {
        const int userId = 100;
        const int clientId = 10;
        var startTime = DateTime.UtcNow;
        var endTime = DateTime.UtcNow.AddHours(1);

        var client = Client.Create(clientId, userId, "Test", "User", "+375291234567", "test@example.com").client!;

        var existingCar = Car.Create(
            5,
            (int)CarStatusEnum.Reserved,
            1,
            1,
            1,
            "Center",
            50,
            null).car!;

        _carRepositoryMock
            .Setup(x => x.TryUpdateStatus(5, (int)CarStatusEnum.Available, (int)CarStatusEnum.Reserved, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _clientRepositoryMock
            .Setup(x => x.GetClientByUserId(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([client]);

        _carRepositoryMock
            .Setup(x => x.GetById(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync([existingCar]);

        using var cts = new CancellationTokenSource();
        CancellationToken specificToken = cts.Token;

        await Assert.ThrowsAsync<ConflictException>(() => _bookingsService.CreateBooking(userId, (int)BookingStatusEnum.Active, 5, startTime, endTime, specificToken));

        _bookingRepositoryMock.Verify(x => x.Create(It.IsAny<Booking>(), specificToken), Times.Never);
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(specificToken), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(specificToken), Times.Never);
    }

    [Fact]
    public async Task CreateBooking_WhenBookingCreationFails_RollsBackTransaction()
    {
        const int userId = 100;
        const int clientId = 10;
        var startTime = DateTime.UtcNow;
        var endTime = DateTime.UtcNow.AddHours(1);

        var client = Client.Create(clientId, userId, "Test", "User", "+375291234567", "test@example.com").client!;

        _carRepositoryMock
            .Setup(x => x.TryUpdateStatus(5, (int)CarStatusEnum.Available, (int)CarStatusEnum.Reserved, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _clientRepositoryMock
            .Setup(x => x.GetClientByUserId(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([client]);

        _bookingRepositoryMock
            .Setup(x => x.Create(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException("booking create failed"));

        using var cts = new CancellationTokenSource();
        CancellationToken specificToken = cts.Token;

        await Assert.ThrowsAsync<ArgumentException>(() => _bookingsService.CreateBooking(userId, (int)BookingStatusEnum.Active, 5, startTime, endTime, specificToken));

        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(specificToken), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(specificToken), Times.Never);
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(specificToken), Times.Once);
    }
}
