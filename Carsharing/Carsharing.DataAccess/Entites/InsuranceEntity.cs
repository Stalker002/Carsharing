namespace Carsharing.DataAccess.Entites;

public class InsuranceEntity
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public int StatusId { get; set; }

    public string? Type { get; set; }

    public string? Company { get; set; }

    public string? PolicyNumber { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal Cost { get; set; }

    public CarEntity? Car { get; set; }

    public InsuranceStatusEntity? InsuranceStatus { get; set; }
}