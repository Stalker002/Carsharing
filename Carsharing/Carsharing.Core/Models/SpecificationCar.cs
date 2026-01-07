using System.Text.RegularExpressions;

namespace Carsharing.Core.Models;

public class SpecificationCar
{
    public const int MaxBrandLength = 50;
    public const int MaxModelLength = 100;
    public const int MaxVinNumberLength = 17;
    public const int MaxStateNumberLength = 15;

    private SpecificationCar(int id, string? fuelType, string? brand, string? model, string? transmission, int year,
        string? vinNumber, string stateNumber, int mileage, decimal maxFuel, decimal fuelPerKm)
    {
        Id = id;
        FuelType = fuelType;
        Brand = brand;
        Model = model;
        Transmission = transmission;
        Year = year;
        VinNumber = vinNumber;
        StateNumber = stateNumber;
        Mileage = mileage;
        MaxFuel = maxFuel;
        FuelPerKm = fuelPerKm;
    }

    public int Id { get; }

    public string? FuelType { get; }

    public string? Brand { get; }

    public string? Model { get; }

    public string? Transmission { get; }

    public int Year { get; }

    public string? VinNumber { get; }

    public string StateNumber { get; }

    public int Mileage { get; }

    public decimal MaxFuel { get; }

    public decimal FuelPerKm { get; }

    public static (SpecificationCar specificationCar, string error) Create(int id, string? fuelType, string? brand,
        string? model, string? transmission, int year, string? vinNumber, string? stateNumber, int mileage,
        decimal maxFuel, decimal fuelPerKm)
    {
        var error = string.Empty;
        var allowedFuelType = new[] { "Бензин", "Дизель", "Электро", "Гибрид", "Газ" };
        var allowedTransmissionType = new[] { "Автомат", "Механика", "Робот" };

        if (string.IsNullOrWhiteSpace(fuelType))
            error = "Fuel type can't be empty";
        if (!allowedFuelType.Contains(fuelType))
            error = $"Invalid fuel type. Allowed: {string.Join(", ", allowedFuelType)}";

        if (string.IsNullOrWhiteSpace(brand))
            error = "Brand can't be empty";
        if (brand is { Length: > MaxBrandLength })
            error = $"Brand name can't be longer than {MaxBrandLength} symbols";

        if (string.IsNullOrWhiteSpace(model))
            error = "Model can't be empty";
        if (model is { Length: > MaxModelLength })
            error = $"Model name can't be longer than {MaxModelLength} symbols";

        if (string.IsNullOrWhiteSpace(transmission))
            error = "Transmission can't be empty";
        if (!allowedTransmissionType.Contains(transmission))
            error = $"Invalid transmission. Allowed: {string.Join(", ", allowedTransmissionType)}";

        error = year switch
        {
            < 1900 => "We don't drive our clients around in old junk cars.",
            > 2025 => "Cars don't fit into the future.",
            _ => error
        };

        if (string.IsNullOrWhiteSpace(vinNumber))
            error = "Vin Number can't be empty";
        if (vinNumber is { Length: > MaxVinNumberLength })
            error = $"Vin number name can't be longer than {MaxVinNumberLength} symbols";
        /*if (!Regex.IsMatch(vinNumber, @"^[A-HJ-NPR-Z0-9]{17}$"))
            error = "VIN number in invalid */

        if (string.IsNullOrWhiteSpace(stateNumber))
            error = "State Number can't be empty";
        if (stateNumber is { Length: > MaxStateNumberLength })
            error = $"State number name can't be longer than {MaxStateNumberLength} symbols";
        if (!Regex.IsMatch(stateNumber ?? throw new ArgumentNullException(nameof(stateNumber)),
                @"^(\d{4}\s?[ABEIKMHOPCTX]{2}-[1-7]|[ABEIKMHOPCTX]{2}\s?\d{4}-[1-7]|(TA|TT|TY)\d{4}|E\d{3}[ABEIKMHOPCTX]{2}[1-7])$"))
            error = "State number in invalid format";

        if (mileage < 0)
            error = "Mileage must be positive";

        if (maxFuel < 0)
            error = "Max fuel must be positive";

        if (fuelPerKm < 0)
            error = "Fuel per km must be positive";

        var specificationCar = new SpecificationCar(id, fuelType, brand, model, transmission, year, vinNumber,
            stateNumber, mileage, maxFuel, fuelPerKm);

        return (specificationCar, error);
    }
}