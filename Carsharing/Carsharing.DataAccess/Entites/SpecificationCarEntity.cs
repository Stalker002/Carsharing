using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("specification_car")]
public class SpecificationCarEntity
{
    [Column("specification_car_id")]
    public int Id { get; set; }

    [Column("specification_car_fuel_type")]
    public string FuelType { get; set; }

    [Column("specification_car_brand")]
    public string Brand { get; set; }

    [Column("specification_car_model")]
    public string Model { get; set; }

    [Column("specification_car_transmission")]
    public string Transmission { get; set; }

    [Column("specification_car_year")]
    public int Year { get; set; }

    [Column("specification_car_vin_number")]
    public string VinNumber { get; set; }

    [Column("specification_car_state_number")]
    public string StateNumber { get; set; }

    [Column("specification_car_mileage")]
    public int Mileage { get; set; }

    [Column("specification_car_max_fuel")]
    public decimal MaxFuel { get; set; }

    [Column("specification_car_fuel_per_km")]
    public decimal FuelPerKm { get; set; }
}