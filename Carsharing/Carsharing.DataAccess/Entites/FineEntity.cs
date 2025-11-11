using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("fines")]
public class FineEntity
{
    [Column("fine_id")]
    public int Id { get; set; }

    [Column("fine_trip_id")]
    public int TripId { get; set; }

    [Column("fine_status_id")]
    public int StatusId { get; set; }

    [Column("fine_type")]
    public string Type { get; set; }

    [Column("fine_amount")]
    public decimal Amount { get; set; }

    [Column("fine_date")]
    public DateOnly Date { get; set; }

    public TripEntity? Trip { get; set; }

    public StatusEntity? Status { get; set; }
}