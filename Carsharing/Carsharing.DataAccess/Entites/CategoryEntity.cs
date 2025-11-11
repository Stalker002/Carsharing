namespace Carsharing.DataAccess.Entites;

public class CategoryEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<CarEntity> Cars { get; set; } = new List<CarEntity>();
}