using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("cars")]
public class CarEntity
{
    [Column("car_id")]
    public int Id { get; set; }

    [Column("car_status_id")]
    public int StatusId { get; set; }

    [Column("car_tariff_id")]
    public int TariffId { get; set; }

    [Column("car_category_id")]
    public int CategoryId { get; set; }

    [Column("car_specification_id")]
    public int SpecificationId { get; set; }

    [Column("car_location")]
    public string Location { get; set; }

    [Column("car_fuel_level")]
    public decimal FuelLevel { get; set; }
}