using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("trip_details")]
public class TripDetailEntity
{
    [Column("trip_detail_id")]
    public int Id { get; set; }

    [Column("trip_id")]
    public int TripId { get; set; }

    [Column("trip_detail_start_location")]
    public string StartLocation { get; set; }

    [Column("trip_detail_end_location")]
    public string EndLocation { get; set; }

    [Column("trip_detail_insurance_active")]
    public bool InsuranceActive { get; set; } = false;

    [Column("trip_detail_fuel_used")]
    public decimal FuelUsed { get; set; }

    [Column("trip_detail_refueled")]
    public decimal Refueled { get; set; }
}