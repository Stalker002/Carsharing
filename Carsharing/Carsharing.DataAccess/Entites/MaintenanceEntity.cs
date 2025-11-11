namespace Carsharing.DataAccess.Entites;

public class MaintenanceEntity
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public string WorkType { get; set; }

    public string Description { get; set; }

    public decimal Cost { get; set; }

    public DateOnly Date { get; set; }

    public CarEntity? Car { get; set; }
}