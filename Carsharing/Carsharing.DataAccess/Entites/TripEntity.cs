namespace Carsharing.DataAccess.Entites;

public class TripEntity
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int StatusId { get; set; }

    public string? TariffType { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public decimal? Duration { get; set; }

    public decimal? Distance { get; set; }

    public BillEntity? Bill { get; set; }

    public TripDetailEntity? TripDetail { get; set; }

    public ICollection<FineEntity> Fine { get; set; } = new List<FineEntity>();

    public BookingEntity? Booking { get; set; }

    public TripStatusEntity? TripStatus { get; set; }
}