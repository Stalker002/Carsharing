namespace Carsharing.DataAccess.Entites;

public class PromocodeStatusEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<PromocodeEntity> Promocodes { get; set; } = new List<PromocodeEntity>();
}