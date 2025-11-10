using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("maintenance")]
public class MaintenanceEntity
{
    [Column("maintenance_id")]
    public int Id { get; set; }

    [Column("maintenance_car_id")]
    public int CarId { get; set; }

    [Column("maintenance_work_type")]
    public string WorkType { get; set; }

    [Column("maintenance_description")]
    public string Description { get; set; }

    [Column("maintenance_cost")]
    public decimal Cost { get; set; }

    [Column("maintenance_date")]
    public DateOnly Date { get; set; }
}