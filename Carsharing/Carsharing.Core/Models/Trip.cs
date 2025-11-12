namespace Carsharing.Core.Models;

public class Trip
{
    private Trip(int id, int bookingId, int statusId, string tariffType, DateTime startTime, 
        DateTime endTime, decimal duration, decimal distance)
    {
        Id = id;
        BookingId = bookingId;
        StatusId = statusId;
        TariffType = tariffType;
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
        Distance = distance;
    }

    public int Id { get; }

    public int BookingId { get; }

    public int StatusId { get; }

    public string TariffType { get; }

    public DateTime StartTime { get; }

    public DateTime EndTime { get; }

    public decimal Duration { get; }

    public decimal Distance { get; }

    public static (Trip trip, string error) Create(int id, int bookingId, int statusId, string tariffType,
        DateTime startTime, DateTime endTime, decimal duration, decimal distance)
    {
        var error = string.Empty;
        var allowedTariffTypes = new[] { "per_minute", "per_km", "per_day" };

        if (bookingId < 0)
            error = "Booking Id must be positive";

        if (statusId < 0)
            error = "Status Id must be positive";
        // Прописать определенные статусы

        if (string.IsNullOrWhiteSpace(tariffType))
            error = "Tariff type can't be empty";
        if (!allowedTariffTypes.Contains(tariffType))
            error = $"Invalid tariff type. Allowed: {string.Join(", ", allowedTariffTypes)}";

        if (endTime < DateTime.Now)
            error = "End trip date can not be in the past";

        if (startTime > endTime)
            error = "Start time can not exceed end time ";

        if (duration < 0)
            error = "Duration must be positive";

        if (distance < 0)
            error = "Distance must be positive";

        var trip = new Trip(id, bookingId, statusId, tariffType, startTime, endTime, duration, distance);
        return (trip, error);
    }

}