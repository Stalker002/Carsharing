using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("bookings")]
public class BookingEntity
{
    [Column("booking_id")]
    public int Id { get; set; }
    [Column("booking_status_id")]
    public int StatusId { get; set; }
    [Column("booking_car_id")]
    public int CarId { get; set; }
    [Column("booking_client_id")]
    public int ClientId { get; set; }
    [Column("booking_start_time")]
    public DateTime StartTime { get; set; }
    [Column("booking_end_time")]
    public DateTime EndTime { get; set; }

    public TripEntity? Trip { get; set; }

    public StatusEntity? Status { get; set; }
    
    public CarEntity? Car { get; set; }

    public ClientEntity ? Client { get; set; }
}