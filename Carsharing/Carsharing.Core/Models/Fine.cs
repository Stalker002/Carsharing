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

    public int Id { get; }

    public int TripId { get; }

    public int StatusId { get; }

    public string Type { get; }

    public decimal Amount { get; }

    public DateOnly Date { get; }

    public static (Fine fine, string error) Create(int id, int tripId, int statusId, string type, decimal amount,
        DateOnly date)
    {
        var error = string.Empty;
        var allowedTypes = new[]
        {
            "Превышение скорости", "Нарушение правил парковки", "Несчастный случай",
            "Позднее возвращение", "Курение в машине", "Другое"
        };
        var allowedStatusTypes = new[] { 16, 17, 18 };

        if (tripId < 0)
            error = "Trip Id must be positive";
        
        if (!allowedStatusTypes.Contains(statusId))
            error = $"Invalid insurance type. Allowed: \"16. Начислен\", \"17. Ожидает оплаты\", \"18. Оплачен\" ";

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