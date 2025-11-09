namespace Carsharing.Core.Models;

public class Maintenance
{
    private Maintenance(int id, int carId, string workType, string description, decimal cost, DateOnly date)
    {
        Id = id;
        CarId = carId;
        WorkType = workType;
        Description = description;
        Cost = cost;
        Date = date;
    }

    public int Id { get; set; }

    public int CarId { get; set; }

    public string WorkType { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Cost { get; set; }

    public DateOnly Date { get; set; }

    public static (Maintenance maintenance, string error) Create(int id, int carId, string workType, string description,
        decimal cost, DateOnly date)
    {
        var error = string.Empty;
        var allowedWorkTypes = new[] { "Замена масла", "Замена шин", "Обслуживание тормозов", "Осмотр", "Ремонт", "Чистка" };
       
        if (carId < 0)
            error = "Car Id must be positive";

        if (!allowedWorkTypes.Contains(workType))
            error = $"Invalid work type. Allowed: {string.Join(", ", allowedWorkTypes)}";
        
        if (cost < 0)
            error = "Cost must be positive";

        var maintenance = new Maintenance(id, carId, workType, description, cost, date);

        return (maintenance, error);
    }
}