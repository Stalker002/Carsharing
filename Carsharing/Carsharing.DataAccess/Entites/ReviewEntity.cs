namespace Carsharing.DataAccess.Entites;

public class ReviewEntity
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int CarId { get; set; }

    public short Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime Date { get; set; }

    public ClientEntity? Client { get; set; }

    public CarEntity? Car { get; set; }
}