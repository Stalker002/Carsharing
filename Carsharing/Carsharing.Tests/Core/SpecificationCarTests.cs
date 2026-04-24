using Carsharing.Core.Models;

namespace Carsharing.Tests.Core;

public class SpecificationCarTests
{
    [Fact]
    public void Create_ValidData_ReturnsSpecification()
    {
        const string validStateNumber = "1234 AB-7";

        var (spec, error) = SpecificationCar.Create(
            1,
            "Бензин",
            "Toyota",
            "Camry",
            "Автомат",
            2020,
            "1HGCM82633A004111",
            validStateNumber,
            15000,
            60m,
            0.08m
        );

        Assert.NotNull(spec);
        Assert.Equal(string.Empty, error);
        Assert.Equal("Toyota", spec.Brand);
    }

    [Theory]
    [InlineData("Вода")]
    [InlineData("")]
    public void Create_InvalidFuelType_ReturnsError(string invalidFuelType)
    {
        var (spec, error) = SpecificationCar.Create(1, invalidFuelType, "T", "C", "Автомат", 2020, "12345678901234567",
            "1234 AB-7", 10, 50, 0.1m);

        Assert.Null(spec);
        Assert.Contains("Invalid fuel type", error);
    }

    [Theory]
    [InlineData(1899)]
    public void Create_InvalidYear_ReturnsError(int invalidYear)
    {
        var (spec, error) = SpecificationCar.Create(1, "Бензин", "T", "C", "Автомат", invalidYear, "12345678901234567",
            "1234 AB-7", 10, 50, 0.1m);

        Assert.Null(spec);
        Assert.NotEmpty(error);
    }

    [Theory]
    [InlineData("1234 ABC-7")]
    [InlineData("XYZ 123")]
    public void Create_InvalidStateNumber_ReturnsFormatError(string invalidNumber)
    {
        var (spec, error) = SpecificationCar.Create(1, "Бензин", "T", "C", "Автомат", 2020, "12345678901234567",
            invalidNumber, 10, 50, 0.1m);

        Assert.Null(spec);
        Assert.Equal("State number in invalid format", error);
    }
}