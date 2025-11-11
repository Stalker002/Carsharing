namespace Carsharing.Core.Models;

public class Booking
{
    private Booking(int id, int statusId, int carId, int clientId, 
        DateTime startTime, DateTime endTime)
    {
        Id = id;
        StatusId = statusId;
        CarId = carId;
        ClientId = clientId;
        StartTime = startTime;
        EndTime = endTime;
    }

    public int Id { get; }

    public int StatusId { get; }

    public int CarId { get; }

    public int ClientId { get; }

    public DateTime StartTime { get; }

    public DateTime EndTime { get; }

    public static (Booking booking, string error) Create(int id, int statusId, int carId, int clientId,
        DateTime startTime, DateTime endTime)
    {
        var error = string.Empty;

        if (statusId < 0)
            error = "Status Id must be positive";
        // Прописать определенные статусы

        if (carId < 0)
            error = "Car Id must be positive";

        if (clientId < 0)
            error = "Client Id must be positive";

        if (startTime > DateTime.Now)
            error = "Start booking time can not be in the future";

        if (endTime < DateTime.Now)
            error = "End booking time can not be in the past";

        var booking = new Booking(id, statusId, carId, clientId, startTime, endTime);
        return (booking, error);
    }
}