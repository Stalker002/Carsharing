using Carsharing.Application.Abstractions;
using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Moq;

namespace Carsharing.Tests.Application;

public class TripServiceTests
{
    private readonly Mock<ITripRepository> _tripRepoMock;
    private readonly TripService _tripService;

    public TripServiceTests()
    {
        _tripRepoMock = new Mock<ITripRepository>();
        
        var mockDetail = new Mock<ITripDetailRepository>();
        var mockClient = new Mock<IClientRepository>();
        var mockCars = new Mock<ICarsService>();
        var mockBooking = new Mock<IBookingRepository>();
        var mockBill = new Mock<IBillRepository>();

        _tripService = new TripService(
            _tripRepoMock.Object, 
            mockDetail.Object, 
            mockClient.Object, 
            null!,
            mockCars.Object, 
            mockBooking.Object,
            mockBill.Object);
    }

    [Fact]
    public async Task CancelTripAsync_CallsRepositoryAndReturnsTrue()
    {
        const int tripIdToCancel = 5;

        _tripRepoMock.Setup(x => x.CancelTripAsync(tripIdToCancel)).Returns(Task.CompletedTask);

        var result = await _tripService.CancelTripAsync(tripIdToCancel);

        Assert.True(result);
        
        _tripRepoMock.Verify(x => x.CancelTripAsync(tripIdToCancel), Times.Once);
    }
}