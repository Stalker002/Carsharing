namespace Carsharing.Core.Models;

public class TripDetail
{
    private const int MaxLocationLength = 50;
    private TripDetail(int id, int tripId, string startLocation, string endLocation, bool insuranceActive, 
        decimal fuelUsed, decimal refueled)
    {
        Id = id;
        TripId = tripId;
        StartLocation = startLocation;
        EndLocation = endLocation;
        InsuranceActive = insuranceActive;
        FuelUsed = fuelUsed;
        Refueled = refueled;
    }

    public int Id { get; }

    public int TripId { get; }

    public string StartLocation { get; }

    public string EndLocation { get; }

    public bool InsuranceActive { get; } = false;

    public decimal FuelUsed { get; }

    public decimal Refueled { get; }

    public static (TripDetail tripDetail, string error) Create(int id, int tripId, string startLocation,
        string endLocation, bool insuranceActive, decimal fuelUsed, decimal refueled)
    {
        var error = string.Empty;

        if (tripId < 0)
            error = "Trip Id must be positive";

        if (string.IsNullOrWhiteSpace(startLocation))
            error = "Start location can't be empty";
        if (startLocation.Length > MaxLocationLength)
            error = $"Start location can't be longer than {MaxLocationLength} symbols";

        if (string.IsNullOrWhiteSpace(endLocation))
            error = "End location can't be empty";
        if (endLocation.Length > MaxLocationLength)
            error = $"End location can't be longer than {MaxLocationLength} symbols";

        if (fuelUsed < 0)
            error = "Fuel used must be positive";

        if (refueled < 0)
            error = "Refueled must be positive";

        var tripDetail = new TripDetail(id, tripId, startLocation, endLocation, insuranceActive, fuelUsed, refueled);
        return (tripDetail, error);

    }
}