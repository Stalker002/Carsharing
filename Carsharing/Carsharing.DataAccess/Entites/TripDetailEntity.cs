namespace Carsharing.DataAccess.Entites;

public class TripDetailEntity
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public string? StartLocation { get; set; }

    public string? EndLocation { get; set; }

    public bool InsuranceActive { get; set; }

    public decimal? FuelUsed { get; set; }

    public decimal? Refueled { get; set; }

    public TripEntity? Trip { get; set; }
}