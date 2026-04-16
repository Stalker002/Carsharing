using Carsharing.Core.Models;
using Shared.Enums;

namespace Carsharing.Tests.Core;

public class CarTests
{
    [Fact]
    public void Create_ValidData_ReturnsCar()
    {
        var (car, error) = Car.Create(1, (int)CarStatusEnum.Available, 1, 1, 1, "Center", 53.900634,
            27.558973, 50m, "image.jpg");

        Assert.NotNull(car);
        Assert.Equal(string.Empty, error);
        Assert.Equal(50m, car.FuelLevel);
        Assert.Equal("Center", car.Location);
    }

    [Fact]
    public void Create_InvalidStatus_ReturnsError()
    {
        var (car, error) = Car.Create(1, 999, 1, 1, 1, "Center", 53.900634,
            27.558973, 50m, null);

        Assert.Null(car);
        Assert.Contains("Invalid", error);
    }

    [Theory]
    [InlineData(-10)]
    public void Create_InvalidFuelLevel_ReturnsError(decimal invalidFuel)
    {
        var (car, error) = Car.Create(1, (int)CarStatusEnum.Available, 1, 1, 1, "Center", 53.900634,
            27.558973, invalidFuel, null);

        Assert.Null(car);
        Assert.NotEmpty(error);
    }

    [Fact]
    public void Create_EmptyLocation_ReturnsError()
    {
        var (car, error) = Car.Create(1, (int)CarStatusEnum.Available, 1, 1, 1, "", 53.900634,
            27.558973, 50m, null);

        Assert.Null(car);
        Assert.Equal("Car location can't be empty", error);
    }

    [Theory]
    [InlineData(-91, 27.558973)]
    [InlineData(91, 27.558973)]
    [InlineData(53.900634, -181)]
    [InlineData(53.900634, 181)]
    public void Create_OutOfRangeCoordinates_ReturnsError(double latitude, double longitude)
    {
        var (car, error) = Car.Create(1, (int)CarStatusEnum.Available, 1, 1, 1, "Center",
            latitude, longitude, 50m, null);

        Assert.Null(car);
        Assert.NotEmpty(error);
    }

    [Fact]
    public void Create_NullCoordinates_ReturnsCar()
    {
        var (car, error) = Car.Create(1, (int)CarStatusEnum.Available, 1, 1, 1, "Center",
            null, null, 50m, null);

        Assert.NotNull(car);
        Assert.Equal(string.Empty, error);
        Assert.Null(car.Latitude);
        Assert.Null(car.Longitude);
    }
}