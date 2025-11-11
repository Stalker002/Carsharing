namespace Carsharing.DataAccess.Entites;

public class ClientEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public UserEntity? User { get; set; }

    public ICollection<ClientDocumentEntity> Documents { get; set; } = new List<ClientDocumentEntity>();

    public ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();

    public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();

}