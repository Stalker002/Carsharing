namespace Carsharing.DataAccess.Entites;

public class PaymentEntity
{
    public int Id { get; set; }

    public int BillId { get; set; }

    public decimal Sum { get; set; }

    public string? Method { get; set; }

    public DateTime Date { get; set; }

    public BillEntity? Bill { get; set; }
}