namespace Carsharing.DataAccess.Entites;

public class FineStatusEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<FineEntity> Fines { get; set; } = new List<FineEntity>();
}