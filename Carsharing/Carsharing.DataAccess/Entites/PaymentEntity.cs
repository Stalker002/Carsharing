using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("payments")]
public class PaymentEntity
{
    [Column("payment_id")]
    public int Id { get; set; }

    [Column("payment_bill_id")]
    public int BillId { get; set; }

    [Column("payment_sum")]
    public decimal Sum { get; set; }

    [Column("payment_method")]
    public string Method { get; set; }

    [Column("payment_date")]
    public DateTime Date { get; set; }
}