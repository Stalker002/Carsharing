namespace Carsharing.Core.Models;

public class Car
{
    private const int MinFuelLevelLength = 0;
    private const int MaxFuelLevelLength = 200;
    private Car(int id, int locationId, int statusId, int tariffId, 
        int category, int specificationId, decimal fuelLevel)
    {
        Id = id;
        LocationId = locationId;
        StatusId = statusId;
        TariffId = tariffId;
        CategoryId = category;
        SpecificationId = specificationId;
        FuelLevel = fuelLevel;
    }

    public int Id { get; set; }

    public int LocationId { get; set; }
          
    public int StatusId { get; set; }
           
    public int TariffId { get; set; }
          
    public int CategoryId { get; set; }
          
    public int SpecificationId { get; set; }
          
    public decimal FuelLevel { get; set; }

    public static (Car car, string error) Create(int id, int locationId, int statusId, int tariffId,
        int categoryId, int specificationId, decimal fuelLevel)
    {
        var error = string.Empty;

        if (locationId < 0)
            error = "Location Id must be positive";

        if (statusId < 0)
            error = "Status Id must be positive";

        if (tariffId < 0)
            error = "Tariff Id must be positive";

        if (categoryId < 0)
            error = "Category Id must be positive";

        if (specificationId < 0)
            error = "Specification Id must be positive";

        error = fuelLevel switch
        {
            > MaxFuelLevelLength => "There can't be that much fuel in the car.",
            < MinFuelLevelLength => "The fuel in the car must be positive",
            _ => error
        };

        var car = new Car(id, locationId, statusId, tariffId, categoryId, specificationId, fuelLevel);

        return (car, error);
    }
}