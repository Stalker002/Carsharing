using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("trips")]
public class TripEntity
{
    [Column("trip_id")]
    public int Id { get; set; }

    [Column("trip_booking_id")]
    public int BookingId { get; set; }

    [Column("trip_status_id")]
    public int StatusId { get; set; }

    [Column("trip_tariff_type")]
    public string TariffType { get; set; }

    [Column("trip_start_time")]
    public DateTime StartTime { get; set; }

    [Column("trip_end_time")]
    public DateTime EndTime { get; set; }

    [Column("trip_duration")]
    public decimal Duration { get; set; }

    [Column("trip_distance")]
    public decimal Distance { get; set; }

    public BillEntity? Bill { get; set; }
    
    public TripDetailEntity? TripDetail { get; set; }

    public ICollection<FineEntity> Fine { get; set; } = new List<FineEntity>();
    
    public BookingEntity? Booking { get; set; }

    public StatusEntity? Status { get; set; }
}