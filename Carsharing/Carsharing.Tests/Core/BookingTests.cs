using Carsharing.Core.Enum;
using Carsharing.Core.Models;

namespace Carsharing.Tests.Core;

public class BookingTests
{
    [Fact]
    public void Create_ValidData_ReturnsBooking()
    {
        var startTime = DateTime.UtcNow.AddMinutes(-5);
        var endTime = DateTime.UtcNow.AddMinutes(30);

        var (booking, error) = Booking.Create(1, (int)BookingStatusEnum.Active, 10, 20, startTime, endTime);

        Assert.NotNull(booking);
        Assert.Equal(string.Empty, error);
        Assert.Equal(10, booking.CarId);
    }

    [Fact]
    public void Create_InvalidStatus_ReturnsError()
    {
        const int invalidStatusId = 999;

        var (booking, error) = Booking.Create(1, invalidStatusId, 10, 20, DateTime.UtcNow, null);

        Assert.Null(booking);
        Assert.Contains("Invalid booking status type", error);
    }

    [Fact]
    public void Create_StartTimeAfterEndTime_ReturnsError()
    {
        var startTime = DateTime.UtcNow;
        var endTime = DateTime.UtcNow.AddHours(-1);

        var (booking, error) = Booking.Create(1, (int)BookingStatusEnum.Active, 10, 20, startTime, endTime);

        Assert.Null(booking);
        Assert.Equal("Start time can not exceed end time", error);
    }
}