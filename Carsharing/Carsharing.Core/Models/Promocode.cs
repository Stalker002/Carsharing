namespace Carsharing.Core.Models;

public class Promocode
{
    public const int MaxCodeLength = 50;

    private Promocode(int id, int statusId, string code, decimal discount, DateOnly startDate, DateOnly endDate)
    {
        Id = id;
        StatusId = statusId;
        Code = code;
        Discount = discount;
        StartDate = startDate;
        EndDate = endDate;
    }

    public int Id { get; }

    public int StatusId { get; }

    public string Code { get; }

    public decimal Discount { get; }

    public DateOnly StartDate { get; }

    public DateOnly EndDate { get; }

    public static (Promocode promocode, string error) Create(int id, int statusId, string code, decimal discount,
        DateOnly startDate, DateOnly endDate)
    {
        var error = string.Empty;
        var allowedStatuses = new[] { 19, 20, 21 };

        if (!allowedStatuses.Contains(statusId))
            error = $"Invalid insurance type. Allowed: \"19. Активен\", \"20. Истек\", \"21. Использован\" ";

        if (string.IsNullOrWhiteSpace(code))
            error = "Code can't be empty";
        if (code.Length > MaxCodeLength)
            error = $"Code can't be longer than {MaxCodeLength} symbols";

        if (discount < 0)
            error = "Discount must be positive";

        if (startDate > DateOnly.FromDateTime(DateTime.Now))
            error = "Start promo code date can not be in the future";

        if (endDate < DateOnly.FromDateTime(DateTime.Now))
            error = "End promo code date can not be in the past";

        if (startDate > endDate)
            error = "Start date can not exceed end date ";

        var promocode = new Promocode(id, statusId, code, discount, startDate, endDate);
        return (promocode, error);
    }
}