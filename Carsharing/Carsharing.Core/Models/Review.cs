namespace Carsharing.Core.Models;

public class Review
{
    private Review(int id, int clientId, int carId, short rating, string? comment, DateTime date)
    {
        Id = id;
        ClientId = clientId;
        CarId = carId;
        Rating = rating;
        Comment = comment;
        Date = date;
    }

    public int Id { get; }

    public int ClientId { get; }

    public int CarId { get; }

    public short Rating { get; }

    public string? Comment { get; }

    public DateTime Date { get; }

    public static (Review review, string error) Create(int id, int clientId, int carId, short rating, string? comment,
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