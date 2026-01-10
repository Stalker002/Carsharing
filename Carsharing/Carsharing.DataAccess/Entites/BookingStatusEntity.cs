namespace Carsharing.DataAccess.Entites;

public class BookingStatusEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
}