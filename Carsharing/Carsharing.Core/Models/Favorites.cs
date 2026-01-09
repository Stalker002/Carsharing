namespace Carsharing.Core.Models;

public class Favorites
{
    private Favorites(int id, int clientId, int carId)
    {
        Id = id;
        ClientId = clientId;
        CarId = carId;
    }

    public int Id { get; }
    public int ClientId { get; }
    public int CarId { get; }

    public static (Favorites favorite, string error) Create(int id, int clientId, int carId)
    {
        var error = string.Empty;

        if (carId < 0)
            error = "Car Id must be positive";

        if (clientId < 0)
            error = "Client Id must be positive";

        var favorites = new Favorites(id, clientId, carId);

        return (favorites, error);
    }
}