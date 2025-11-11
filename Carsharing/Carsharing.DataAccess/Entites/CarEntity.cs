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

    public ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();

    public ICollection<MaintenanceEntity> Maintenance { get; set; } = new List<MaintenanceEntity>();

    public ICollection<BookingEntity> Booking { get; set; } = new List<BookingEntity>();

    public ICollection<InsuranceEntity> Insurance { get; set; } = new List<InsuranceEntity>();

    public StatusEntity? Status { get; set; }

    public TariffEntity? Tariff { get; set; }

    public CategoryEntity? Category { get; set; }

    public SpecificationCarEntity? SpecificationCar { get; set; }
}