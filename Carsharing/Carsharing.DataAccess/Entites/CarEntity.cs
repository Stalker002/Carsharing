namespace Carsharing.DataAccess.Entites;

public class CarEntity
{
    public int Id { get; set; }

    public int StatusId { get; set; }

    public int TariffId { get; set; }

    public int CategoryId { get; set; }

    public int SpecificationId { get; set; }

    public string Location { get; set; }

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