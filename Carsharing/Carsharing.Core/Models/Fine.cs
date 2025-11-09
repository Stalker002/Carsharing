namespace Carsharing.Core.Models;

public class Fine
{
    private Fine(int id, int tripId, int statusId, string type, decimal amount, 
        DateOnly date)
    {
        Id = id;
        TripId = tripId;
        StatusId = statusId;
        Type = type;
        Amount = amount;
        Date = date;
    }

    public int Id { get; set; }

    public int TripId { get; set; }

    public int StatusId { get; set; }

    public string Type { get; set; }

    public decimal Amount { get; set; }

    public DateOnly Date { get; set; }

    public static (Fine fine, string error) Create(int id, int tripId, int statusId, string type, decimal amount,
        DateOnly date)
    {
        var error = string.Empty;
        var allowedTypes = new[] { "Превышение скорости", "Нарушение правил парковки", "Несчастный случай", 
            "Позднее возвращение", "Курение в машине", "Другое" };

        if (tripId < 0)
            error = "Trip Id must be positive";

        if (statusId < 0)
            error = "Status Id must be positive";
        // Прописать определенные статусы

        if (string.IsNullOrWhiteSpace(type))
            error = "Fine type can't be empty";
        if (!allowedTypes.Contains(type))
            error = $"Invalid fine type. Allowed: {string.Join(", ", allowedTypes)}";

        if (amount < 0)
            error = "Cost must be positive";

        if (date > DateOnly.FromDateTime(DateTime.Now))
            error = "Fine date can not be in the future";

        var fine = new Fine(id, tripId, statusId, type, amount, date);
        return (fine, error);
    }
}