using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("insurance")]
public class InsuranceEntity
{
    [Column("insurance_id")]
    public int Id { get; set; }

    [Column("insurance_car_id")]
    public int CarId { get; set; }

    [Column("insurance_status_id")]
    public int StatusId { get; set; }

    [Column("insurance_type")]
    public string Type { get; set; }

    [Column("insurance_company")]
    public string Company { get; set; }

    [Column("insurance_policy_number")]
    public string PolicyNumber { get; set; }

    [Column("insurance_start_date")]
    public DateOnly StartDate { get; set; }

    [Column("insurance_end_date")]
    public DateOnly EndDate { get; set; }

    [Column("insurance_cost")]
    public decimal Cost { get; set; }

    public CarEntity? Car { get; set; }

    public StatusEntity? Status { get; set; }
}