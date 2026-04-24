using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace CarsharingMobile.Services;

public class ClientDocumentsService(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<ClientDocumentSummaryResponse>?> GetMyDocumentsAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("Clients/MyDocuments");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                SecureStorage.Default.Remove("tasty");
                MainThread.BeginInvokeOnMainThread(async () => { await Shell.Current.GoToAsync("//LoginPage"); });
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Ошибка загрузки документов: {response.StatusCode}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<ClientDocumentSummaryResponse>>(JsonOptions);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка загрузки документов: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> CreateDriverLicenseAsync(
        FileResult file,
        string number,
        string category,
        DateOnly issueDate,
        DateOnly expiryDate)
    {
        try
        {
            await using var fileStream = await file.OpenReadAsync();

            using var content = new MultipartFormDataContent();
            using var streamContent = new StreamContent(fileStream);

            streamContent.Headers.ContentType = new MediaTypeHeaderValue(GetContentType(file.FileName));

            content.Add(new StringContent("0"), "ClientId");
            content.Add(new StringContent("Водительское удостоверение"), "Type");
            content.Add(new StringContent(category), "LicenseCategory");
            content.Add(new StringContent(number), "Number");
            content.Add(new StringContent(issueDate.ToString("yyyy-MM-dd")), "IssueDate");
            content.Add(new StringContent(expiryDate.ToString("yyyy-MM-dd")), "ExpiryDate");
            content.Add(streamContent, "File", file.FileName);

            var response = await httpClient.PostAsync("ClientDocuments", content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                SecureStorage.Default.Remove("tasty");
                MainThread.BeginInvokeOnMainThread(async () => { await Shell.Current.GoToAsync("//LoginPage"); });
                return "Сессия истекла. Войдите снова.";
            }

            if (response.IsSuccessStatusCode)
                return null;

            var errorContent = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(errorContent) ? "Не удалось добавить водительские права." : errorContent;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка сохранения водительских прав: {ex}");
            return "Ошибка подключения к серверу";
        }
    }

    private static string GetContentType(string? fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };
    }
}

public class ClientDocumentSummaryResponse
{
    public int Id { get; init; }
    public int ClientId { get; init; }
    public string? Type { get; init; }
    public string? LicenseCategory { get; init; }
    public DateOnly IssueDate { get; init; }
    public DateOnly ExpiryDate { get; init; }
}
