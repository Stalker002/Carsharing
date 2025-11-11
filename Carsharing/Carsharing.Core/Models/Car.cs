namespace Carsharing.Core.Models;

public class Car
{
    private const int MaxLocationLength = 50;
    private const int MinFuelLevelLength = 0;
    private const int MaxFuelLevelLength = 200;
    private Car(int id,  int statusId, int tariffId, int category, int specificationId, 
        string location, decimal fuelLevel)
    {
        Id = id;
        Location = location;
        StatusId = statusId;
        TariffId = tariffId;
        CategoryId = category;
        SpecificationId = specificationId;
        FuelLevel = fuelLevel;
    }

    public int Id { get; }
          
    public int StatusId { get; }
           
    public int TariffId { get; }
          
    public int CategoryId { get; }
          
    public int SpecificationId { get; }

    public string Location { get; }

    public decimal FuelLevel { get; }

    public static (Car car, string error) Create(int id,  int statusId, int tariffId,
        int categoryId, int specificationId, string location, decimal fuelLevel)
    {
        var error = string.Empty;

        if (statusId < 0)
            error = "Status Id must be positive";

        if (tariffId < 0)
            error = "Tariff Id must be positive";

        if (categoryId < 0)
            error = "Category Id must be positive";

        if (specificationId < 0)
            error = "Specification Id must be positive";

        if (string.IsNullOrWhiteSpace(location))
            error = "Car location can't be empty";
        if (location.Length > MaxLocationLength)
            error = $"Car location can't be longer than {MaxLocationLength} symbols";


        error = fuelLevel switch
        {
            > MaxFuelLevelLength => "There can't be that much fuel in the car.",
            < MinFuelLevelLength => "The fuel in the car must be positive",
            _ => error
        };

        var car = new Car(id, statusId, tariffId, categoryId, specificationId, location, fuelLevel);

        return (car, error);
    }
}