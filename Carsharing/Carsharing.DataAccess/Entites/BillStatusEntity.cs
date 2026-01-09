namespace Carsharing.DataAccess.Entites;

public class BillStatusEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<BillEntity> Bills { get; set; } = new List<BillEntity>();
}