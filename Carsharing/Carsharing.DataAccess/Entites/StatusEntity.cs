namespace Carsharing.DataAccess.Entites;

public class StatusEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<InsuranceEntity> Insurances { get; set; } = new List<InsuranceEntity>();

    public ICollection<CarEntity> Cars { get; set; } = new List<CarEntity>();

    public ICollection<PromocodeEntity> Promocodes { get; set; } = new List<PromocodeEntity>();

    public ICollection<BillEntity> Bills { get; set; } = new List<BillEntity>();

    public ICollection<FineEntity> Fines { get; set; } = new List<FineEntity>();

    public ICollection<TripEntity> Trip { get; set; } = new List<TripEntity>();

    public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
}