namespace Carsharing.Core.Models;

public class Payment
{
    private Payment(int id, int billId, decimal sum, string method, DateTime date)
    {
        Id = id;
        BillId = billId;
        Sum = sum;
        Method = method;
        Date = date;
    }

    public int Id { get; set; }

    public int BillId { get; set; }

    public decimal Sum { get; set; }

    public string Method { get; set; }

    public DateTime Date { get; set; }

    public static (Payment payment, string error) Create(int id, int billId, decimal sum, string method, DateTime date)
    {
        var error = string.Empty;
        var allowedMethod = new[] { "Картой", "Наличными", "ЕРИП", "Другое" };

        if (billId < 0)
            error = "Bill Id must be positive";

        if (sum < 0)
            error = "Sum must be positive";

        if (string.IsNullOrWhiteSpace(method))
            error = "Method can't be empty";
        if (!allowedMethod.Contains(method))
            error = $"Invalid method. Allowed: {string.Join(", ", allowedMethod)}";

        var payment = new Payment(id, billId, sum, method, date);
        return (payment, error);
    }
}