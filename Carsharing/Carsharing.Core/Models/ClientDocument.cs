using System.Text.RegularExpressions;

namespace Carsharing.Core.Models;

public class ClientDocument
{
    public const int MaxTypeLength = 50;
    public const int MaxLicenseCategoryLength = 50;
    public const int MaxNumberLength = 50;
    public const int MaxFilePathLength = 255;

    private ClientDocument(int id, int clientId, string type, string? licenseCategory, string number,
        DateOnly issueDate, DateOnly expiryDate,
        string filePath)
    {
        Id = id;
        ClientId = clientId;
        Type = type;
        LicenseCategory = licenseCategory;
        Number = number;
        IssueDate = issueDate;
        ExpiryDate = expiryDate;
        FilePath = filePath;
    }

    public int Id { get; }
    public int ClientId { get; }
    public string Type { get; } = string.Empty;
    public string? LicenseCategory { get; } = string.Empty;
    public string Number { get; } = string.Empty;
    public DateOnly IssueDate { get; }
    public DateOnly ExpiryDate { get; }
    public string FilePath { get; } = string.Empty;

    public static (ClientDocument clientDocument, string error) Create(int id, int clientId, string type,
        string? licenseCategory, string number, DateOnly issueDate, DateOnly expiryDate, string? filePath)
    {
        var error = string.Empty;
        var allowedTypes = new[] { "Водительские права", "Паспорт" };

        if (clientId < 0)
            error = "Client Id must be positive";

        if (string.IsNullOrWhiteSpace(type))
            error = "Type can't be empty";
        if (type.Length > MaxTypeLength)
            error = $"Type can't be longer than {MaxTypeLength} symbols";
        if (!allowedTypes.Contains(type))
            error = $"Invalid insurance type. Allowed: {string.Join(", ", allowedTypes)}";

        if (string.IsNullOrEmpty(licenseCategory))
            error = "Category can't be empty";
        if (licenseCategory is { Length: > MaxLicenseCategoryLength })
            error = $"Category can't be longer than {MaxLicenseCategoryLength} symbols";

        if (type == "Водительские права")
        {
            var cleanNumber = number.Replace(" ", "").ToUpper();
            if (!Regex.IsMatch(cleanNumber, @"^[A-ZА-Я]{2}\d{7}$"))
                error = "Неверный формат прав. Ожидается: AA 0000000 (2 буквы, 7 цифр)";
        }

        if (string.IsNullOrWhiteSpace(number))
            error = "Number can't be empty";
        if (number.Length > MaxNumberLength)
            error = $"Number can't be longer than {MaxNumberLength} symbols";

        if (issueDate > DateOnly.FromDateTime(DateTime.Now))
            error = "The date of adding the document cannot be in the future";

        if (expiryDate < DateOnly.FromDateTime(DateTime.Now))
            error = "The expiration date of the document cannot be in the past";

        if (issueDate > expiryDate)
            error = "Start time can not exceed end time ";

        if (string.IsNullOrWhiteSpace(filePath))
            error = "File path can't be empty";
        if (filePath.Length > MaxFilePathLength)
            error = $"Type can't be longer than {MaxFilePathLength} symbols";

        var clientDocument =
            new ClientDocument(id, clientId, type, licenseCategory, number.ToUpper(), issueDate, expiryDate, filePath);

        return (clientDocument, error);
    }
}