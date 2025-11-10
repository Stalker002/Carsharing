using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("bills")]
public class BillEntity
{
    [Column("bill_id")]
    public int Id { get; set; }

    [Column("bill_trip_id")]
    public int TripId { get; set; }

    [Column("bill_promocode_id")]
    public int PromocodeId { get; set; }

    [Column("bill_status_id")]
    public int StatusId { get; set; }

    [Column("bill_issue_date")]
    public DateTime IssueDate { get; set; }

    [Column("bill_amount")]
    public decimal Amount { get; set; }

    [Column("bill_remaining_amount")]
    public decimal RemainingAmount { get; set; }
}