namespace Carsharing.DataAccess.Entites;

public class BookingEntity
{
    public int Id { get; set; }
    public int StatusId { get; set; }
    public int CarId { get; set; }
    public int ClientId { get; set; }
    public DateTime? StartTime { get; set; }


    public DateTime? EndTime { get; set; }

    public TripEntity? Trip { get; set; }

    public BookingStatusEntity? BookingStatus { get; set; }

    public CarEntity? Car { get; set; }

    public ClientEntity? Client { get; set; }
}