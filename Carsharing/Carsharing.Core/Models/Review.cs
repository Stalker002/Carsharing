namespace Carsharing.Core.Models;

public class Review
{
    private Review(int id, int clientId, int carId, short rating, string comment, DateTime date)
    {
        Id = id;
        ClientId = clientId;
        CarId = carId;
        Rating = rating;
        Comment = comment;
        Date = date;
    }

    public int Id { get; set; }

    public int ClientId { get; set; }

    public int CarId { get; set; }

    public short Rating { get; set; }

    public string Comment { get; set; }

    public DateTime Date { get; set; }

    public static (Review review, string error) Create(int id, int clientId, int carId, short rating, string comment,
        DateTime date)
    {
        var error = string.Empty;

        if (clientId < 0)
            error = "Client Id must be positive";

        if (carId < 0)
            error = "Car Id must be positive";

        if (rating is < 0 or > 5)
            error = "The rating cannot be lower than 0 or higher than 5";

        if (date > DateTime.UtcNow)
            error = "Review date cannot be in the future";

        var review = new Review(id, clientId, carId, rating, comment, date);
        return (review, error);
    }
}