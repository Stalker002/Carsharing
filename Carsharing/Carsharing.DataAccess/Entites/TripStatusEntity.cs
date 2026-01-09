namespace Carsharing.DataAccess.Entites;

public class TripStatusEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<TripEntity> Trip { get; set; } = new List<TripEntity>();
}