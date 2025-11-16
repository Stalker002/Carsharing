namespace Carsharing.Core.Models;

public class Bill
{
    private Bill(int id, int tripId, int? promocodeId, int statusId, DateTime issueDate, decimal? amount, 
        decimal? remainingAmount)
    {
        Id = id;
        TripId = tripId;
        PromocodeId = promocodeId;
        StatusId = statusId;
        IssueDate = issueDate;
        Amount = amount;
        RemainingAmount = remainingAmount;
    }

    public int Id { get; }
    public int TripId { get; }
    public int? PromocodeId { get; }
    public int StatusId { get; }    
    public DateTime IssueDate { get; }
    public decimal? Amount { get; }
    public decimal? RemainingAmount { get; }

    public static (Bill bill, string error) Create(int id, int tripId, int? promocodeId, int statusId,
        DateTime issueDate, decimal? amount, decimal? remainingAmount)
    {
        var error = string.Empty;

        if (tripId < 0)
            error = "Trip Id must be positive";

        if (promocodeId < 0)
            error = "Promo code Id must be positive";

        if (statusId < 0)
            error = "Status Id must be positive";
        // Прописать определенные статусы

        if (issueDate < DateTime.Now)
            error = "End bill date can not be in the past";

        if (amount < 0)
            error = "Amount must be positive";

        if (remainingAmount < 0)
            error = "Remaining amount must be positive";

        var bill = new Bill(id, tripId, promocodeId, statusId, issueDate, amount, remainingAmount);
        return (bill, error);
    }
}