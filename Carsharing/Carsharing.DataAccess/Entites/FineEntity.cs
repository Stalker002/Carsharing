namespace Carsharing.DataAccess.Entites;

public class FineEntity
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public int StatusId { get; set; }

    public string Type { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public TripEntity? Trip { get; set; }

    public StatusEntity? Status { get; set; }
}