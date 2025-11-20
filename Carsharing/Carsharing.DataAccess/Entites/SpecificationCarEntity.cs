namespace Carsharing.DataAccess.Entites;

public class SpecificationCarEntity
{
    public int Id { get; set; }

    public string? FuelType { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public string? Transmission { get; set; }

    public int Year { get; set; }

    public string? VinNumber { get; set; }

    public string? StateNumber { get; set; }

    public int Mileage { get; set; }

    public decimal MaxFuel { get; set; }

    public decimal FuelPerKm { get; set; }

    public CarEntity? Car { get; set; }
}