using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("promocodes")]
public class PromocodeEntity
{
    [Column("promocode_id")]
    public int Id { get; set; }

    [Column("promocode_status_id")]
    public int StatusId { get; set; }

    [Column("promocode_code")]
    public string Code { get; set; }

    [Column("promocode_discount")]
    public decimal Discount { get; set; }

    [Column("promocode_start_date")]
    public DateOnly StartDate { get; set; }

    [Column("promocode_end_date")]
    public DateOnly EndDate { get; set; }

    public ICollection<BillEntity> Bill { get; set; } = new List<BillEntity>();

    public StatusEntity? Status { get; set; }
}