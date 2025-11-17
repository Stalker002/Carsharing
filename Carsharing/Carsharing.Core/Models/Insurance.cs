namespace Carsharing.Core.Models;

public class Insurance
{
    public const int MaxCompanyLength = 100;
    public const int MaxPolicyNumberLength = 100;

    private Insurance(int id, int carId, int statusId, string type, string company, string policyNumber,
        DateOnly startDate, DateOnly endDate, decimal cost)
    {
        Id = id;
        CarId = carId;
        StatusId = statusId;
        Type = type;
        Company = company;
        PolicyNumber = policyNumber;
        StartDate = startDate;
        EndDate = endDate;
        Cost = cost;
    }

    public int Id { get; }

    public int CarId { get; }

    public int StatusId { get; }

    public string Type { get; }

    public string Company { get; }

    public string PolicyNumber { get; }

    public DateOnly StartDate { get; }

    public DateOnly EndDate { get; }

    public decimal Cost { get; }

    public static (Insurance insurance, string error) Create(int id, int carId, int statusId, string type,
        string company, string policyNumber, DateOnly startDate, DateOnly endDate, decimal cost)
    {
        var error = string.Empty;
        var allowedTypes = new[] { "ОСАГО", "КАСКО" };

        if (carId < 0)
            error = "Car Id must be positive";

        if (statusId < 0)
            error = "Status Id must be positive";
        // Прописать определенные статусы

        if (string.IsNullOrWhiteSpace(type))
            error = "Type can't be empty";
        if (!allowedTypes.Contains(type))
            error = $"Invalid insurance type. Allowed: {string.Join(", ", allowedTypes)}";

        if (string.IsNullOrWhiteSpace(company))
            error = "Company name can't be empty";
        if (company.Length > MaxCompanyLength)
            error = $"Company name can't be longer than {MaxCompanyLength} symbols";

        if (string.IsNullOrWhiteSpace(policyNumber))
            error = "Policy number can't be empty";
        if (policyNumber.Length > MaxPolicyNumberLength)
            error = $"Policy number can't be longer than {MaxPolicyNumberLength} symbols";

        if (startDate > DateOnly.FromDateTime(DateTime.Now))
            error = "Start insurance date can not be in the future";

        if (endDate < DateOnly.FromDateTime(DateTime.Now))
            error = "End insurance date can not be in the past";

        if (startDate > endDate)
            error = "Start time can not exceed end time ";

        if (cost < 0)
            error = "Cost must be positive";

        var insurance = new Insurance(id, carId, statusId, type, company, policyNumber, startDate, endDate, cost);
        return (insurance, error);
    }
}