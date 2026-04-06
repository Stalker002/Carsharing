using Carsharing.Core.Models;
using Shared.Enums;

namespace Carsharing.Tests.Core;

public class TripTests
{
    [Fact]
    public void Create_ValidData_ReturnsTrip()
    {
        var startTime = DateTime.UtcNow.AddMinutes(-30);
        var endTime = DateTime.UtcNow;

        var (trip, error) = Trip.Create(
            id: 1,
            bookingId: 10,
            statusId: (int)TripStatusEnum.Finished,
            tariffType: "per_minute",
            startTime: startTime,
            endTime: endTime,
            duration: 30m,
            distance: 15.5m
        );

        Assert.NotNull(trip);
        Assert.Equal(string.Empty, error);
        Assert.Equal(30m, trip.Duration);
    }

    [Fact]
    public void Create_EndTimeBeforeStartTime_ReturnsError()
    {
        var startTime = DateTime.UtcNow;
        var endTime = startTime.AddMinutes(-10);

        var (trip, error) = Trip.Create(1, 10, (int)TripStatusEnum.Finished, "per_minute", startTime, endTime, 10m, 5m);

        Assert.Null(trip);
        Assert.Equal("Start time can not exceed end time ", error);
    }

    [Fact]
    public void Create_NegativeDurationOrDistance_ReturnsError()
    {
        var (trip, error) = Trip.Create(1, 10, (int)TripStatusEnum.Finished, "per_minute",
            DateTime.UtcNow.AddMinutes(-5), DateTime.UtcNow, -5m, 10m);

        Assert.Null(trip);
        Assert.Equal("Duration must be positive", error);

        var (trip2, error2) = Trip.Create(1, 10, (int)TripStatusEnum.Finished, "per_minute",
            DateTime.UtcNow.AddMinutes(-5), DateTime.UtcNow, 5m, -10m);

        Assert.Null(trip2);
        Assert.Equal("Distance must be positive", error2);
    }

    [Fact]
    public void Create_InvalidTariffType_ReturnsError()
    {
        var (trip, error) = Trip.Create(1, 10, (int)TripStatusEnum.Finished, "per_hour", DateTime.UtcNow, null, 0, 0);

        Assert.Null(trip);
        Assert.Contains("Invalid tariff type", error);
    }
}