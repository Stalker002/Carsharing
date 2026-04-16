using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CarsharingMobile.Extensions;

public static class HttpClientExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<(List<TResponse>? Items, int TotalCount)> GetPagedAsync<TResponse>(
        this HttpClient httpClient, string url)
    {
        try
        {
            var response = await httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                SecureStorage.Default.Remove("tasty");
                MainThread.BeginInvokeOnMainThread(async () => { await Shell.Current.GoToAsync("//LoginPage"); });
                return (null, 0);
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[API Error] GET {url} | Status: {response.StatusCode} | Body: {errorBody}");
                return (null, 0);
            }

            var totalCount = 0;
            if (response.Headers.TryGetValues("x-total-count", out var values))
                int.TryParse(values.FirstOrDefault(), out totalCount);

            var items = await response.Content.ReadFromJsonAsync<List<TResponse>>(JsonOptions);
            return (items, totalCount);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching {typeof(TResponse).Name}: {ex}");
            return (null, 0);
        }
    }
}