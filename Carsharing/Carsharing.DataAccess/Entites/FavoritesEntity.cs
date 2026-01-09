namespace Carsharing.DataAccess.Entites;

public class FavoritesEntity
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int CarId { get; set; }

    public ClientEntity? Client { get; set; }
    public CarEntity? Car { get; set; }
}