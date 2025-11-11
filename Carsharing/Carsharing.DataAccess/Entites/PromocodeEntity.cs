namespace Carsharing.DataAccess.Entites;

public class PromocodeEntity
{
    public int Id { get; set; }

    public int StatusId { get; set; }

    public string Code { get; set; }

    public decimal Discount { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public ICollection<BillEntity> Bill { get; set; } = new List<BillEntity>();

    public StatusEntity? Status { get; set; }
}