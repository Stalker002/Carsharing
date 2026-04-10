using Shared.Enums;

namespace Carsharing.Core.Models;

public class Car
{
    public const int MaxLocationLength = 50;
    public const int MinFuelLevelLength = 0;
    public const int MaxFuelLevelLength = 200;

    private Car(int id, int carStatusId, int tariffId, int categoryId, int specificationId,
        string? location, double? latitude, double? longitude, decimal fuelLevel, string? imagePath)
    {
        Id = id;
        Location = location;
        Latitude = latitude;
        Longitude = longitude;
        CarStatusId = carStatusId;
        TariffId = tariffId;
        CategoryId = categoryId;
        SpecificationId = specificationId;
        FuelLevel = fuelLevel;
        ImagePath = imagePath;
    }

    public int Id { get; }

    public int CarStatusId { get; }

    public int TariffId { get; }

    public int CategoryId { get; }

    public int SpecificationId { get; }

    public string? Location { get; }

    public double? Latitude { get; }

    public double? Longitude { get; }

    public decimal FuelLevel { get; }

    public string? ImagePath { get; }

    public static (Car car, string error) Create(int id, int carStatusId, int tariffId,
        int categoryId, int specificationId, string? location, double? latitude, double? longitude, decimal fuelLevel, string? imagePath)
    {
        var error = string.Empty;

        if (!System.Enum.IsDefined(typeof(CarStatusEnum), carStatusId))
            error = "Invalid insurance type. Allowed: \"1.Доступен\", \"2. Недоступен\", \"3. На обслуживании\", \"4. В ремонте\"";

        if (tariffId < 0)
            error = "Tariff Id must be positive";

        if (categoryId < 0)
            error = "Category Id must be positive";

        if (specificationId < 0)
            error = "Specification Id must be positive";

        if (string.IsNullOrWhiteSpace(location))
            error = "Car location can't be empty";
        if (location is { Length: > MaxLocationLength })
            error = $"Car location can't be longer than {MaxLocationLength} symbols";

        if (latitude.HasValue != longitude.HasValue)
            error = "Latitude and longitude must be provided together";

        if (latitude is < -90 or > 90)
            error = "Latitude must be between -90 and 90";

        if (longitude is < -180 or > 180)
            error = "Longitude must be between -180 and 180";

        error = fuelLevel switch
        {
            > MaxFuelLevelLength => "There can't be that much fuel in the car.",
            < MinFuelLevelLength => "The fuel in the car must be positive",
            _ => error
        };
        
        if(error.Length != 0)
        {
            return (null, error)!;
        }

        var car = new Car(id, carStatusId, tariffId, categoryId, specificationId, location, latitude, longitude, fuelLevel, imagePath);

        return (car, error);
    }
}
