namespace Carsharing.DataAccess.Entites;

public class BillEntity
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public int PromocodeId { get; set; }

    public int StatusId { get; set; } = 1;

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    public decimal Amount { get; set; }

    public decimal RemainingAmount { get; set; }

    public ICollection<PaymentEntity> PaymentEntities { get; set; } = new List<PaymentEntity>();
    
    public TripEntity? Trip { get; set; }

    public PromocodeEntity? Promocode { get; set; }

    public StatusEntity? Status { get; set; }
}