using Carsharing.Core.Enum;
using Carsharing.Core.Models;

namespace Carsharing.Tests.Core;

public class CarTests
{
    [Fact]
    public void Create_ValidData_ReturnsCar()
    {
        var (car, error) = Car.Create(1, (int)CarStatusEnum.Available, 1, 1, 1, "Center", 50m, "image.jpg");

        Assert.NotNull(car);
        Assert.Equal(string.Empty, error);
        Assert.Equal(50m, car.FuelLevel);
        Assert.Equal("Center", car.Location);
    }

    [Fact]
    public void Create_InvalidStatus_ReturnsError()
    {
        var (car, error) = Car.Create(1, 999, 1, 1, 1, "Center", 50m, null);

        Assert.Null(car);
        Assert.Contains("Invalid", error);
    }

    [Theory]
    [InlineData(-10)] 
    public void Create_InvalidFuelLevel_ReturnsError(decimal invalidFuel)
    {
        var (car, error) = Car.Create(1, (int)CarStatusEnum.Available, 1, 1, 1, "Center", invalidFuel, null);

        Assert.Null(car);
        Assert.NotEmpty(error);
    }

    [Fact]
    public void Create_EmptyLocation_ReturnsError()
    {
        var (car, error) = Car.Create(1, (int)CarStatusEnum.Available, 1, 1, 1, "", 50m, null);

        Assert.Null(car);
        Assert.Equal("Car location can't be empty", error);
    }
}